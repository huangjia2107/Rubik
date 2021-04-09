using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Rubik.Experiment.Views;
using Rubik.Service.Regions;

namespace Rubik.Experiment
{
    public class ExperimentModule : IModule
    {
        private IRegionManager _regionManager = null;

        #region IModule Members

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(SidebarControl));
            _regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(ExperimentControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        #endregion
    }
}
