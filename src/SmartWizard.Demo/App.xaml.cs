using Prism.Ioc;
using Prism.Modularity;
using SmartWizard.Demo.Views;
using SmartWizard;
using System.Windows;

namespace SmartWizard.Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 注册应用程序特定的类型
            // 注意：IWizardService 会在 SmartWizardModule 中注册
            
            // 显式注册 MainWindowViewModel 以确保依赖注入正常工作
            containerRegistry.Register<ViewModels.MainWindowViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<SmartWizardModule>();
            moduleCatalog.AddModule<DemoModule>();
        }
    }
}