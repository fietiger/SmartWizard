# 更新日志

## [1.0.0] - 2025-01-14

### 新增
- 🎯 基于 Prism MVVM + Dialog Service 的向导控件
- 📋 多步骤支持和动态导航
- ✅ 实时数据验证和状态管理
- 🎨 可自定义的 UI 模板和样式
- 🔄 完整的 IDialogAware 实现
- 📦 模块化设计，支持独立部署
- 🧪 完整的单元测试覆盖
- 📖 详细的文档和示例项目

### 核心组件
- `WizardStepViewModel`: 步骤基类
- `WizardDialogViewModel`: 主对话框 ViewModel
- `IWizardService`: 向导服务接口
- `SmartWizardModule`: Prism 模块

### 特性
- 支持 .NET 6+ 和 WPF
- 集成 Prism 8+ 框架
- 支持依赖注入和事件聚合
- 完全基于 MVVM 模式
- 支持单元测试和 Mock