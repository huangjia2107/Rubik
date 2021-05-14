using Microsoft.Extensions.DependencyInjection;

using Prism.Ioc;
using Prism.DryIoc;
using Prism.Modularity;
using Prism.Regions;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using Rubik.Module.Github.WebApi.Github;
using Rubik.Module.Github.Views;
using Rubik.Service.Regions;

namespace Rubik.Module.Github
{
    public class GithubModule : IModule
    {
        private IRegionManager _regionManager = null;

        #region IModule Members

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterTypesByServiceCollection(containerRegistry);
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();

            _regionManager.RegisterViewWithRegion(RegionNames.Sidebar, typeof(SidebarControl));
            _regionManager.RegisterViewWithRegion(RegionNames.Content, typeof(GithubControl));
        }

        #endregion

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
    }
}
