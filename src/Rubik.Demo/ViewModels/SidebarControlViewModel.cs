using System;
using System.Linq;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.Demo.Events;
using Rubik.Demo.Regions;
using Rubik.Demo.Views;

namespace Rubik.Demo.ViewModels
{
    public class SidebarControlViewModel : BindableBase
    {
        //Ioc
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand HomeCommand { get; protected set; }
        public DelegateCommand GithubCommand { get; protected set; }
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
            HomeCommand = new DelegateCommand(Home);
            GithubCommand = new DelegateCommand(Github);
            InformationCommand = new DelegateCommand(Information);
        }

        #endregion

        #region Command

        private void Home()
        {
            OpenView(typeof(HomeControl));
        }

        private void Github()
        {
            OpenView(typeof(GithubControl));
        }

        private void Information()
        {
            OpenView(typeof(InformationControl));
        }

        #endregion

        #region Func

        /// <summary>
        /// 打开页签
        /// </summary>
        private void OpenView(Type viewType)
        {
            if (!_regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = _regionManager.Regions[RegionNames.Content];

            var view = region.Views.FirstOrDefault(v => v.GetType() == viewType);
            if (view != null)
            {
                region.Activate(view);
                _eventAggregator.GetEvent<NavigationContentEvent>().Publish(viewType);
            }
        }

        #endregion
    }
}
