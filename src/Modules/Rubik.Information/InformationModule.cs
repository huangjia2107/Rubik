using System;
using System.Collections.Generic;
using System.Text;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Rubik.Information
{
    public class InformationModule : IModule
    {
        private IRegionManager _regionManager = null;

        #region IModule Members

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();
            //_regionManager.RegisterViewWithRegion(RegionNames.DesignerName, typeof(DesignerControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        #endregion
    }
}
