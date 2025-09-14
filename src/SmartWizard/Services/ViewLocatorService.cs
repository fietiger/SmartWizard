using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace SmartWizard.Services
{
    /// <summary>
    /// 视图定位服务实现
    /// </summary>
    public class ViewLocatorService : IViewLocatorService
    {
        private readonly Dictionary<Type, Type> _viewMappings;

        public ViewLocatorService()
        {
            _viewMappings = new Dictionary<Type, Type>();
        }

        public FrameworkElement LocateViewForViewModel(Type viewModelType)
        {
            if (viewModelType == null)
                return null;

            // 首先检查显式注册的映射
            if (_viewMappings.TryGetValue(viewModelType, out Type explicitViewType))
            {
                return CreateViewInstance(explicitViewType);
            }

            // 基于约定的View定位
            var conventionViewType = LocateViewByConvention(viewModelType);
            if (conventionViewType != null)
            {
                return CreateViewInstance(conventionViewType);
            }

            return null;
        }

        public void RegisterViewMapping(Type viewModelType, Type viewType)
        {
            if (viewModelType == null)
                throw new ArgumentNullException(nameof(viewModelType));
            if (viewType == null)
                throw new ArgumentNullException(nameof(viewType));

            if (!typeof(FrameworkElement).IsAssignableFrom(viewType))
                throw new ArgumentException($"View type {viewType.Name} must inherit from FrameworkElement", nameof(viewType));

            _viewMappings[viewModelType] = viewType;
        }

        public void RegisterViewMapping<TViewModel, TView>()
            where TView : FrameworkElement, new()
        {
            RegisterViewMapping(typeof(TViewModel), typeof(TView));
        }

        private Type LocateViewByConvention(Type viewModelType)
        {
            // 约定：ViewModel名称以"ViewModel"结尾，对应的View名称以"View"结尾
            // 例如：Step1ViewModel -> Step1View
            var viewModelName = viewModelType.Name;
            var viewModelNamespace = viewModelType.Namespace;

            // 尝试不同的命名约定
            var possibleViewNames = new[]
            {
                // Step1ViewModel -> Step1View
                viewModelName.Replace("ViewModel", "View"),
                // Step1ViewModel -> Step1UserControl
                viewModelName.Replace("ViewModel", "UserControl"),
                // Step1ViewModel -> Step1
                viewModelName.Replace("ViewModel", "")
            };

            // 尝试不同的命名空间约定
            var possibleNamespaces = new[]
            {
                // ViewModels -> Views
                viewModelNamespace?.Replace("ViewModels", "Views"),
                // ViewModels -> UserControls
                viewModelNamespace?.Replace("ViewModels", "UserControls"),
                // 同一命名空间
                viewModelNamespace
            };

            foreach (var namespaceName in possibleNamespaces)
            {
                if (string.IsNullOrEmpty(namespaceName))
                    continue;

                foreach (var viewName in possibleViewNames)
                {
                    var fullTypeName = $"{namespaceName}.{viewName}";
                    var viewType = viewModelType.Assembly.GetType(fullTypeName);
                    
                    if (viewType != null && typeof(FrameworkElement).IsAssignableFrom(viewType))
                    {
                        return viewType;
                    }

                    // 也尝试在引用的程序集中查找
                    foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        try
                        {
                            viewType = assembly.GetType(fullTypeName);
                            if (viewType != null && typeof(FrameworkElement).IsAssignableFrom(viewType))
                            {
                                return viewType;
                            }
                        }
                        catch
                        {
                            // 忽略程序集加载错误
                        }
                    }
                }
            }

            return null;
        }

        private FrameworkElement CreateViewInstance(Type viewType)
        {
            try
            {
                return (FrameworkElement)Activator.CreateInstance(viewType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to create view instance of type {viewType.Name}: {ex.Message}");
                return null;
            }
        }
    }
}