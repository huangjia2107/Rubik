using Rubik.Service.WebApi.Github;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rubik.Module.Github.ViewModels
{
    public class GithubLicenseViewModel
    {
        private GithubLicenseModel _model = null;

        public GithubLicenseViewModel(GithubLicenseModel model)
        {
            _model = model;
        }

        public string Name =>_model.name;
        public string SpdxID => _model.spdx_id;
    }
}
