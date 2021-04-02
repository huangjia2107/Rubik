using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;

using Rubik.Demo.Utils;
using Rubik.Demo.Dialogs;
using Rubik.Demo.Models;
using Rubik.Demo.ViewModels;
using Rubik.Demo.Views;
using Rubik.Demo.Regions;
using Rubik.Service.IO;
using Rubik.Service.Models;
using Rubik.Service.Utils;
using Rubik.Util.Log;

namespace Rubik.Demo
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
        /// 启动 Shell
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
        }

        /// <summary>
        /// 注入
        /// </summary>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Config
            var localConfig = FileHelper.LoadFromJsonFile<GlobalConfig>(ResourcesMap.LocationDic[Location.GlobalConfigFile]);

            //Version
            var version = ResourceAssembly.GetName().Version;
            var appData = new AppData
            {
                Version = $"{version.Major}.{version.Minor}.{version.Build}",
                Config = localConfig ?? new GlobalConfig()
            };

            Logger.Instance.Main.Info($"[ Version ] v{version.Major}.{version.Minor}.{version.Build}.{version.Revision}");
            
            //Demos
            appData.InternalData.DemoModels = new DemoResolver(ResourcesMap.LocationDic[Location.DemoPath]).LoadDemoModels();

            //Settings
            containerRegistry.RegisterInstance(appData);

            //Dialog
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<MessageDialog, MessageDialogViewModel>();
        }

        /*
        /// <summary>
        /// ServiceCollection
        /// </summary>
        private void RegisterTypesByServiceCollection(IContainerRegistry containerRegistry)
        {
            var services = new ServiceCollection();
            var action = (Action<HttpApiOptions>)(options => options.JsonDeserializeOptions.Converters.Add(new JsonNumberConverter()));

            //Web Api
            services.AddHttpApi<IServerApi>(action);

            var sp = services.BuildServiceProvider();

            var container = containerRegistry.GetContainer();
            container.Register<IServiceScopeFactory, DryIocServiceScopeFactory>(Reuse.Singleton);
            container.Populate(services);
        }
        */

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
                && Directory.Exists(ResourcesMap.LocationDic[Location.ProcdumpPath])
                && File.Exists(ResourcesMap.LocationDic[Location.ProcdumpBatFile]))
            {
                using (var p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo
                    {
                        FileName = ResourcesMap.LocationDic[Location.ProcdumpBatFile],
                        CreateNoWindow = true,
                        WorkingDirectory = ResourcesMap.LocationDic[Location.ProcdumpPath]
                    };

                    p.Start();
                }
            }

            //Region
            regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(SidebarControl));
            regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(HomeControl));
            regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(GithubControl));
            regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(InformationControl));

            if (!regionManager.Regions.ContainsRegionWithName(RegionNames.Content))
                return;

            var region = regionManager.Regions[RegionNames.Content];
            var homeView = region.Views.FirstOrDefault(v => v.GetType() == typeof(HomeControl));

            if (homeView != null)
                region.Activate(homeView);
        }
    }
}
