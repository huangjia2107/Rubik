using System;
using System.Linq;
using System.Collections.Generic;

using Prism;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.Demo.Events;
using Rubik.Demo.Models;
using Rubik.Demo.Services;
using Rubik.Demo.Views;
using Rubik.Service.Models;

namespace Rubik.Demo.ViewModels
{
    public class DemoControlViewModel : BindableBase, IActiveAware, IChildViewModel
    {
        private AppData _appData = null;
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        public DemoControlViewModel(AppData appData, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _appData = appData;
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
            if (_appData.InternalData.DemoModels == null || _appData.InternalData.DemoModels.Length == 0)
                return;

            IsOpened = true;
            ViewName = Service.Models.ResourcesMap.DemoTypeNameDic[type];

            DemoList = _appData.InternalData.DemoModels.Where(d => d.Type == type).Select(d => new DemoViewModel(d));
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

        #endregion
    }
}
