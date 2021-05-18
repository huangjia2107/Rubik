using Prism.Mvvm;
using Rubik.Service.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubUserViewModel : BindableBase
    {
        private GithubUserModel _model = null;

        public GithubUserViewModel(GithubUserModel model)
        {
            _model = model;
        }

        public string Login => _model.login;
        public string AvatarUrl => _model.avatar_url;
        public string Url => _model.html_url;
    }
}
