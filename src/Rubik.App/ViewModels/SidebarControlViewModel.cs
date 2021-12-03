using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using Rubik.App.Models;
using Rubik.Toolkit.Utils;

namespace Rubik.App.ViewModels
{
    public class SidebarControlViewModel : BindableBase
    {
        //Ioc
        private AppData _appData = null;
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand LogoCommand { get; protected set; }

        public SidebarControlViewModel(AppData appData, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _appData = appData;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            InitCommands();
        }

        #region Property

        private Brush _logoBrush = ColorUtil.GetBrushFromString("#283142");
        public Brush LogoBrush
        {
            get { return _logoBrush; }
            set { SetProperty(ref _logoBrush, value); }
        }

        public string Version => _appData.Version;

        #endregion

        #region Init

        /// <summary>
        /// 初始化命令
        /// </summary>
        private void InitCommands()
        {
            LogoCommand = new DelegateCommand(Logo);
        }

        #endregion

        #region Command

        private void Logo()
        {
            LogoBrush = ColorUtil.GetRandomBrush();
        }

        #endregion
    }
}
