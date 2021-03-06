using MinatoProject.Apps.ToDoCoreWpf.Content.ViewModels;
using MinatoProject.Apps.ToDoCoreWpf.Content.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MinatoProject.Apps.ToDoCoreWpf.Content
{
    public class ContentModule : IModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionMan = containerProvider.Resolve<IRegionManager>();
            _ = regionMan.RegisterViewWithRegion("ContentRegion", typeof(TasksPage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<TaskDialog, TaskDialogViewModel>();
            containerRegistry.RegisterDialog<ConfigureCategoryDialog, ConfigureCategoryDialogViewModel>();
            containerRegistry.RegisterDialog<ConfigureStatusDialog, ConfigureStatusDialogViewModel>();
            containerRegistry.RegisterDialog<ConfigureStyleDialog, ConfigureStyleDialogViewModel>();
            containerRegistry.RegisterDialog<ApplicationPreferenceDialog, ApplicationPreferenceDialogViewModel>();
        }
    }
}