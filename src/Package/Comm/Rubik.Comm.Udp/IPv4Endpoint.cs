using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace Rubik.Comm.Udp
{
    public class IPv4Endpoint : IDisposable
    {
        private bool _disposed = false;
        private SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        private Socket _socketSender;
        private IEnumerable<LibPcapLiveDevice> _devices;

        public IPv4Endpoint()
        {
#if Win
            _socketSender = new Socket(AddressFamily.InterNetwork, SocketType.Raw, System.Net.Sockets.ProtocolType.IP);
#else
            _socket = new Socket((AddressFamily)2, (SocketType)3, (ProtocolType)255);
#endif
            _socketSender.Bind(new IPEndPoint(IPAddress.Any, 0));
            _socketSender.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

        }

        #region Property

        public bool IsCapturing { get; private set; }

        #endregion

        #region Event

        public event EventHandler<byte[]> BytesReceived;

        #endregion

        #region Receive

        public string StartCapture(string ip, bool lookback = true, string filter = null, Predicate<LibPcapLiveDevice> predicate = null)
        {
            filter ??= $"ip and not tcp and dst host {ip}";

            if (!PcapDevice.CheckFilter(filter, out string error))
                return $"Filter is invalid: {error}";

            try
            {
                _devices = LibPcapLiveDeviceList.Instance.Where(d =>
                {
                    if (predicate is not null)
                        return predicate(d);

                    return lookback && d.Loopback || d.Addresses.Any(a => a.Addr.ipAddress is not null && a.Addr.ipAddress.ToString() == ip);
                });

                if (_devices is null)
                    return "Devices is null";

                foreach (var device in _devices)
                {
                    device.Open();

                    device.Filter = filter;
                    device.OnPacketArrival += Device_OnPacketArrival;

                    device.StartCapture();
                }

                IsCapturing = true;
            }
            catch (Exception ex)
            {
                IsCapturing = false;
                DisposeCapture();

                return ex.Message;
            }

            return null;
        }

        public void StopCapture()
        {
            if (!IsCapturing)
                return;

            DisposeCapture();

            IsCapturing = false;
        }

        private void Device_OnPacketArrival(object sender, PacketCapture e)
        {
            var packet = e.GetPacket()?.GetPacket();
            if (packet is null)
                return;

            if (!packet.HasPayloadPacket || packet.PayloadPacket is not IPv4Packet iPv4Packet)
                return;

            BytesReceived?.Invoke(this, iPv4Packet.Bytes);
        }

        #endregion

        #region Send

        public void Send(string ip, int port, string text, short ttl = 64, Action<Socket> config = null)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));

            var data = Encoding.UTF8.GetBytes(text);
            Send(ip, port, data, 0, data.Length, ttl, config);
        }

        public void Send(string ip, int port, byte[] data, short ttl = 64, Action<Socket> config = null)
        {
            Send(ip, port, data, 0, data.Length, ttl, config);
        }

        public void Send(string ip, int port, byte[] data, int bytes, short ttl = 64, Action<Socket> config = null)
        {
            Send(ip, port, data, 0, bytes, ttl, config);
        }

        public void Send(string ip, int port, byte[] data, int offset, int bytes, short ttl = 64, Action<Socket> config = null)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException(nameof(ip));

            if (port < 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (bytes <= 0 || bytes > data.Length)
                throw new ArgumentOutOfRangeException(nameof(bytes));

            if (ttl < 0)
                throw new ArgumentOutOfRangeException(nameof(ttl));

            SendInternal(ip, port, data, offset, bytes, ttl, config);
        }

        private void SendInternal(string ip, int port, byte[] data, int offset, int bytes, short ttl = 64, Action<Socket> config = null)
        {
            _sendLock.Wait();

            var ipe = new IPEndPoint(IPAddress.Parse(ip), port);

            try
            {
                config?.Invoke(_socketSender);

                _socketSender.Ttl = ttl;
                _socketSender.SendTo(data, offset, bytes, SocketFlags.None, ipe);
            }
            finally
            {
                _sendLock.Release();
            }
        }

#if NET6_0_OR_GREATER

        public void Send(string ip, int port, ReadOnlySpan<byte> data, short ttl = 64, Action<Socket> config = null)
        {
            _sendLock.Wait();

            var ipe = new IPEndPoint(IPAddress.Parse(ip), port);

            try
            {
                config?.Invoke(_socketSender);

                _socketSender.Ttl = ttl;
                _socketSender.SendTo(data, ipe);
            }
            finally
            {
                _sendLock.Release();
            }
        }

#endif

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
                StopCapture();

                _socketSender?.Dispose();
                _socketSender = null;

                _sendLock?.Dispose();
                _sendLock = null;
            }

            _disposed = true;
        }

        private void DisposeCapture()
        {
            if (_devices is not null)
            {
                foreach (var device in _devices)
                {
                    device.OnPacketArrival -= Device_OnPacketArrival;

                    device.StopCapture();
                    device.Close();
                    device.Dispose();
                }

                _devices = null;
            }
        }

        #endregion
    }
}
