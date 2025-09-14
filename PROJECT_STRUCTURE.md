# SmartWizard 项目结构

```
SmartWizard/
├── src/
│   ├── SmartWizard/                    # 核心库项目
│   │   ├── ViewModels/
│   │   │   ├── WizardStepViewModel.cs      # 步骤基类
│   │   │   └── WizardDialogViewModel.cs    # 主对话框ViewModel
│   │   ├── Views/
│   │   │   ├── WizardDialogView.xaml       # 主对话框视图
│   │   │   └── WizardDialogView.xaml.cs
│   │   ├── Services/
│   │   │   ├── IWizardService.cs           # 向导服务接口
│   │   │   └── WizardService.cs            # 向导服务实现
│   │   ├── Events/
│   │   │   └── WizardCompletedEvent.cs     # 完成事件
│   │   ├── Converters/
│   │   │   └── IndexConverter.cs           # 索引转换器
│   │   ├── SmartWizardModule.cs            # Prism模块
│   │   └── SmartWizard.csproj
│   └── SmartWizard.Demo/               # 演示项目
│       ├── ViewModels/
│       │   ├── MainWindowViewModel.cs      # 主窗口ViewModel
│       │   ├── ConfigWizardViewModel.cs    # 配置向导ViewModel
│       │   ├── Step1ViewModel.cs           # 步骤1ViewModel
│       │   └── Step2ViewModel.cs           # 步骤2ViewModel
│       ├── Views/
│       │   ├── MainWindow.xaml             # 主窗口
│       │   └── ConfigWizardView.xaml       # 配置向导视图
│       ├── App.xaml                        # 应用程序入口
│       ├── DemoModule.cs                   # 演示模块
│       └── SmartWizard.Demo.csproj
├── tests/
│   └── SmartWizard.Tests/              # 单元测试项目
│       ├── ViewModels/
│       │   └── WizardDialogViewModelTests.cs
│       ├── Services/
│       │   └── WizardServiceTests.cs
│       └── SmartWizard.Tests.csproj
├── SmartWizard.sln                     # 解决方案文件
├── README.md                           # 项目说明
├── CHANGELOG.md                        # 更新日志
├── PROJECT_STRUCTURE.md                # 项目结构说明
├── Directory.Build.props               # 全局构建属性
├── nuget.config                        # NuGet配置
├── build.cmd                           # 构建脚本
├── .gitignore                          # Git忽略文件
└── LICENSE                             # 许可证文件
```

## 核心组件说明

### 1. SmartWizard 核心库
- **WizardStepViewModel**: 所有向导步骤的抽象基类
- **WizardDialogViewModel**: 主向导对话框的ViewModel，实现IDialogAware
- **IWizardService**: 向导服务接口，提供注册和显示向导的功能
- **SmartWizardModule**: Prism模块，负责依赖注入注册

### 2. 演示项目
- 展示如何创建自定义向导
- 包含两个示例步骤：基本信息和偏好设置
- 演示数据验证和收集

### 3. 测试项目
- 完整的单元测试覆盖
- 使用Moq进行Mock测试
- 测试ViewModel逻辑和服务接口

## 技术栈

- **.NET 6+**: 目标框架
- **WPF**: UI框架
- **Prism 8+**: MVVM框架
- **DryIoc**: 依赖注入容器
- **xUnit**: 单元测试框架
- **Moq**: Mock框架

## 构建和运行

1. **构建项目**:
   ```bash
   dotnet build
   ```

2. **运行演示**:
   ```bash
   dotnet run --project src/SmartWizard.Demo
   ```

3. **运行测试**:
   ```bash
   dotnet test
   ```

4. **打包NuGet**:
   ```bash
   dotnet pack src/SmartWizard --configuration Release
   ```