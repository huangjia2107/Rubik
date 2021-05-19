using Rubik.Service.Models;

namespace Rubik.App.Models
{
    public class AppData
    {
        public string Version { get; set; }

        public InternalData InternalData { get; set; } = new InternalData();
        public GlobalConfig Config { get; set; } = new GlobalConfig();
    }
}
