using SmartWizard.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SmartWizard.Demo.ViewModels
{
    /// <summary>
    /// 演示步骤2：偏好设置
    /// </summary>
    public class Step2ViewModel : WizardStepViewModel
    {
        private string _selectedLanguage;
        private bool _enableNotifications;
        private string _theme;

        public override string Title => "偏好设置";

        public List<string> AvailableLanguages { get; } = new List<string>
        {
            "中文", "English", "日本語", "Français"
        };

        public List<string> AvailableThemes { get; } = new List<string>
        {
            "浅色", "深色", "自动"
        };

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    IsValid = ValidateStep();
                }
            }
        }

        public bool EnableNotifications
        {
            get => _enableNotifications;
            set => SetProperty(ref _enableNotifications, value);
        }

        public string Theme
        {
            get => _theme;
            set
            {
                if (SetProperty(ref _theme, value))
                {
                    IsValid = ValidateStep();
                }
            }
        }

        public override bool ValidateStep()
        {
            return !string.IsNullOrEmpty(SelectedLanguage) && 
                   !string.IsNullOrEmpty(Theme);
        }

        public Step2ViewModel()
        {
            // 设置默认值
            _selectedLanguage = "";
            _theme = "";
        }

        public override void OnStepActivated()
        {
            base.OnStepActivated();
            
            // 设置默认值
            if (string.IsNullOrEmpty(SelectedLanguage))
                SelectedLanguage = AvailableLanguages.First();
            
            if (string.IsNullOrEmpty(Theme))
                Theme = AvailableThemes.First();
                
            IsValid = ValidateStep();
        }
    }
}