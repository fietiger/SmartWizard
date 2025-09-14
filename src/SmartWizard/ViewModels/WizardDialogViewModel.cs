using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using SmartWizard.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartWizard.ViewModels
{
    /// <summary>
    /// 向导对话框的主ViewModel
    /// </summary>
    public class WizardDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator _eventAggregator;
        private int _currentStepIndex;
        private string _nextButtonText = "下一步";

        public WizardDialogViewModel(IEventAggregator eventAggregator = null)
        {
            _eventAggregator = eventAggregator;
            Steps = new ObservableCollection<WizardStepViewModel>();
            
            GoBackCommand = new DelegateCommand(ExecuteGoBack, CanExecuteGoBack);
            GoNextCommand = new DelegateCommand(ExecuteGoNext, CanExecuteGoNext);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        #region Properties

        public string Title => "配置向导";

        /// <summary>
        /// 所有向导步骤
        /// </summary>
        public ObservableCollection<WizardStepViewModel> Steps { get; }

        /// <summary>
        /// 当前步骤索引
        /// </summary>
        public int CurrentStepIndex
        {
            get => _currentStepIndex;
            protected set
            {
                if (SetProperty(ref _currentStepIndex, value))
                {
                    RaisePropertyChanged(nameof(CurrentStep));
                    RaisePropertyChanged(nameof(CanGoBack));
                    RaisePropertyChanged(nameof(CanGoNext));
                    RaisePropertyChanged(nameof(IsLastStep));
                    UpdateNextButtonText();
                    UpdateCommandStates();
                }
            }
        }

        /// <summary>
        /// 当前步骤
        /// </summary>
        public WizardStepViewModel CurrentStep 
        { 
            get
            {
                var step = Steps.Count > 0 && CurrentStepIndex >= 0 && CurrentStepIndex < Steps.Count 
                    ? Steps[CurrentStepIndex] 
                    : null;
                System.Diagnostics.Debug.WriteLine($"CurrentStep getter: Index={CurrentStepIndex}, Count={Steps.Count}, Step={step?.GetType().Name}");
                return step;
            }
        }

        /// <summary>
        /// 是否可以返回上一步
        /// </summary>
        public bool CanGoBack => CurrentStepIndex > 0;

        /// <summary>
        /// 是否可以进入下一步
        /// </summary>
        public bool CanGoNext => CurrentStep?.IsValid == true;

        /// <summary>
        /// 是否为最后一步
        /// </summary>
        public bool IsLastStep => CurrentStepIndex == Steps.Count - 1;

        /// <summary>
        /// 下一步按钮文本
        /// </summary>
        public string NextButtonText
        {
            get => _nextButtonText;
            private set => SetProperty(ref _nextButtonText, value);
        }

        #endregion

        #region Commands

        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoNextCommand { get; }
        public DelegateCommand CancelCommand { get; }

        #endregion

        #region IDialogAware Implementation

        public event Action<IDialogResult> RequestClose = delegate { };

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //parameters.TryGet("steps",out var vals);
            System.Diagnostics.Debug.WriteLine("OnDialogOpened called");
            InitializeSteps(parameters);
            System.Diagnostics.Debug.WriteLine($"After InitializeSteps: {Steps.Count} steps");
            
            // 检查每个步骤
            for (int i = 0; i < Steps.Count; i++)
            {
                System.Diagnostics.Debug.WriteLine($"Step {i}: {Steps[i]?.GetType().Name} - {Steps[i]?.Title}");
            }
            
            if (Steps.Count > 0)
            {
                CurrentStepIndex = 0;
                System.Diagnostics.Debug.WriteLine($"CurrentStepIndex set to 0");
                System.Diagnostics.Debug.WriteLine($"CurrentStep check: Index={CurrentStepIndex}, Count={Steps.Count}");
                System.Diagnostics.Debug.WriteLine($"CurrentStep: {CurrentStep?.GetType().Name}");
                System.Diagnostics.Debug.WriteLine($"CurrentStep Title: {CurrentStep?.Title}");
                CurrentStep?.OnStepActivated();
                System.Diagnostics.Debug.WriteLine($"CurrentStep IsValid: {CurrentStep?.IsValid}");
                
                // 强制刷新UI绑定
                RaisePropertyChanged(nameof(CurrentStep));
                RaisePropertyChanged(nameof(Steps));
                UpdateCommandStates();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 初始化向导步骤（子类重写）
        /// </summary>
        protected virtual void InitializeSteps(IDialogParameters parameters)
        {
            // 子类实现具体的步骤初始化逻辑
        }

        /// <summary>
        /// 收集所有步骤的数据（子类重写）
        /// </summary>
        protected virtual object CollectWizardData()
        {
            // 子类实现数据收集逻辑
            return new { Message = "向导完成" };
        }

        #endregion

        #region Private Methods

        private bool CanExecuteGoBack() => CanGoBack;

        private void ExecuteGoBack()
        {
            if (!CanExecuteGoBack()) return;

            CurrentStep?.OnStepDeactivated();
            CurrentStepIndex--;
            CurrentStep?.OnStepActivated();
        }

        private bool CanExecuteGoNext() => CanGoNext;

        private void ExecuteGoNext()
        {
            if (!CanExecuteGoNext()) return;

            if (IsLastStep)
            {
                // 完成向导
                FinishWizard();
            }
            else
            {
                // 进入下一步
                CurrentStep?.OnStepDeactivated();
                CurrentStepIndex++;
                CurrentStep?.OnStepActivated();
            }
        }

        private void ExecuteCancel()
        {
            var result = new DialogResult(ButtonResult.Cancel);
            RequestClose?.Invoke(result);
        }

        private void FinishWizard()
        {
            var wizardData = CollectWizardData();
            var parameters = new DialogParameters
            {
                { "WizardResult", wizardData }
            };

            var result = new DialogResult(ButtonResult.OK, parameters);
            RequestClose?.Invoke(result);

            // 可选：发布完成事件
            _eventAggregator?.GetEvent<WizardCompletedEvent>()?.Publish(wizardData);
        }

        private void UpdateNextButtonText()
        {
            NextButtonText = IsLastStep ? "完成" : "下一步";
        }

        private void UpdateCommandStates()
        {
            GoBackCommand.RaiseCanExecuteChanged();
            GoNextCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Step Management

        /// <summary>
        /// 添加步骤
        /// </summary>
        public void AddStep(WizardStepViewModel step)
        {
            if (step == null) return;

            Steps.Add(step);
            step.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(WizardStepViewModel.IsValid))
                {
                    UpdateCommandStates();
                }
            };
            
            // 更新按钮文本和状态
            UpdateNextButtonText();
            UpdateCommandStates();
        }

        /// <summary>
        /// 移除步骤
        /// </summary>
        public void RemoveStep(WizardStepViewModel step)
        {
            Steps.Remove(step);
        }

        /// <summary>
        /// 清空所有步骤
        /// </summary>
        public void ClearSteps()
        {
            Steps.Clear();
            CurrentStepIndex = 0;
        }

        #endregion
    }
}