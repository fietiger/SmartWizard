# SmartWizard

一个基于 Prism MVVM + Dialog Service 的 WPF 向导控件库，支持多步骤引导流程、动态验证和模态对话框集成。

## ✨ 特性

- 🎯 **Prism 集成**: 深度集成 Prism 框架，支持 MVVM、依赖注入和事件聚合
- 🔄 **Dialog Service**: 基于 `IDialogService` 的模态对话框管理
- 📋 **多步骤支持**: 灵活的步骤管理和导航控制
- ✅ **动态验证**: 实时验证步骤数据，控制导航状态
- 🎨 **可定制界面**: 支持自定义步骤模板和样式
- 🧪 **易于测试**: 完全基于接口设计，支持单元测试和 Mock
- 📦 **模块化**: 可作为独立 Prism 模块集成到现有应用

## 🚀 快速开始

### 安装

```xml
<PackageReference Include="SmartWizard" Version="1.0.0" />
<PackageReference Include="Prism.Wpf" Version="8.1.97" />
```

### 基本用法

1. **注册模块**

```csharp
protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
{
    moduleCatalog.AddModule<SmartWizardModule>();
}
```

2. **创建步骤 ViewModel**

```csharp
public class Step1ViewModel : WizardStepViewModel
{
    private string _name;

    public override string Title => "基本信息";

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

    public override bool ValidateStep()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }
}
```

3. **创建向导 ViewModel**

```csharp
public class MyWizardViewModel : WizardDialogViewModel
{
    public MyWizardViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
    {
    }

    protected override void InitializeSteps(IDialogParameters parameters)
    {
        AddStep(new Step1ViewModel());
        AddStep(new Step2ViewModel());
    }

    protected override object CollectWizardData()
    {
        // 收集所有步骤的数据
        return new { /* 结果数据 */ };
    }
}
```

4. **注册和显示向导**

```csharp
// 注册
containerRegistry.RegisterDialog<MyWizardView, MyWizardViewModel>();
wizardService.RegisterWizard("MyWizard", "MyWizardView");

// 显示
_wizardService.ShowWizard("MyWizard", parameters, result =>
{
    if (result.Result == ButtonResult.OK)
    {
        var data = result.Parameters.GetValue<object>("WizardResult");
        // 处理完成数据
    }
});
```

## 📖 详细文档

### 核心组件

#### WizardStepViewModel
所有向导步骤的基类，提供：
- `Title`: 步骤标题
- `IsValid`: 步骤验证状态
- `ValidateStep()`: 验证逻辑
- `OnStepActivated()` / `OnStepDeactivated()`: 生命周期回调

#### WizardDialogViewModel
向导对话框的主 ViewModel，实现 `IDialogAware`：
- 步骤管理和导航
- 命令处理（上一步/下一步/取消）
- 数据收集和结果返回
- 事件通信支持

#### IWizardService
向导服务接口，提供：
- `RegisterWizard()`: 注册向导
- `ShowWizard()`: 显示向导对话框

### 架构设计

```
WizardDialogViewModel (IDialogAware)
├── Steps: ObservableCollection<WizardStepViewModel>
├── CurrentStep: WizardStepViewModel
├── Navigation Commands (DelegateCommand)
└── Dialog Result Handling

WizardStepViewModel (BindableBase)
├── Title: string
├── IsValid: bool
└── Validation Logic
```

## 🎨 自定义样式

向导控件支持完全的样式自定义：

```xml
<Style x:Key="CustomWizardStyle" TargetType="UserControl">
    <!-- 自定义样式 -->
</Style>
```

## 🧪 测试

项目包含完整的单元测试：

```bash
dotnet test
```

测试覆盖：
- ViewModel 逻辑测试
- 服务接口测试
- 命令执行测试
- Mock 对象支持

## 📋 示例项目

查看 `SmartWizard.Demo` 项目了解完整的使用示例，包括：
- 多步骤配置向导
- 数据验证和收集
- 主题和本地化支持

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

MIT License - 详见 [LICENSE](LICENSE) 文件

## 🔗 相关链接

- [Prism Library 官方文档](https://prismlibrary.com/)
- [WPF 官方文档](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)