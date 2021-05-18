using System;
using System.Collections.Generic;
using System.Text;

namespace Rubik.Service.WebApi.Github
{
    public class GithubRepositoryModel
    {
        public string name { get; set; }
        public string html_url { get; set; }

        public string git_url { get; set; }
        public string ssh_url { get; set; }
        public string clone_url { get; set; }
        public string svn_url { get; set; }

        public string contributors_url { get; set; }

        public string stargazers_url { get; set; }
        public string subscribers_url { get; set; }
        public string forks_url { get; set; }

        public int stargazers_count { get; set; }
        public int subscribers_count { get; set; }
        public int forks_count { get; set; }

        public int open_issues_count { get; set; }

        public string updated_at { get; set; }

        public GithubLicenseModel license { get; set; }
    }

    public class GithubLicenseModel
    {
        public string name { get; set; }
        public string spdx_id { get; set; }
    }
}
