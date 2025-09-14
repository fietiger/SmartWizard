using Prism.Ioc;
using Prism.Modularity;
using SmartWizard.Demo.ViewModels;
using SmartWizard.Demo.Views;
using SmartWizard.Services;

namespace SmartWizard.Demo
{
    /// <summary>
    /// 演示模块
    /// </summary>
    public class DemoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // 注册向导
            var wizardService = containerProvider.Resolve<IWizardService>();
            wizardService.RegisterWizard("ConfigWizard", "ConfigWizardDialogView");
            
            // 注册Step ViewModels和Views的映射
            var viewLocatorService = containerProvider.Resolve<IViewLocatorService>();
            viewLocatorService.RegisterViewMapping<Step1ViewModel, Step1View>();
            viewLocatorService.RegisterViewMapping<Step2ViewModel, Step2View>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册对话框 - 使用专门的ConfigWizardDialogView
            containerRegistry.RegisterDialog<ConfigWizardDialogView, ConfigWizardViewModel>();
            
            // 注册ViewModels
            containerRegistry.Register<Step1ViewModel>();
            containerRegistry.Register<Step2ViewModel>();
        }
    }
}