using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Rubik.Module.UdpTester.Views;
using Rubik.Service.Regions;

namespace Rubik.Module.UdpTester
{
    public class UdpTesterModule : IModule
    {
        private IRegionManager _regionManager = null;

        #region IModule Members

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(SidebarControl));
            _regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(UdpTesterControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        #endregion
    }
}
