using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

using Prism.Regions;
using Prism.Events;
using Prism.Commands;
using Prism.Mvvm;

using Rubik.Module.Home.Models;
using Rubik.Module.Home.Views;
using Rubik.Module.Home.Events;
using Rubik.Service.Events;
using Rubik.Service.Regions;
using Rubik.Theme.UI;

namespace Rubik.Module.Home.ViewModels
{
    public class HomeControlViewModel : BindableBase
    {
        private HomeModel _homeModel = null;
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        public DelegateCommand<MouseButtonEventArgs> DoubleClickDemoCommand { get; protected set; }

        public HomeControlViewModel(HomeModel homeModel, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _homeModel = homeModel;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            InitCommands();
        }

        public List<DisplayDemoViewModel> AllDemos
        {
            get
            {
                if (_homeModel.DemoModels == null || _homeModel.DemoModels.Length == 0)
                    return null;

                return _homeModel.DemoModels.GroupBy(d => d.Type)
                                            .Select(g => new DisplayDemoViewModel
                                            {
                                                Type = g.Key,
                                                DemoList = g.Select(d => new DemoViewModel(d)).ToList()
                                            }).ToList();
            }
        }

        #region Init

        private void InitCommands()
        {
            DoubleClickDemoCommand = new DelegateCommand<MouseButtonEventArgs>(DoubleClickDemo);
        }

        #endregion

        #region Command

        private void DoubleClickDemo(MouseButtonEventArgs e)
        {
            var listBoxItem = TreeUtil.FindVisualParent<ListBoxItem>(e.OriginalSource as DependencyObject);
            if (listBoxItem == null)
                return;

            var model = listBoxItem.DataContext as DisplayDemoViewModel;
            if (model == null)
                return;

            if (!_regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = _regionManager.Regions[RegionNames.Content];
            var viewType = typeof(DemoControl);

            var demoView = region.Views.FirstOrDefault(v => v.GetType() == viewType);
            if (demoView != null)
            {
                _eventAggregator.GetEvent<OpenDemoTypeEvent>().Publish(model.Type);
                _eventAggregator.GetEvent<NavigationContentEvent>().Publish(new NavigationContentPayload { IsChild = true, ViewType = viewType, ViewName = Service.Models.ResourcesMap.DemoTypeNameDic[model.Type] });

                region.Activate(demoView);
            }
        }

        #endregion
    }
}