using System.Windows;
using System.Windows.Controls;

namespace SmartWizard.ViewModels
{
    /// <summary>
    /// 向导配置选项类，提供向导的基本配置选项
    /// </summary>
    public class WizardOptions
    {
        /// <summary>
        /// 向导标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 向导窗口宽度
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// 向导窗口高度
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// 是否显示步骤指示器
        /// </summary>
        public bool ShowStepIndicator { get; set; } = true;

        /// <summary>
        /// 下一步按钮文本
        /// </summary>
        public string NextButtonText { get; set; } = "下一步";

        /// <summary>
        /// 上一步按钮文本
        /// </summary>
        public string BackButtonText { get; set; } = "上一步";

        /// <summary>
        /// 取消按钮文本
        /// </summary>
        public string CancelButtonText { get; set; } = "取消";

        /// <summary>
        /// 完成按钮文本
        /// </summary>
        public string FinishButtonText { get; set; } = "完成";

        /// <summary>
        /// 窗口样式
        /// </summary>
        public Style WindowStyle { get; set; }

        /// <summary>
        /// 自定义步骤模板
        /// </summary>
        public DataTemplate CustomStepTemplate { get; set; }
    }
}