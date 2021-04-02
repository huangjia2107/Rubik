﻿using System;
using System.ComponentModel;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Services.Dialogs;

using Rubik.Demo.Models;
using Rubik.Demo.Events;
using Rubik.Demo.Views;
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

        //Event
        private IDialogService _dialogService = null;
        private IEventAggregator _eventAggregator = null;

        //Command
        public DelegateCommand<CancelEventArgs> ClosingCommand { get; private set; }

        public MainWindowViewModel(AppData appData, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _appData = appData;

            _dialogService = dialogService;
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

        #endregion

        #region Init

        private void InitEvents()
        {
            _eventAggregator.GetEvent<NavigationContentEvent>().Subscribe(OnNavigationContent);
        }

        private void InitCommands()
        {
            ClosingCommand = new DelegateCommand<CancelEventArgs>(Closing);
        }

        #endregion

        #region Event

        private void OnNavigationContent(Type type)
        {
            if (type == typeof(HomeControl))
            {
                ContentTitle = "Home";
                return;
            }

            if (type == typeof(GithubControl))
            {
                ContentTitle = "Github";
                return;
            }

            if (type == typeof(InformationControl))
            {
                ContentTitle = "Information";
                return;
            }
        }

        #endregion

        #region Command

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
                FileHelper.SaveToJsonFile(ResourcesMap.LocationDic[Location.GlobalConfigFile], _appData.Config);
        }

        #endregion
    }
}
