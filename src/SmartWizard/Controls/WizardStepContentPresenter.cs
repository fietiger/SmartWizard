using SmartWizard.Services;
using SmartWizard.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SmartWizard.Controls
{
    /// <summary>
    /// 专门用于显示向导步骤内容的ContentPresenter
    /// 自动为步骤ViewModels定位对应的Views
    /// </summary>
    public class WizardStepContentPresenter : ContentPresenter
    {
        private static IViewLocatorService _viewLocatorService;

        static WizardStepContentPresenter()
        {
            // 重写Content属性的元数据，以便监听变化
            ContentProperty.OverrideMetadata(typeof(WizardStepContentPresenter), 
                new FrameworkPropertyMetadata(null, OnContentChanged));
        }

        /// <summary>
        /// 设置全局的视图定位服务
        /// </summary>
        /// <param name="viewLocatorService">视图定位服务实例</param>
        public static void SetViewLocatorService(IViewLocatorService viewLocatorService)
        {
            _viewLocatorService = viewLocatorService;
        }

        /// <summary>
        /// 视图定位服务依赖属性
        /// </summary>
        public static readonly DependencyProperty ViewLocatorServiceProperty =
            DependencyProperty.Register(
                nameof(ViewLocatorService),
                typeof(IViewLocatorService),
                typeof(WizardStepContentPresenter),
                new PropertyMetadata(null, OnViewLocatorServiceChanged));

        /// <summary>
        /// 视图定位服务
        /// </summary>
        public IViewLocatorService ViewLocatorService
        {
            get => (IViewLocatorService)GetValue(ViewLocatorServiceProperty);
            set => SetValue(ViewLocatorServiceProperty, value);
        }

        private static void OnViewLocatorServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WizardStepContentPresenter presenter)
            {
                presenter.UpdateContent();
            }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WizardStepContentPresenter presenter)
            {
                presenter.UpdateContent();
            }
        }

        private void UpdateContent()
        {
            var content = Content;
            
            // 如果内容是WizardStepViewModel，尝试为其定位View
            if (content is WizardStepViewModel stepViewModel)
            {
                var viewLocator = ViewLocatorService ?? _viewLocatorService;
                if (viewLocator != null)
                {
                    var view = viewLocator.LocateViewForViewModel(stepViewModel.GetType());
                    if (view != null)
                    {
                        // 设置DataContext并使用View作为内容
                        view.DataContext = stepViewModel;
                        
                        // 通过设置Content来显示View，但要避免无限递归
                        if (Content != view)
                        {
                            SetCurrentValue(ContentProperty, view);
                        }
                        return;
                    }
                }
            }

            // 如果没有找到对应的View，保持原有内容不变
        }
    }
}