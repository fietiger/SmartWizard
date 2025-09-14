using Prism.Events;
using Prism.Services.Dialogs;
using SmartWizard.ViewModels;

namespace SmartWizard.Demo.ViewModels
{
    /// <summary>
    /// 配置向导的主ViewModel
    /// </summary>
    public class ConfigWizardViewModel : WizardDialogViewModel, IDialogAware
    {
        public ConfigWizardViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        protected override void InitializeSteps(IDialogParameters parameters)
        {
            // 添加步骤
            var step1 = new Step1ViewModel();
            var step2 = new Step2ViewModel();
            AddStep(step1);
            AddStep(step2);
            // 调试信息
            System.Diagnostics.Debug.WriteLine($"Steps initialized: {Steps.Count} steps");
            System.Diagnostics.Debug.WriteLine($"Step1 Title: {step1.Title}");
            System.Diagnostics.Debug.WriteLine($"Step2 Title: {step2.Title}");
        }

        protected override object CollectWizardData()
        {
            var step1 = Steps[0] as Step1ViewModel;
            var step2 = Steps[1] as Step2ViewModel;

            return new ConfigResult
            {
                Name = step1?.Name,
                Email = step1?.Email,
                Language = step2?.SelectedLanguage,
                Theme = step2?.Theme,
                EnableNotifications = step2?.EnableNotifications ?? false
            };
        }
    }

    /// <summary>
    /// 配置结果数据模型
    /// </summary>
    public class ConfigResult
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
        public bool EnableNotifications { get; set; }
    }
}