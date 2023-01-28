
namespace Rubik.Service.WebApi.Github
{
    public class GithubReleaseModel
    {
        public string name { get; set; }
        public string published_at { get; set; }
        public string tag_name { get; set; }
        public bool prerelease { get; set; }
        public string body { get; set; }

        public GithubReleaseAssetModel[] assets { get; set; }
    }

    public class GithubReleaseAssetModel
    {
        public string name { get; set; }
        public int download_count { get; set; }
        public long size { get; set; }
        public string browser_download_url { get; set; }
    }
}
