using System;
using System.Collections.Generic;
using System.Text;

namespace Rubik.Service.WebApi.Github
{
    public class GithubForkModel
    {
        public string full_name { get; set; }
        public GithubUserModel owner { get; set; }
    }
}
