using System;
using System.Windows;

namespace SmartWizard.Services
{
    /// <summary>
    /// 视图定位服务接口
    /// </summary>
    public interface IViewLocatorService
    {
        /// <summary>
        /// 为指定的ViewModel类型定位对应的View
        /// </summary>
        /// <param name="viewModelType">ViewModel类型</param>
        /// <returns>View实例，如果找不到则返回null</returns>
        FrameworkElement LocateViewForViewModel(Type viewModelType);

        /// <summary>
        /// 注册ViewModel和View的映射关系
        /// </summary>
        /// <param name="viewModelType">ViewModel类型</param>
        /// <param name="viewType">View类型</param>
        void RegisterViewMapping(Type viewModelType, Type viewType);

        /// <summary>
        /// 注册ViewModel和View的映射关系（泛型版本）
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel类型</typeparam>
        /// <typeparam name="TView">View类型</typeparam>
        void RegisterViewMapping<TViewModel, TView>()
            where TView : FrameworkElement, new();
    }
}