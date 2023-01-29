using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Rubik.Comm.Udp.Models;
using Rubik.Toolkit.CancellationTokens;

namespace Rubik.Comm.Udp
{
    public class UdpEndpoint : IDisposable
    {
        private Socket _serverSocket = null;
        private SocketAsyncEventArgs _recvArgs = null;

        private byte[] _receivedBuffer = null;
        private ArrayPool<byte> _arrayPool = null;

        private bool _disposed = false;
        private ReusableCancellationTokenProvider _cancellationTokenProvider = null;

        private UdpClient _udpClient = null;
        private SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        private readonly EndPoint _blankEndpoint = new IPEndPoint(IPAddress.Any, 0);

        public UdpEndpoint()
        {
            _arrayPool = ArrayPool<byte>.Shared;
            _cancellationTokenProvider = new ReusableCancellationTokenProvider();

            _udpClient = new UdpClient();
        }

        #region Event

        public event EventHandler<Datagram> DatagramReceived;

        #endregion

        #region Property

        public bool IsListening { get; private set; }

        private int _maxDatagramSize = 65527;
        public int MaxDatagramSize
        {
            get { return _maxDatagramSize; }
            set { _maxDatagramSize = Math.Min(65527, Math.Max(1, value)); }
        }

        #endregion

        #region Receive

#if NET6_0_OR_GREATER
        public async Task Start(string ip, int port, Action<Socket> config = null)
#else
        public void Start(string ip, int port, Action<Socket> config = null)
#endif
        {
            if (string.IsNullOrWhiteSpace(ip))
                throw new ArgumentNullException(nameof(ip));

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
                throw new ArgumentOutOfRangeException($"{nameof(port)} is less than {IPEndPoint.MinPort} or {nameof(port)} is greater than {IPEndPoint.MaxPort}.");

            if (!IPAddress.TryParse(ip, out IPAddress address))
                throw new ArgumentException($"{nameof(ip)} is invalid.");

            try
            {
                IsListening = true;

                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _serverSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);

                config?.Invoke(_serverSocket);

                _serverSocket.Bind(new IPEndPoint(address, port));

#if NET6_0_OR_GREATER
                await StartReceiving().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsListening = false;
            }
#else
                StartReceiving();
            }
            catch (Exception ex)
            {
                IsListening = false;
                throw ex;
            }
#endif
        }

        public void Stop()
        {
            if (!IsListening)
                return;

            _cancellationTokenProvider.Cancel();
            DisposeServer();

            IsListening = false;
        }

#if NET6_0_OR_GREATER
        private async Task StartReceiving()
#else
        private void StartReceiving()
#endif
        {
            var datagramSize = MaxDatagramSize;
            _receivedBuffer = _arrayPool.Rent(datagramSize);

#if NET6_0_OR_GREATER
            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    Memory<byte> memory = _receivedBuffer;

                    while (!_cancellationTokenProvider.IsCancellationRequested)
                    {
                        var result = await _serverSocket.ReceiveFromAsync(memory, SocketFlags.None, _blankEndpoint, _cancellationTokenProvider.CancellationToken.Token);

                        if (result.ReceivedBytes > 0)
                        {
                            DatagramReceived?.Invoke(this, new Datagram(result.RemoteEndPoint as IPEndPoint, memory.Slice(0, Math.Min(result.ReceivedBytes, datagramSize))));
                        }
                    }
                }
                catch
                {
                    IsListening = false;
                    DisposeServer();
                }
            }, _cancellationTokenProvider.CancellationToken.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            if (_cancellationTokenProvider.IsCancellationRequested)
                _cancellationTokenProvider.Reset();
#else
            _recvArgs ??= new SocketAsyncEventArgs
            {
                RemoteEndPoint = _blankEndpoint,
                UserToken = datagramSize
            };

            _recvArgs.SetBuffer(_receivedBuffer, 0, datagramSize);
            _recvArgs.Completed -= Args_Completed;
            _recvArgs.Completed += Args_Completed;

            if (!_serverSocket.ReceiveFromAsync(_recvArgs))
            {
                Args_Completed(this, _recvArgs);
            }
#endif
        }

        private void Args_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (_cancellationTokenProvider.IsCancellationRequested)
            {
                _cancellationTokenProvider.Reset();
                return;
            }

            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                var datagramSize = (int)e.UserToken;
                DatagramReceived?.Invoke(this, new Datagram(e.RemoteEndPoint as IPEndPoint, e.MemoryBuffer.Slice(0, Math.Min(e.BytesTransferred, datagramSize))));

                if (!_serverSocket.ReceiveFromAsync(_recvArgs))
                {
                    Args_Completed(this, _recvArgs);
                }
            }
        }

        #endregion

        #region Send

        public void Send(string ip, int port, string text, short ttl = 64, Action<UdpClient> config = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            var data = Encoding.UTF8.GetBytes(text);
            Send(ip, port, data, data.Length, ttl, config);
        }

        public void Send(string ip, int port, byte[] data, short ttl = 64, Action<UdpClient> config = null)
        {
            Send(ip, port, data, data.Length, ttl, config);
        }

        public void Send(string ip, int port, byte[] data, int bytes, short ttl = 64, Action<UdpClient> config = null)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException(nameof(ip));

            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (bytes <= 0 || bytes > data.Length || bytes > MaxDatagramSize)
                throw new ArgumentOutOfRangeException(nameof(bytes));

            if (ttl < 0)
                throw new ArgumentOutOfRangeException(nameof(ttl));

            SendInternal(ip, port, data, bytes, ttl, config);
        }

        public async Task SendAsync(string ip, int port, string text, short ttl = 64, Action<UdpClient> config = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            var data = Encoding.UTF8.GetBytes(text);
            await SendAsync(ip, port, data, data.Length, ttl, config).ConfigureAwait(false);
        }

        public async Task SendAsync(string ip, int port, byte[] data, short ttl = 64, Action<UdpClient> config = null)
        {
            await SendAsync(ip, port, data, data.Length, ttl, config).ConfigureAwait(false);
        }

        public async Task SendAsync(string ip, int port, byte[] data, int bytes, short ttl = 64, Action<UdpClient> config = null)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException(nameof(ip));

            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (bytes <= 0 || bytes > data.Length || bytes > MaxDatagramSize)
                throw new ArgumentOutOfRangeException(nameof(bytes));

            if (ttl < 0)
                throw new ArgumentOutOfRangeException(nameof(ttl));

            await SendInternalAsync(ip, port, data, bytes, ttl, config).ConfigureAwait(false);
        }

        private void SendInternal(string ip, int port, byte[] data, int bytes, short ttl, Action<UdpClient> config = null)
        {
            _sendLock.Wait();

            var ipe = new IPEndPoint(IPAddress.Parse(ip), port);

            try
            {
                config?.Invoke(_udpClient);

                _udpClient.Ttl = ttl;
                _udpClient.Send(data, bytes, ipe);
            }
            finally
            {
                _sendLock.Release();
            }
        }

        private async Task SendInternalAsync(string ip, int port, byte[] data, int bytes, short ttl, Action<UdpClient> config = null)
        {
            await _sendLock.WaitAsync();

            var ipe = new IPEndPoint(IPAddress.Parse(ip), port);

            try
            {
                config?.Invoke(_udpClient);

                _udpClient.Ttl = ttl;
                await _udpClient.SendAsync(data, bytes, ipe).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {

            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                _sendLock.Release();
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Stop();

                _udpClient?.Dispose();
                _udpClient = null;

                _sendLock?.Dispose();
                _sendLock = null;
            }

            _disposed = true;
        }

        private void DisposeServer()
        {
            _serverSocket?.Close();
            _serverSocket = null;

            if (_receivedBuffer != null)
                _arrayPool.Return(_receivedBuffer);
        }

        #endregion
    }
}
