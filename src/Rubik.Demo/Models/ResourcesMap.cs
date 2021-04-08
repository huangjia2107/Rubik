using System;
using System.Collections.Generic;

using Rubik.Demo.Views;

namespace Rubik.Demo.Models
{
    /// <summary>
    /// 键值映射
    /// </summary>
    public static class ResourcesMap
    {
        public static Dictionary<Type, string> ViewTypeViewNameDic = new Dictionary<Type, string>
        {
            [typeof(HomeControl)] = "Home",
            [typeof(ExperimentControl)] = "Experiment",
            [typeof(GithubControl)] = "Github",
            [typeof(InformationControl)] = "Information",
        };
    }
}
