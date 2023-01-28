using System;
using System.Linq;
using System.Collections.Generic;

using Prism;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.Service.Models;
using Rubik.Service.ViewModels;
using Rubik.Module.Home.Events;
using Rubik.Module.Home.Views;
using Rubik.Module.Home.Models;

namespace Rubik.Module.Home.ViewModels
{
    public class DemoControlViewModel : BindableBase, IActiveAware, IChildViewModel
    {
        private HomeModel _homeModel = null;
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        public DemoControlViewModel(HomeModel homeModel, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _homeModel = homeModel;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            InitEvents();
        }

        #region Property

        public IEnumerable<DemoViewModel> DemoList { get; private set; }

        #endregion

        #region Init

        private void InitEvents()
        {
            _eventAggregator.GetEvent<OpenDemoTypeEvent>().Subscribe(OnOpenDemoType);
        }

        #endregion

        #region Event

        private void OnOpenDemoType(DemoType type)
        {
            if (_homeModel.DemoModels == null || _homeModel.DemoModels.Length == 0)
                return;

            IsOpened = true;
            ViewName = ResourcesMap.DemoTypeNameDic[type];

            DemoList = _homeModel.DemoModels.Where(d => d.Type == type).Select(d => new DemoViewModel(d));
            RaisePropertyChanged(nameof(DemoList));
        }

        #endregion

        #region IActiveAware

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public event EventHandler IsActiveChanged;

        #endregion

        #region IChildViewModel

        public bool IsOpened { get; set; }

        public string ViewName { get; private set; }

        public Type ParentViewType => typeof(HomeControl);

        public string ParentViewName => "Home";

        #endregion
    }
}
