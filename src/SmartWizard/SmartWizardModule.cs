using Prism.Ioc;
using Prism.Modularity;
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
            // 模块初始化后的操作
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册服务
            containerRegistry.RegisterSingleton<IWizardService, WizardService>();

            // 注册基础组件
            containerRegistry.Register<WizardDialogViewModel>();
            
            // 注册通用向导组件
            containerRegistry.Register<GenericWizardDialogViewModel>();
            
            // 注册通用对话框视图
            containerRegistry.RegisterDialog<WizardDialogView, GenericWizardDialogViewModel>("WizardDialogView");
        }
    }
}