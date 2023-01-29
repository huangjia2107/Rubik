using System;
using System.Buffers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Prism.Commands;
using Prism.Mvvm;

using Rubik.Comm.Udp;
using Rubik.Comm.Udp.Models;
using Rubik.Module.UdpTester.Views;
using Rubik.Toolkit.Extensions;

namespace Rubik.Module.UdpTester.ViewModels
{
    public class UdpTesterControlViewModel : BindableBase
    {
        private ArrayPool<byte> _arrayPool = null;
        private UdpEndpoint _udpEndpoint = null;

        private ScrollViewer _receiveScrollViewer = null;

        public DelegateCommand<RoutedEventArgs> LoadedCommand { get; private set; }
        public DelegateCommand StartListenCommand { get; private set; }
        public DelegateCommand StopListenCommand { get; private set; }
        public DelegateCommand ClearReceivedCountCommand { get; private set; }

        public DelegateCommand SendEncodeCommand { get; private set; }
        public DelegateCommand StartSendCommand { get; private set; }
        public DelegateCommand StopSendCommand { get; private set; }
        public DelegateCommand ClearSendCountCommand { get; private set; }

        public UdpTesterControlViewModel()
        {
            _arrayPool = ArrayPool<byte>.Shared;

            _udpEndpoint = new UdpEndpoint();
            _udpEndpoint.DatagramReceived += Udp_DatagramReceived;

            InitCommand();
        }

        #region Init

        private void InitCommand()
        {
            LoadedCommand = new DelegateCommand<RoutedEventArgs>(Loaded);
            StartListenCommand = new DelegateCommand(StartListen);
            StopListenCommand = new DelegateCommand(StopListen);
            ClearReceivedCountCommand = new DelegateCommand(ClearReceivedCount);
            SendEncodeCommand = new DelegateCommand(SendEncode);
            StartSendCommand = new DelegateCommand(StartSend);
            StopSendCommand = new DelegateCommand(StopSend);
            ClearSendCountCommand = new DelegateCommand(ClearSendCount);
        }

        #endregion

        #region Address

        private string _ip = "127.0.0.1";
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; RaisePropertyChanged(); }
        }

        private int _port = 10000;
        public string Port
        {
            get { return $"{_port}"; }
            set
            {
                if (int.TryParse(value, out int p))
                    _port = Math.Min(65535, Math.Max(0, p));

                RaisePropertyChanged();
            }
        }

        #endregion

        #region Receive

        public ObservableCollection<string> ReceivedCollection { get; } = new ObservableCollection<string>();

        private int _receivedCount;
        public int ReceivedCount
        {
            get { return _receivedCount; }
            set { _receivedCount = value; RaisePropertyChanged(); }
        }

        private bool _receivedStringSwitch = true;
        public bool ReceivedStringSwitch
        {
            get { return _receivedStringSwitch; }
            set { _receivedStringSwitch = value; RaisePropertyChanged(); }
        }

        private int _receivedLatest = 100;
        public string ReceivedLatest
        {
            get { return $"{_receivedLatest}"; }
            set
            {
                if (int.TryParse(value, out int latest))
                    _receivedLatest = Math.Max(0, latest);

                RaisePropertyChanged();
            }
        }

        private bool _isListening;
        public bool IsListening
        {
            get { return _isListening; }
            set { _isListening = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Send

        private string _sendText = "https://github.com/huangjia2107/Wave";
        public string SendText
        {
            get { return _sendText; }
            set { _sendText = value; RaisePropertyChanged(); }
        }

        private int _sendCount;
        public int SendCount
        {
            get { return _sendCount; }
            set { _sendCount = value; RaisePropertyChanged(); }
        }

        private bool _sendStringSwitch = true;
        public bool SendStringSwitch
        {
            get { return _sendStringSwitch; }
            set { _sendStringSwitch = value; RaisePropertyChanged(); }
        }

        private int _sendTimes = 1;
        public string SendTimes
        {
            get { return $"{_sendTimes}"; }
            set
            {
                if (int.TryParse(value, out int t))
                    _sendTimes = Math.Max(1, t);

                RaisePropertyChanged();
            }
        }

        private int _sendInterval = 1000;
        public string SendInterval
        {
            get { return $"{_sendInterval}"; }
            set
            {
                if (int.TryParse(value, out int i))
                    _sendInterval = Math.Max(0, i);

                RaisePropertyChanged();
            }
        }

        private bool _isSending;
        public bool IsSending
        {
            get { return _isSending; }
            set { _isSending = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Command

        private void Loaded(RoutedEventArgs e)
        {
            var mainControl = e.OriginalSource as UdpTesterControl;
            _receiveScrollViewer = mainControl?.ReceiveScrollViewer;
        }

#if NET6_0_OR_GREATER
        private async void StartListen()
        {
            try
            {
                await _udpEndpoint.Start(Ip, _port).ConfigureAwait(false);
#else
        private void StartListen()
        {
            try
            {
                _udpEndpoint.Start(Ip, _port);
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UDP", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsListening = _udpEndpoint.IsListening;
            }
        }

        private void StopListen()
        {
            _udpEndpoint.Stop();
            IsListening = _udpEndpoint.IsListening;
        }

        private void ClearReceivedCount()
        {
            ReceivedCount = 0;
        }

        private void Udp_DatagramReceived(object sender, Datagram e)
        {
            var ros = new ReadOnlySequence<byte>(e.Data);
            var byteCount = e.Data.Length;

            ReceivedCount++;

            if (ReceivedStringSwitch)
            {
                var receivedBytes = _arrayPool.Rent(byteCount);
                ros.CopyAll(receivedBytes, 0);

                var result = Encoding.UTF8.GetString(receivedBytes, 0, byteCount);
                _arrayPool.Return(receivedBytes);

                ShowReceivedBytes(result);
            }
            else
            {
                ShowReceivedBytes(ros.ToHexString());
            }
        }

#endregion

#region Send

        private void SendEncode()
        {
            SendText = SendText?.Trim();

            if (!string.IsNullOrWhiteSpace(SendText))
                SendText = Regex.Replace(SendText, @"\s+", " ");

            if (string.IsNullOrWhiteSpace(SendText))
                return;

            if (!SendStringSwitch)
            {
                if (!Regex.IsMatch(SendText, @"^[0-9a-fA-F]{2}(\s{1}[0-9a-fA-F]{2})*$"))
                    SendText = BitConverter.ToString(Encoding.UTF8.GetBytes(SendText)).Replace("-", " ").ToUpper();

                return;
            }

            if (Regex.IsMatch(SendText, @"^[0-9a-fA-F]{2}(\s{1}[0-9a-fA-F]{2})*$"))
            {
                SendText = Encoding.UTF8.GetString(SendText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(h => Convert.ToByte(h, 16)).ToArray());
            }
        }

        private async void StartSend()
        {
            await Task.Run(async () =>
            {
                IsSending = true;

                for (var i = 0; i < _sendTimes; i++)
                {
                    if (!IsSending)
                        break;

                    if (SendStringSwitch)
                    {
                        await _udpEndpoint.SendAsync(Ip, _port, SendText);
                        SendCount++;

                        await Task.Delay(_sendInterval);
                        continue;
                    }

                    await _udpEndpoint.SendAsync(Ip, _port, SendText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(h => Convert.ToByte(h, 16)).ToArray());
                    SendCount++;

                    await Task.Delay(_sendInterval);
                }

                IsSending = false;
            });
        }

        private void StopSend()
        {
            IsSending = false;
        }

        private void ClearSendCount()
        {
            SendCount = 0;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _udpEndpoint?.Stop();
            _udpEndpoint?.Dispose();
        }

#endregion

#region Func

        private void ShowReceivedBytes(string result)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                while (ReceivedCollection.Count >= _receivedLatest)
                    ReceivedCollection.RemoveAt(0);

                ReceivedCollection.Add(result);
                _receiveScrollViewer.ScrollToBottom();
            });
        }

#endregion
    }
}
