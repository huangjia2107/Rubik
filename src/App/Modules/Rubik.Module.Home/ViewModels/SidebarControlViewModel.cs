using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.Module.Home.Views;
using Rubik.Service.Events;
using Rubik.Service.Regions;
using Rubik.Service.ViewModels;

namespace Rubik.Module.Home.ViewModels
{
    public class SidebarControlViewModel : BindableBase
    {
        //Ioc
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand HomeCommand { get; protected set; }

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
            HomeCommand = new DelegateCommand(Home);
        }

        #endregion

        #region Command

        private void Home()
        {
            if (!_regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = _regionManager.Regions[RegionNames.Content];

            var viewType = typeof(HomeControl);
            var childViewType = typeof(DemoControl);

            var childView = region.Views.FirstOrDefault(v => v.GetType() == childViewType);
            if (childView != null)
            {
                var childViewModel = (childView as FrameworkElement).DataContext as IChildViewModel;
                if (childViewModel != null && childViewModel.IsOpened)
                {
                    region.Activate(childView);
                    _eventAggregator.GetEvent<NavigationContentEvent>().Publish(new NavigationContentPayload { IsChild = true, ViewType = childViewType, ViewName = childViewModel.ViewName });

                    return;
                }
            }

            var view = region.Views.FirstOrDefault(v => v.GetType() == viewType);
            if (view != null)
            {
                region.Activate(view);
                _eventAggregator.GetEvent<NavigationContentEvent>().Publish(new NavigationContentPayload { ViewType = viewType, ViewName = "Home" });
            }
        }

        #endregion
    }
}
