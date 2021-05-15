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
        Task<GithubReleaseModel[]> GetReleasesAsync(string user= "huangjia2107", string repo= "rubik");
    }
}
