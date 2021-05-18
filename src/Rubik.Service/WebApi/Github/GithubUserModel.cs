using System;
using System.Collections.Generic;
using System.Text;

namespace Rubik.Service.WebApi.Github
{
    public class GithubUserModel
    {
        public string login { get; set; }
        public string avatar_url { get; set; }
        public string html_url { get; set; }
    }
}
