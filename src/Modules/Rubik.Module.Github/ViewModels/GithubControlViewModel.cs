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

        private GithubRepositoryViewModel _repositoryModel;
        public GithubRepositoryViewModel RepositoryModel
        {
            get { return _repositoryModel; }
            set { SetProperty(ref _repositoryModel, value); }
        }

        private GithubUserViewModel[] _contributors;
        public GithubUserViewModel[] Contributors
        {
            get { return _contributors; }
            set { SetProperty(ref _contributors, value); }
        }

        private GithubUserViewModel[] _stargazers;
        public GithubUserViewModel[] Stargazers
        {
            get { return _stargazers; }
            set { SetProperty(ref _stargazers, value); }
        }

        private GithubUserViewModel[] _subscribers;
        public GithubUserViewModel[] Subscribers
        {
            get { return _subscribers; }
            set { SetProperty(ref _subscribers, value); }
        }

        private GithubUserViewModel[] _forkers;
        public GithubUserViewModel[] Forkers
        {
            get { return _forkers; }
            set { SetProperty(ref _forkers, value); }
        }

        private async Task fff()
        {
            RepositoryModel = new GithubRepositoryViewModel(await _githubApi.GetRepositoryAsync(repo: "xamlviewer"));

            var contributors = await _githubApi.GetContributorsAsync(RepositoryModel.ContributorsUrl);
            Contributors = contributors.Select(u => new GithubUserViewModel(u)).ToArray();

            var stargazers = await _githubApi.GetStargazersAsync(RepositoryModel.StargazersUrl);
            Stargazers = stargazers.Take(20).Select(u => new GithubUserViewModel(u)).ToArray();

            var subscribers = await _githubApi.GetSubscribersAsync(RepositoryModel.SubscribersUrl);
            Subscribers = subscribers.Take(20).Select(u => new GithubUserViewModel(u)).ToArray();

            var forkers = await _githubApi.GetForkersAsync(RepositoryModel.ForksUrl);
            Forkers = forkers.Take(20).Select(f => new GithubUserViewModel(f.owner)).ToArray();

            var models = await _githubApi.GetReleasesAsync(repo: "xamlviewer");
            ReleaseViewModels.AddRange(models.Select(m => new GithubReleaseViewModel(m)));


        }
    }
}
