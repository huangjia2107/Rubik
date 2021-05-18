using Prism.Mvvm;
using Rubik.Service.WebApi.Github;
using System;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubRepositoryViewModel : BindableBase
    {
        private GithubRepositoryModel _model = null;

        public GithubRepositoryViewModel(GithubRepositoryModel model)
        {
            _model = model;
        }

        public string Name => _model.name;
        public string Url => _model.html_url;

        public string GitUrl => _model.git_url;
        public string SshUrl => _model.ssh_url;
        public string CloneUrl => _model.clone_url;
        public string SvnUrl => _model.svn_url;

        public string ContributorsUrl => _model.contributors_url;

        public string StargazersUrl => _model.stargazers_url;
        public string SubscribersUrl => _model.subscribers_url;
        public string ForksUrl => _model.forks_url;

        public int StargazersCount => _model.stargazers_count;
        public int SubscribersCount => _model.subscribers_count;
        public int Forks_count => _model.forks_count;

        public int IssuesCount => _model.open_issues_count;

        public DateTime UpdateTime => DateTime.Parse(_model.updated_at).ToLocalTime();

        GithubLicenseViewModel _license = null;
        public GithubLicenseViewModel License => _license ??= new GithubLicenseViewModel(_model.license);
    }
}
