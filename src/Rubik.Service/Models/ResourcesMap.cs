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

        public static Dictionary<DemoType, string> DemoTypeNameDic = new Dictionary<DemoType, string>
        {
            [DemoType.Button] = "Button",
            [DemoType.Collection] = "Collection",
            [DemoType.Chart] = "Chart",
            [DemoType.Date] = "Date",
            [DemoType.Layout] = "Layout",
            [DemoType.Media] = "Media",
            [DemoType.Motion] = "Motion",
            [DemoType.Slider] = "Slider",
            [DemoType.Text] = "Text",
            [DemoType.Misc] = "Misc",
        };

        public static Dictionary<DemoType, string> DemoTypeIconDataDic = new Dictionary<DemoType, string>
        {
            [DemoType.Button] = "M0,0 H10 V10 H0 z",
            [DemoType.Collection] = "M0,0 H10 V10 H0 z",
            [DemoType.Chart] = "M0,0 H10 V10 H0 z",
            [DemoType.Date] = "M0,0 H10 V10 H0 z",
            [DemoType.Layout] = "M0,0 H10 V10 H0 z",
            [DemoType.Media] = "M0,0 H10 V10 H0 z",
            [DemoType.Motion] = "M0,0 H10 V10 H0 z",
            [DemoType.Slider] = "M0,0 H10 V10 H0 z",
            [DemoType.Text] = "M0,0 H10 V10 H0 z",
            [DemoType.Misc] = "M0,0 H10 V10 H0 z",
        };
    }
}
