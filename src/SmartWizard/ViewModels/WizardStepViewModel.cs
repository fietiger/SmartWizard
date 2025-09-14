using Prism.Mvvm;

namespace SmartWizard.ViewModels
{
    /// <summary>
    /// 向导步骤的基础ViewModel
    /// </summary>
    public abstract class WizardStepViewModel : BindableBase
    {
        private bool _isValid;

        /// <summary>
        /// 步骤标题
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// 当前步骤是否有效（可以进入下一步）
        /// </summary>
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        /// <summary>
        /// 验证当前步骤数据
        /// </summary>
        /// <returns>验证是否通过</returns>
        public abstract bool ValidateStep();

        /// <summary>
        /// 步骤激活时调用
        /// </summary>
        public virtual void OnStepActivated() { }

        /// <summary>
        /// 步骤离开时调用
        /// </summary>
        public virtual void OnStepDeactivated() { }
    }
}