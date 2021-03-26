using System;
using System.Collections.Generic;
using System.Text;

namespace Rubik.Service.Models
{
    /// <summary>
    /// 键值映射
    /// </summary>
    public static class ResourcesMap
    {
        /// <summary>
        /// 各个路径
        /// </summary>
        public static Dictionary<Location, string> LocationDic = new Dictionary<Location, string>
        {
            [Location.GlobalConfigFile] = AppDomain.CurrentDomain.BaseDirectory + "Config\\GlobalConfig.json",
            [Location.DemoPath] = AppDomain.CurrentDomain.BaseDirectory + "Demos",
            [Location.ProcdumpPath] = AppDomain.CurrentDomain.BaseDirectory + "Procdump\\",
            [Location.ProcdumpBatFile] = AppDomain.CurrentDomain.BaseDirectory + "Procdump\\monitor.bat",
        };
    }
}
