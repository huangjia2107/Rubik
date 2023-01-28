using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using Rubik.Service.WebApi.Github;
using Rubik.Service.Models;
using Rubik.Service.Github.WebApi;
using Prism.Services.Dialogs;
using Rubik.Service.Extensions;
using Rubik.Service.Log;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubControlViewModel : BindableBase
    {
        private GlobalConfig _globalConfig = null;
        private IGithubApi _githubApi = null;
        private IDialogService _dialogService = null;

        public DelegateCommand OpenInBrowserCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }

        public DelegateCommand GitCommand { get; private set; }
        public DelegateCommand SvnCommand { get; private set; }
        public DelegateCommand LicenseCommand { get; private set; }
        public DelegateCommand FeedbackCommand { get; private set; }

        public GithubControlViewModel(GlobalConfig globalConfig, IGithubApi githubApi, IDialogService dialogService)
        {
            _globalConfig = globalConfig;
            _githubApi = githubApi;
            _dialogService = dialogService;

            InitCommands();
            LoadGithubInfos();
        }

        #region Property

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

        #endregion

        #region Init

        private void InitCommands()
        {
            OpenInBrowserCommand = new DelegateCommand(OpenInBrowser);
            RefreshCommand = new DelegateCommand(Refresh);

            GitCommand = new DelegateCommand(Git);
            SvnCommand = new DelegateCommand(Svn);
            LicenseCommand = new DelegateCommand(License);
            FeedbackCommand = new DelegateCommand(Feedback);
        }

        #endregion

        #region Command

        private void OpenInBrowser()
        {
            if (RepositoryModel == null)
                return;

            Process.Start(new ProcessStartInfo(RepositoryModel.Url) { UseShellExecute = true });
        }

        private async void Refresh()
        {
            await LoadGithubInfos(true);
        }

        private void Git()
        {
            if (RepositoryModel == null)
                return;

            Clipboard.SetText(RepositoryModel.CloneUrl);
        }

        private void Svn()
        {
            if (RepositoryModel == null)
                return;

            Clipboard.SetText(RepositoryModel.SvnUrl);
        }

        private void License()
        {
            if (RepositoryModel == null)
                return;

            Process.Start(new ProcessStartInfo($"https://github.com/{_globalConfig.Github.User}/{_globalConfig.Github.Repository}/blob/{RepositoryModel.DefaultBranch}/LICENSE") { UseShellExecute = true });
        }

        private void Feedback()
        {
            Process.Start(new ProcessStartInfo($"https://github.com/{_globalConfig.Github.User}/{_globalConfig.Github.Repository}/issues") { UseShellExecute = true });
        }

        #endregion

        #region Func

        private async Task LoadGithubInfos(bool showMsg = false)
        {
            //Releases
            var releasesResult = await WebApiUtil.Catch(() => _githubApi.GetReleasesAsync(_globalConfig.Github.User, _globalConfig.Github.Repository));
            if(!string.IsNullOrEmpty(releasesResult.Error))
            {
                if(showMsg)
                    _dialogService.ShowMessage(releasesResult.Error, MessageButton.OK, MessageType.Error);

                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get releases error: {releasesResult.Error}");
                return;
            }

            ReleaseViewModels.AddRange(releasesResult.Output.Select(m => new GithubReleaseViewModel(m)));

            //Repository
            var repositoryResult = await WebApiUtil.Catch(() => _githubApi.GetRepositoryAsync(_globalConfig.Github.User, _globalConfig.Github.Repository));
            if (!string.IsNullOrEmpty(repositoryResult.Error))
            {
                if (showMsg)
                    _dialogService.ShowMessage(repositoryResult.Error, MessageButton.OK, MessageType.Error);

                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get repository error: {repositoryResult.Error}");
                return;
            }

            if (repositoryResult.Output == null)
                return;

            RepositoryModel = new GithubRepositoryViewModel(repositoryResult.Output);

            //Contributors
            var contributorsResult = await WebApiUtil.Catch(() => _githubApi.GetContributorsAsync(RepositoryModel.ContributorsUrl));
            if (!string.IsNullOrEmpty(contributorsResult.Error))
            {
                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get contributors error: {contributorsResult.Error}");
                return;
            }

            Contributors = contributorsResult.Output.Select(u => new GithubUserViewModel(u)).ToArray();

            //Stargazers
            var stargazersResult = await WebApiUtil.Catch(() => _githubApi.GetStargazersAsync(RepositoryModel.StargazersUrl));
            if (!string.IsNullOrEmpty(stargazersResult.Error))
            {
                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get stargazers error: {stargazersResult.Error}");
                return;
            }

            Stargazers = stargazersResult.Output.Take(20).Select(u => new GithubUserViewModel(u)).ToArray();

            //Subscribers
            var subscribersResult = await WebApiUtil.Catch(() => _githubApi.GetSubscribersAsync(RepositoryModel.SubscribersUrl));
            if (!string.IsNullOrEmpty(subscribersResult.Error))
            {
                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get subscribers error: {subscribersResult.Error}");
                return;
            }
             
            Subscribers = subscribersResult.Output.Take(20).Select(u => new GithubUserViewModel(u)).ToArray();

            //Forkers
            var forkersResult = await WebApiUtil.Catch(() => _githubApi.GetForkersAsync(RepositoryModel.ForksUrl));
            if (!string.IsNullOrEmpty(forkersResult.Error))
            {
                Logger.Instance.Module.Error($"[ Github ] LoadGithubInfos, Get forkers error: {forkersResult.Error}");
                return;
            }

            Forkers = forkersResult.Output.Take(20).Select(f => new GithubUserViewModel(f.owner)).ToArray();
        }

        #endregion
    }
}
