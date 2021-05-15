using Prism.Mvvm;
using Rubik.Service.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubReleaseAssetViewModel:BindableBase
    {
        private GithubReleaseAssetModel _model = null;

        public GithubReleaseAssetViewModel(GithubReleaseAssetModel model)
        {
            _model = model;
        }

        public string Name => _model.name;
        public int DownloadCount => _model.download_count;
        public string Size => $"{_model.size * 1.0 / 1024 / 1024:0.0#} MB";
        public string DownloadUrl => _model.browser_download_url;
    }
}
