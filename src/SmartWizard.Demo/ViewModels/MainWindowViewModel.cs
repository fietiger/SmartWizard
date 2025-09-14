using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SmartWizard.Services;
using SmartWizard.Demo.ViewModels;
using SmartWizard.ViewModels;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartWizard.Demo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly Lazy<IWizardService> _lazyWizardService;
        private string _result;

        protected IWizardService _wizardService => _lazyWizardService.Value;

        public MainWindowViewModel(IContainerProvider containerProvider, Lazy<IWizardService> lazyWizardService)
        {
            _containerProvider = containerProvider;
            _lazyWizardService = lazyWizardService;
            ShowConfigWizardCommand = new DelegateCommand(ExecuteShowConfigWizard);
            ShowConfigWizardWithServiceCommand = new DelegateCommand(ExecuteShowConfigWizardWithService);
            ShowSimplifiedWizardCommand = new DelegateCommand(ExecuteShowSimplifiedWizard);
            ShowSimplifiedWizardWithDataCollectorCommand = new DelegateCommand(ExecuteShowSimplifiedWizardWithDataCollector);
        }

        public string Title => "SmartWizard 演示程序";

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public DelegateCommand ShowConfigWizardCommand { get; }
        public DelegateCommand ShowConfigWizardWithServiceCommand { get; }
        public DelegateCommand ShowSimplifiedWizardCommand { get; }
        public DelegateCommand ShowSimplifiedWizardWithDataCollectorCommand { get; }

        private void ExecuteShowConfigWizard()
        {
            try
            {
                // 直接使用 IDialogService 进行测试
                var dialogService = _containerProvider.Resolve<IDialogService>();
                var parameters = new DialogParameters();
                
                dialogService.ShowDialog("ConfigWizardDialogView", parameters, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var configResult = result.Parameters.GetValue<ConfigResult>("WizardResult");
                        if (configResult != null)
                        {
                            Result = $"配置完成！（IDialogService方式）\n" +
                                    $"姓名: {configResult.Name}\n" +
                                    $"邮箱: {configResult.Email}\n" +
                                    $"语言: {configResult.Language}\n" +
                                    $"主题: {configResult.Theme}\n" +
                                    $"通知: {(configResult.EnableNotifications ? "启用" : "禁用")}";
                        }
                        else
                        {
                            Result = "配置完成，但没有返回数据（IDialogService方式）";
                        }
                    }
                    else
                    {
                        Result = "用户取消了配置（IDialogService方式）";
                    }
                });
            }
            catch (System.Exception ex)
            {
                Result = $"错误: {ex.Message}（IDialogService方式）\n堆栈: {ex.StackTrace}";
            }
        }

        private void ExecuteShowConfigWizardWithService()
        {
            try
            {
                var parameters = new DialogParameters();
                
                _wizardService.ShowWizard("ConfigWizard", parameters, result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        var configResult = result.Parameters.GetValue<ConfigResult>("WizardResult");
                        if (configResult != null)
                        {
                            Result = $"配置完成！（IWizardService方式）\n" +
                                    $"姓名: {configResult.Name}\n" +
                                    $"邮箱: {configResult.Email}\n" +
                                    $"语言: {configResult.Language}\n" +
                                    $"主题: {configResult.Theme}\n" +
                                    $"通知: {(configResult.EnableNotifications ? "启用" : "禁用")}";
                        }
                        else
                        {
                            Result = "配置完成，但没有返回数据（IWizardService方式）";
                        }
                    }
                    else
                    {
                        Result = "用户取消了配置（IWizardService方式）";
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                // 处理向导未注册异常
                Result = $"错误: 向导未注册 - {ex.Message}（IWizardService方式）\n堆栈: {ex.StackTrace}";
            }
            catch (ArgumentException ex)
            {
                // 处理参数异常（如向导名称为空等）
                Result = $"错误: 参数错误 - {ex.Message}（IWizardService方式）\n堆栈: {ex.StackTrace}";
            }

            catch (System.InvalidCastException ex)
            {
                // 处理类型转换异常（可能在服务解析时发生）
                Result = $"错误: 类型转换失败 - {ex.Message}（IWizardService方式）\n堆栈: {ex.StackTrace}";
            }
            catch (System.NullReferenceException ex)
            {
                // 处理空引用异常（可能在服务未正确注册时发生）
                Result = $"错误: 服务未正确初始化 - {ex.Message}（IWizardService方式）\n堆栈: {ex.StackTrace}";
            }
            catch (System.Exception ex)
            {
                // 处理其他通用异常
                Result = $"错误: {ex.Message}（IWizardService方式）\n堆栈: {ex.StackTrace}";
            }
        }

        private void ExecuteShowSimplifiedWizard()
        {
            try
            {
                // 创建步骤ViewModels
                var steps = new List<WizardStepViewModel>
                {
                    new Step1ViewModel(),
                    new Step2ViewModel()
                };

                // 使用简化API - 基本用法
                _wizardService.ShowWizard(
                    "配置向导（简化API）",
                    steps,
                    result =>
                    {
                        if (result.Result == ButtonResult.OK)
                        {
                            var wizardResult = result.Parameters.GetValue<object>("WizardResult");
                            if (wizardResult != null)
                            {
                                // 简化API默认返回步骤列表，我们需要手动提取数据
                                var step1 = steps[0] as Step1ViewModel;
                                var step2 = steps[1] as Step2ViewModel;
                                
                                Result = $"配置完成！（简化API - 基本用法）\n" +
                                        $"姓名: {step1?.Name}\n" +
                                        $"邮箱: {step1?.Email}\n" +
                                        $"语言: {step2?.SelectedLanguage}\n" +
                                        $"主题: {step2?.Theme}\n" +
                                        $"通知: {(step2?.EnableNotifications == true ? "启用" : "禁用")}\n" +
                                        $"返回数据类型: {wizardResult.GetType().Name}";
                            }
                            else
                            {
                                Result = "配置完成，但没有返回数据（简化API - 基本用法）";
                            }
                        }
                        else
                        {
                            Result = "用户取消了配置（简化API - 基本用法）";
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                Result = $"错误: {ex.Message}（简化API - 基本用法）\n堆栈: {ex.StackTrace}";
            }
        }

        private void ExecuteShowSimplifiedWizardWithDataCollector()
        {
            try
            {
                // 创建步骤ViewModels
                var steps = new List<WizardStepViewModel>
                {
                    new Step1ViewModel(),
                    new Step2ViewModel()
                };

                // 使用简化API - 带自定义数据收集器
                _wizardService.ShowWizard<ConfigResult>(
                    "配置向导（简化API + 数据收集器）",
                    steps,
                    result =>
                    {
                        if (result.Result == ButtonResult.OK)
                        {
                            var configResult = result.Parameters.GetValue<ConfigResult>("WizardResult");
                            if (configResult != null)
                            {
                                Result = $"配置完成！（简化API + 数据收集器）\n" +
                                        $"姓名: {configResult.Name}\n" +
                                        $"邮箱: {configResult.Email}\n" +
                                        $"语言: {configResult.Language}\n" +
                                        $"主题: {configResult.Theme}\n" +
                                        $"通知: {(configResult.EnableNotifications ? "启用" : "禁用")}\n" +
                                        $"返回数据类型: {configResult.GetType().Name}";
                            }
                            else
                            {
                                Result = "配置完成，但没有返回数据（简化API + 数据收集器）";
                            }
                        }
                        else
                        {
                            Result = "用户取消了配置（简化API + 数据收集器）";
                        }
                    },
                    // 自定义数据收集器 - 将步骤数据转换为ConfigResult
                    stepList =>
                    {
                        var step1 = stepList.FirstOrDefault() as Step1ViewModel;
                        var step2 = stepList.Skip(1).FirstOrDefault() as Step2ViewModel;

                        return new ConfigResult
                        {
                            Name = step1?.Name,
                            Email = step1?.Email,
                            Language = step2?.SelectedLanguage,
                            Theme = step2?.Theme,
                            EnableNotifications = step2?.EnableNotifications ?? false
                        };
                    }
                );
            }
            catch (Exception ex)
            {
                Result = $"错误: {ex.Message}（简化API + 数据收集器）\n堆栈: {ex.StackTrace}";
            }
        }
    }
}