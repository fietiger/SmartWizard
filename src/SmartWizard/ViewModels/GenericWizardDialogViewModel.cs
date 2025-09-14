using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Events;
using Prism.Services.Dialogs;

namespace SmartWizard.ViewModels
{
    /// <summary>
    /// 通用向导对话框ViewModel，支持直接传入步骤列表和配置选项
    /// </summary>
    public class GenericWizardDialogViewModel : WizardDialogViewModel
    {
        private Func<IEnumerable<WizardStepViewModel>, object> _dataCollector;
        private WizardOptions _options;
        private IEnumerable<WizardStepViewModel> _initialSteps;

        /// <summary>
        /// 初始化GenericWizardDialogViewModel（用于构造函数注入）
        /// </summary>
        /// <param name="steps">向导步骤列表</param>
        /// <param name="options">向导配置选项</param>
        /// <param name="dataCollector">自定义数据收集器</param>
        /// <param name="eventAggregator">事件聚合器</param>
        public GenericWizardDialogViewModel(
            IEnumerable<WizardStepViewModel> steps,
            WizardOptions options = null,
            Func<IEnumerable<WizardStepViewModel>, object> dataCollector = null,
            IEventAggregator eventAggregator = null
        ) : base(eventAggregator)
        {
            _initialSteps = steps ?? throw new ArgumentNullException(nameof(steps));
            _dataCollector = dataCollector;
            _options = options ?? new WizardOptions();
        }

        /// <summary>
        /// 初始化GenericWizardDialogViewModel（用于IDialogService）
        /// </summary>
        /// <param name="eventAggregator">事件聚合器</param>
        public GenericWizardDialogViewModel(IEventAggregator eventAggregator = null) : base(eventAggregator)
        {
            // 参数将通过DialogParameters传入
        }

        /// <summary>
        /// 向导标题，从配置选项中获取
        /// </summary>
        public new string Title => _options?.Title ?? base.Title;

        /// <summary>
        /// 向导配置选项
        /// </summary>
        public WizardOptions Options => _options;

        /// <summary>
        /// 初始化向导步骤
        /// </summary>
        /// <param name="parameters">对话框参数</param>
        protected override void InitializeSteps(IDialogParameters parameters)
        {
            // 清空现有步骤
            ClearSteps();
            
            // 如果没有通过构造函数传入步骤，尝试从参数中获取
            if (_initialSteps?.Count() == 0 && parameters != null)
            {
                _initialSteps = parameters.GetValue<IEnumerable<WizardStepViewModel>>("Steps");
                _options = parameters.GetValue<WizardOptions>("Options") ?? new WizardOptions();
                _dataCollector = parameters.GetValue<Func<IEnumerable<WizardStepViewModel>, object>>("DataCollector");
                
                // 如果参数中有标题，更新标题
                var title = parameters.GetValue<string>("Title");
                if (!string.IsNullOrEmpty(title))
                {
                    _options.Title = title;
                }
            }
            
            // 验证步骤列表
            if (_initialSteps == null)
            {
                throw new ArgumentException("未提供向导步骤列表。请通过构造函数或DialogParameters传入Steps参数。");
            }
            
            // 添加传入的步骤
            foreach (var step in _initialSteps)
            {
                if (step != null)
                {
                    AddStep(step);
                }
            }
            
            System.Diagnostics.Debug.WriteLine($"GenericWizardDialogViewModel: Initialized {Steps.Count} steps");
        }

        /// <summary>
        /// 收集向导数据
        /// </summary>
        /// <returns>收集的数据对象</returns>
        protected override object CollectWizardData()
        {
            try
            {
                // 如果提供了自定义数据收集器，使用它
                if (_dataCollector != null)
                {
                    return _dataCollector(Steps);
                }
                
                // 否则使用默认的数据收集逻辑
                return base.CollectWizardData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"数据收集器执行失败: {ex.Message}");
                // 如果数据收集器失败，回退到基类实现
                return base.CollectWizardData();
            }
        }
    }
}