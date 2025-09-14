using Prism.Services.Dialogs;
using SmartWizard.ViewModels;
using System;
using System.Collections.Generic;

namespace SmartWizard.Services
{
    /// <summary>
    /// 向导服务接口
    /// </summary>
    public interface IWizardService
    {
        /// <summary>
        /// 显示向导对话框
        /// </summary>
        /// <param name="wizardName">向导名称</param>
        /// <param name="parameters">初始化参数</param>
        /// <param name="callback">完成回调</param>
        void ShowWizard(string wizardName, IDialogParameters parameters, Action<IDialogResult> callback);

        /// <summary>
        /// 注册向导
        /// </summary>
        /// <param name="wizardName">向导名称</param>
        /// <param name="viewName">视图名称</param>
        void RegisterWizard(string wizardName, string viewName);

        /// <summary>
        /// 显示向导对话框（简化方式，带泛型数据收集器）
        /// </summary>
        /// <typeparam name="TResult">数据收集器返回的结果类型</typeparam>
        /// <param name="title">向导标题</param>
        /// <param name="steps">向导步骤列表</param>
        /// <param name="callback">完成回调</param>
        /// <param name="dataCollector">数据收集器函数</param>
        /// <param name="options">向导配置选项</param>
        void ShowWizard<TResult>(
            string title,
            IEnumerable<WizardStepViewModel> steps,
            Action<IDialogResult> callback,
            Func<IEnumerable<WizardStepViewModel>, TResult> dataCollector = null,
            WizardOptions options = null
        );

        /// <summary>
        /// 显示向导对话框（简化方式）
        /// </summary>
        /// <param name="title">向导标题</param>
        /// <param name="steps">向导步骤列表</param>
        /// <param name="callback">完成回调</param>
        /// <param name="options">向导配置选项</param>
        void ShowWizard(
            string title,
            IEnumerable<WizardStepViewModel> steps,
            Action<IDialogResult> callback,
            WizardOptions options = null
        );
    }
}