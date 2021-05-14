using System;
using System.Collections.Generic;
using System.Text;

using Prism.Mvvm;
using Rubik.Module.Github.WebApi.Github;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubControlViewModel : BindableBase
    {
        private IGithubApi _githubApi = null;

        public GithubControlViewModel(IGithubApi githubApi)
        {
            _githubApi = githubApi;
        }
    }
}
