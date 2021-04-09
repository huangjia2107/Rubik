﻿using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Rubik.Module.Home.Models;
using Rubik.Module.Home.Utils;
using Rubik.Module.Home.Views;
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
            var homeModel = new HomeModel();

            //Demos
            homeModel.DemoModels = new DemoResolver(ResourcesMap.LocationDic[Location.DemoPath]).LoadDemoModels();

            containerRegistry.RegisterInstance(homeModel);
        }

        #endregion
    }
}