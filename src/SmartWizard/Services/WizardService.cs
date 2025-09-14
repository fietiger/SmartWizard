using Prism.Services.Dialogs;
using Prism.Events;
using SmartWizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartWizard.Services
{
    /// <summary>
    /// 向导服务实现
    /// </summary>
    public class WizardService : IWizardService
    {
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly Dictionary<string, string> _wizardRegistry;

        public WizardService(IDialogService dialogService, IEventAggregator eventAggregator = null)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _eventAggregator = eventAggregator;
            _wizardRegistry = new Dictionary<string, string>();
        }

        public void ShowWizard(string wizardName, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            if (string.IsNullOrEmpty(wizardName))
                throw new ArgumentException("Wizard name cannot be null or empty", nameof(wizardName));

            if (!_wizardRegistry.TryGetValue(wizardName, out string viewName))
            {
                throw new InvalidOperationException($"Wizard '{wizardName}' is not registered");
            }

            _dialogService.ShowDialog(viewName, parameters, callback);
        }

        public void RegisterWizard(string wizardName, string viewName)
        {
            if (string.IsNullOrEmpty(wizardName))
                throw new ArgumentException("Wizard name cannot be null or empty", nameof(wizardName));

            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("View name cannot be null or empty", nameof(viewName));

            _wizardRegistry[wizardName] = viewName;
        }

        public void ShowWizard<TResult>(
            string title,
            IEnumerable<WizardStepViewModel> steps,
            Action<IDialogResult> callback,
            Func<IEnumerable<WizardStepViewModel>, TResult> dataCollector = null,
            WizardOptions options = null)
        {
            var genericDataCollector = dataCollector != null 
                ? (stepList) => dataCollector(stepList)
                : (Func<IEnumerable<WizardStepViewModel>, object>)null;
                
            ShowWizardInternal(title, steps, callback, genericDataCollector, options);
        }

        public void ShowWizard(
            string title,
            IEnumerable<WizardStepViewModel> steps,
            Action<IDialogResult> callback,
            WizardOptions options = null)
        {
            ShowWizardInternal(title, steps, callback, null, options);
        }

        private void ShowWizardInternal(
            string title,
            IEnumerable<WizardStepViewModel> steps,
            Action<IDialogResult> callback,
            Func<IEnumerable<WizardStepViewModel>, object> dataCollector,
            WizardOptions options)
        {
            // 参数验证
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null, empty, or whitespace", nameof(title));

            if (steps == null)
                throw new ArgumentNullException(nameof(steps));

            var stepsList = steps.ToList();
            if (!stepsList.Any())
                throw new ArgumentException("Steps collection cannot be empty", nameof(steps));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            // 设置默认选项
            options = options ?? new WizardOptions();
            options.Title = title;

            // 创建对话框参数，传递步骤和配置给GenericWizardDialogViewModel
            var parameters = new DialogParameters
            {
                { "Steps", stepsList },
                { "Title", title },
                { "Options", options },
                { "DataCollector", dataCollector }
            };

            // 使用通用的WizardDialogView，它现在使用GenericWizardDialogViewModel
            _dialogService.ShowDialog("WizardDialogView", parameters, callback);
        }
    }
}