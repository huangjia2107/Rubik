using System;
using System.Collections.Generic;
using System.IO;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Rubik.Module.Home.Models;
using Rubik.Module.Home.Utils;
using Rubik.Module.Home.Views;
using Rubik.Service;
using Rubik.Service.IO;
using Rubik.Service.Models;
using Rubik.Service.Regions;

namespace Rubik.Module.Home
{
    public class HomeModule : IModule
    {
        private IRegionManager _regionManager = null;

        #region IModule Members

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(SidebarControl));
            _regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(HomeControl));
            _regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(DemoControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Demos
            var demoModelsFromConfig = new List<IDemoModel>();

            var demoModelsFromLib = new DemoResolver(ResourcesMap.LocationDic[Location.DemoPath]).LoadDemoModels();
            demoModelsFromConfig.AddRange(demoModelsFromLib);

            foreach (var childDir in Directory.GetDirectories(ResourcesMap.LocationDic[Location.DemoPath]))
            {
                var dirName = Path.GetFileName(childDir);

                if (!Enum.IsDefined(typeof(DemoType), dirName))
                    continue;

                foreach(var xmlfile in Directory.GetFiles(childDir,"*.xml"))
                {
                    var model = FileUtil.LoadFromXmlFile<ConfigDemoModel>(xmlfile);
                    if (model == null)
                        continue;

                    model.Type = (DemoType)Enum.Parse(typeof(DemoType), dirName);
                    model.Name = Path.GetFileNameWithoutExtension(xmlfile);

                    demoModelsFromConfig.Add(model);
                }
            }

            var homeModel = new HomeModel
            {
                DemoModels = demoModelsFromConfig.ToArray()
            };

            containerRegistry.RegisterInstance(homeModel);
        }

        #endregion
    }
}
