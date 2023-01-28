using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

using Rubik.Theme.UI;

namespace Rubik.Demo.MediaPlayer.ViewModels
{
    public class MainControlViewModel : BindableBase
    {
        public DelegateCommand OpenWpfCommand { get; private set; }
        public DelegateCommand OpenVlcCommand { get; private set; }

        public MainControlViewModel()
        {
            OpenWpfCommand = new DelegateCommand(OpenWpf);
            OpenVlcCommand = new DelegateCommand(OpenVlc);
        }

        private string _wpfVideoFile;
        public string WpfVideoFile
        {
            get { return _wpfVideoFile; }
            set { SetProperty(ref _wpfVideoFile, value); }
        }

        private string _vlcVideoFile;
        public string VlcVideoFile
        {
            get { return _vlcVideoFile; }
            set { SetProperty(ref _vlcVideoFile, value); }
        }

        private void OpenWpf()
        {
            var videoFile = DialogUtil.ShowOpenFileDialog("Video Files|*.*");
            if (string.IsNullOrWhiteSpace(videoFile))
                return;

            WpfVideoFile = videoFile;
        }

        private void OpenVlc()
        {
            var videoFile = DialogUtil.ShowOpenFileDialog("Video Files|*.*");
            if (string.IsNullOrWhiteSpace(videoFile))
                return;

            VlcVideoFile = videoFile;
        }
    }
}
