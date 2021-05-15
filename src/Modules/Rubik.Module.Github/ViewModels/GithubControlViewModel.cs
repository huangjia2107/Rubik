using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Prism.Mvvm;
using Rubik.Service.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubControlViewModel : BindableBase
    {
        private IGithubApi _githubApi = null;

        public GithubControlViewModel(IGithubApi githubApi)
        {
            _githubApi = githubApi;

            fff();
        }

        private ObservableCollection<GithubReleaseViewModel> _releaseViewModels = null;
        public ObservableCollection<GithubReleaseViewModel> ReleaseViewModels
        {
            get { return _releaseViewModels ??= new ObservableCollection<GithubReleaseViewModel>(); }
        }

        private async Task fff()
        {
            var models = await _githubApi.GetReleasesAsync(repo: "xamlviewer");
            ReleaseViewModels.AddRange(models.Select(m => new GithubReleaseViewModel(m)));
        }
    }
}
