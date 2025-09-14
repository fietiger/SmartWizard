using Prism.Ioc;
using Prism.Modularity;
using SmartWizard.Controls;
using SmartWizard.Services;
using SmartWizard.ViewModels;
using SmartWizard.Views;

namespace SmartWizard
{
    /// <summary>
    /// SmartWizard Prism模块
    /// </summary>
    public class SmartWizardModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // 初始化ViewLocatorService
            var viewLocatorService = containerProvider.Resolve<IViewLocatorService>();
            WizardStepContentPresenter.SetViewLocatorService(viewLocatorService);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册服务
            containerRegistry.RegisterSingleton<IWizardService, WizardService>();
            containerRegistry.RegisterSingleton<IViewLocatorService, ViewLocatorService>();

            // 注册基础组件
            containerRegistry.Register<WizardDialogViewModel>();
            
            // 注册通用向导组件
            containerRegistry.Register<GenericWizardDialogViewModel>();
            
            // 注册通用对话框视图
            containerRegistry.RegisterDialog<WizardDialogView, GenericWizardDialogViewModel>("WizardDialogView");
        }
    }
}