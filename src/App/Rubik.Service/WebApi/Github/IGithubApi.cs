using System.Threading.Tasks;
using WebApiClientCore.Attributes;
using Rubik.Service.WebApi.Attributes;

namespace Rubik.Service.WebApi.Github
{
    [Timeout(5000)]
    [RequestLogging]
    [HttpHost("https://api.github.com/")]
    public interface IGithubApi
    {
        [HttpGet("repos/{user}/{repo}/releases")]
        Task<GithubReleaseModel[]> GetReleasesAsync(string user = "huangjia2107", string repo = "rubik");

        [HttpGet("repos/{user}/{repo}")]
        Task<GithubRepositoryModel> GetRepositoryAsync(string user = "huangjia2107", string repo = "rubik");

        [HttpGet]
        Task<GithubUserModel[]> GetContributorsAsync([Uri] string uri);

        [HttpGet]
        Task<GithubUserModel[]> GetStargazersAsync([Uri] string uri);

        [HttpGet]
        Task<GithubUserModel[]> GetSubscribersAsync([Uri] string uri);

        [HttpGet]
        Task<GithubForkModel[]> GetForkersAsync([Uri] string uri);
    }
}
