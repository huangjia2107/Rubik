using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace Rubik.Module.Github.WebApi.Github
{
    [HttpHost("https://api.github.com/")]
    public interface IGithubApi
    {
        [HttpGet("repos/{user}/{repo}/releases")]
        Task<ReleaseModel[]> GetReleasesAsync(string user= "huangjia2107", string repo= "rubik");
    }
}
