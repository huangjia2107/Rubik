using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Prism.Modularity;

using Rubik.App.Models;
using Rubik.App.Views;
using Rubik.Service.IO;
using Rubik.Service.Models;
using Rubik.Service.Regions;
using Rubik.Service.Utils;
using Rubik.Service.Dialogs;
using Rubik.Service.Log;
using Rubik.Service.ViewModels;
using Rubik.Service.WebApi.Github;

namespace Rubik.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
            : base()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        #region Exception

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var msg = "[ UnhandledException ] UI Dispatcher, " + CommonUtil.GetExceptionStringFormat(e.Exception);
            var time = " [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "]";

            Logger.Instance.Main.Error(msg);
            MessageBox.Show(msg, "Exception" + time, MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var msg = "[ UnhandledException ] Task, " + e.Exception.Message;
            var time = " [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "]";

            Logger.Instance.Main.Error(msg);
            //MessageBox.Show(msg, "Exception" + time, MessageBoxButton.OK, MessageBoxImage.Error);

            e.SetObserved();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var msg = "[ UnhandledException ] Current Domain, " + CommonUtil.GetExceptionStringFormat(e.ExceptionObject as Exception);
            var time = " [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff") + "]";

            Logger.Instance.Main.Error(msg);
            MessageBox.Show(msg, "Exception" + time, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        /// <summary>
        /// View-ViewModel
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        /// <summary>
        /// Shell
        /// </summary>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// Region Adapter Mappings
        /// </summary>
        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping<Grid, GridRegionAdapter>();
            regionAdapterMappings.RegisterMapping<StackPanel, StackPanelRegionAdapter>();
        }

        /// <summary>
        /// 注入
        /// </summary>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Config
            var localConfig = FileHelper.LoadFromJsonFile<GlobalConfig>(Service.Models.ResourcesMap.LocationDic[Location.GlobalConfigFile]);

            //Version
            var version = ResourceAssembly.GetName().Version;
            var appData = new AppData
            {
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Config = localConfig ?? new GlobalConfig()
            };

            Logger.Instance.Main.Info($"[ Version ] v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
            
            //Settings
            containerRegistry.RegisterInstance(appData);

            //Dialog
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();

            RegisterTypesByServiceCollection(containerRegistry);
        }

        /// <summary>
        /// ServiceCollection
        /// </summary>
        private void RegisterTypesByServiceCollection(IContainerRegistry containerRegistry)
        {
            var services = new ServiceCollection();

            //WebApi
            services.AddHttpApi<IGithubApi>();

            var sp = services.BuildServiceProvider();

            var container = containerRegistry.GetContainer();
            container.Register<IServiceScopeFactory, DryIocServiceScopeFactory>(Reuse.Singleton);
            container.Populate(services);
        }

        /// <summary>
        /// Module Catalog
        /// </summary>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog() { ModulePath = Service.Models.ResourcesMap.LocationDic[Location.ModulePath] };
        }

        /// <summary>
        /// 初始化完成
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var appData = Container.Resolve<AppData>();
            var regionManager = Container.Resolve<IRegionManager>();

            //Procdump
            if (appData.Config.Procdump
                && Directory.Exists(Service.Models.ResourcesMap.LocationDic[Location.ProcdumpPath])
                && File.Exists(Service.Models.ResourcesMap.LocationDic[Location.ProcdumpBatFile]))
            {
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        FileName = Service.Models.ResourcesMap.LocationDic[Location.ProcdumpBatFile],
                        CreateNoWindow = true,
                        WorkingDirectory = Service.Models.ResourcesMap.LocationDic[Location.ProcdumpPath]
                    };

                    p.Start();
                }
            }

            if (!regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = regionManager.Regions[RegionNames.Content];
            
            var defaultActivateView = region.Views.FirstOrDefault(v =>
            {
                return (Attribute.GetCustomAttribute(v.GetType(), typeof(ViewSortHintAttribute)) as ViewSortHintAttribute) != null;
            });

            if (defaultActivateView != null)
                region.Activate(defaultActivateView);
        }
    }
}
