
namespace Rubik.Service.Models
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// 异常时生成 dump 文件
        /// </summary>
        public bool Procdump { get; set; } = false;

        /// <summary>
        /// Github
        /// </summary>
        public GithubConfig Github { get; set; } = new GithubConfig();
    }
}
