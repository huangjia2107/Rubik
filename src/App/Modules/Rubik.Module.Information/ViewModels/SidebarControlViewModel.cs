using System.Linq;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.Module.Information.Views;
using Rubik.Service.Events;
using Rubik.Service.Regions;

namespace Rubik.Module.Information.ViewModels
{
    public class SidebarControlViewModel : BindableBase
    {
        //Ioc
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand InformationCommand { get; protected set; }

        public SidebarControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            InitCommands();
        }

        #region Init

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void InitCommands()
        {
            InformationCommand = new DelegateCommand(Information);
        }

        #endregion

        #region Command

        private void Information()
        {
            if (!_regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = _regionManager.Regions[RegionNames.Content];
            var viewType = typeof(InformationControl);

            var view = region.Views.FirstOrDefault(v => v.GetType() == viewType);
            if (view != null)
            {
                region.Activate(view);
                _eventAggregator.GetEvent<NavigationContentEvent>().Publish(new NavigationContentPayload { ViewType = viewType, ViewName = "Information" });
            }
        }

        #endregion
    }
}
