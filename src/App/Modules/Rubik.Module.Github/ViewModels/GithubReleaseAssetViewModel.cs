using System;
using System.Diagnostics;

using Prism.Commands;
using Prism.Mvvm;

using Rubik.Service.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubReleaseAssetViewModel : BindableBase
    {
        private GithubReleaseAssetModel _model = null;
        private Action<string> _downloadAction = null;

        public DelegateCommand DownloadCommand { get; private set; }

        public GithubReleaseAssetViewModel(GithubReleaseAssetModel model, Action<string> downloadAction = null)
        {
            _model = model;
            _downloadAction = downloadAction;

            DownloadCommand = new DelegateCommand(Download);
        }

        public string Name => _model.name;
        public int DownloadCount => _model.download_count;
        public string Size => $"{_model.size * 1.0 / 1024 / 1024:#0.00} MB";
        public string DownloadUrl => _model.browser_download_url;

        private void Download()
        {
            if (_downloadAction == null)
            {
                Process.Start(new ProcessStartInfo(DownloadUrl) { UseShellExecute = true });
                return;
            }

            _downloadAction?.Invoke(DownloadUrl);
        }
    }
}
