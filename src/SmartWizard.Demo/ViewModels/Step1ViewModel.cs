using SmartWizard.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace SmartWizard.Demo.ViewModels
{
    /// <summary>
    /// 演示步骤1：基本信息
    /// </summary>
    public class Step1ViewModel : WizardStepViewModel
    {
        private string _name;
        private string _email;

        public override string Title => "基本信息";

        [Required(ErrorMessage = "姓名不能为空")]
        public string Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    IsValid = ValidateStep();
                }
            }
        }

        [Required(ErrorMessage = "邮箱不能为空")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确")]
        public string Email
        {
            get => _email;
            set
            {
                if (SetProperty(ref _email, value))
                {
                    IsValid = ValidateStep();
                }
            }
        }

        public override bool ValidateStep()
        {
            return !string.IsNullOrWhiteSpace(Name) && 
                   !string.IsNullOrWhiteSpace(Email) &&
                   Email.Contains("@");
        }

        public override void OnStepActivated()
        {
            base.OnStepActivated();
            // 初始化时设置为无效，用户需要填写信息
            //IsValid = ValidateStep();
        }

        public Step1ViewModel()
        {
            // 初始化空值
            _name = "";
            _email = "";
        }
    }
}