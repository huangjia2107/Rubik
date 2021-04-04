using System;
using System.Linq;
using System.Windows;
using System.ComponentModel;

using Prism.Regions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Services.Dialogs;

using Rubik.Demo.Models;
using Rubik.Demo.Events;
using Rubik.Demo.Regions;
using Rubik.Demo.Services;
using Rubik.Service.Models;
using Rubik.Service.IO;

namespace Rubik.Demo.ViewModels
{
    /// <summary>
    /// 主窗体 ViewModel
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        //数据
        private AppData _appData = null;

        //Ioc
        private IDialogService _dialogService = null;
        private IRegionManager _regionManager = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand<CancelEventArgs> ClosingCommand { get; private set; }

        public MainWindowViewModel(AppData appData, IDialogService dialogService, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _appData = appData;

            _dialogService = dialogService;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            InitEvents();
            InitCommands();
        }

        #region Property

        /// <summary>
        /// 标题
        /// </summary>
        private string _title = "Rubik Demo";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version => _appData.Version;

        private string _contentTitle = "Home";
        public string ContentTitle
        {
            get { return _contentTitle; }
            set { SetProperty(ref _contentTitle, value); }
        }

        private Visibility _backVisibility = Visibility.Collapsed;
        public Visibility BackVisibility
        {
            get { return _backVisibility; }
            set { SetProperty(ref _backVisibility, value); }
        }

        #endregion

        #region Init

        private void InitEvents()
        {
            _eventAggregator.GetEvent<NavigationContentEvent>().Subscribe(OnNavigationContent);
        }

        private void InitCommands()
        {
            BackCommand = new DelegateCommand(Back);
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
        }

        #endregion

        #region Event

        private void OnNavigationContent(NavigationContentPayload payload)
        {
            ContentTitle = payload.ViewName;
            BackVisibility = payload.IsChild ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Command

        private void Back()
        {
            if (!_regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = _regionManager.Regions[RegionNames.Content];
            Type parentViewType = null;

            var childView = region.Views.FirstOrDefault(v =>
            {
                var childViewModel = (v as FrameworkElement).DataContext as IChildViewModel;
                if (childViewModel != null && childViewModel.IsOpened)
                {
                    childViewModel.IsOpened = false;
                    parentViewType = childViewModel.ParentViewType;
                    return true;
                }

                return false;
            });

            if (childView == null || parentViewType == null)
                return;

            var parentView = region.Views.FirstOrDefault(v => v.GetType() == parentViewType);
            if (parentView != null)
            {
                region.Activate(parentView);
                OnNavigationContent(new NavigationContentPayload { ViewType = parentViewType, ViewName = Models.ResourcesMap.ViewTypeViewNameDic[parentViewType] });
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void Closing(CancelEventArgs e)
        {
            /*
            var confirm = false;
            _dialogService.ShowMessage("确认退出吗？", MessageButton.YesNo, MessageType.Question, r => confirm = r.Result == ButtonResult.Yes);

            if (!confirm)
            {
                e.Cancel = true;
                return;
            }
            */

            if (_appData.Config != null)
                FileHelper.SaveToJsonFile(Service.Models.ResourcesMap.LocationDic[Location.GlobalConfigFile], _appData.Config);
        }

        #endregion
    }
}
