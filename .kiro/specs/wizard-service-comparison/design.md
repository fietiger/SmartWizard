# Design Document

## Overview

本设计文档描述了如何在SmartWizard.Demo应用程序中添加一个新按钮，该按钮使用IWizardService来实现向导功能，以便与现有的直接使用IDialogService的方式进行对比。设计将保持现有架构的完整性，同时展示两种不同实现方式的差异。

## Architecture

### Current Architecture Analysis

当前的SmartWizard.Demo应用程序架构如下：

1. **SmartWizardModule**: 注册IWizardService服务和基础组件
2. **DemoModule**: 注册对话框视图、ViewModels，并在初始化时向IWizardService注册向导
3. **MainWindowViewModel**: 当前只使用IDialogService直接显示向导
4. **IWizardService**: 提供向导注册和显示功能，内部使用IDialogService

### Proposed Architecture Changes

设计将在现有架构基础上进行最小化修改：

1. **MainWindowViewModel扩展**: 添加新的命令和方法来使用IWizardService
2. **UI更新**: 在MainWindow.xaml中添加新按钮
3. **结果区分**: 修改结果显示逻辑以区分两种实现方式

## Components and Interfaces

### 1. MainWindowViewModel 扩展

**新增属性和命令:**
```csharp
public DelegateCommand ShowConfigWizardWithServiceCommand { get; }
private readonly IWizardService _wizardService;
```

**新增方法:**
```csharp
private void ExecuteShowConfigWizardWithService()
```

**修改构造函数:**
- 注入IWizardService依赖
- 初始化新命令

### 2. MainWindow.xaml 更新

**UI布局修改:**
- 将单个按钮改为两个按钮的水平布局
- 第一个按钮：保持现有的"使用IDialogService打开向导"
- 第二个按钮：新增"使用IWizardService打开向导"

**按钮样式:**
- 保持一致的视觉风格
- 使用清晰的标签区分两种方式

### 3. 结果显示逻辑

**结果格式化:**
- 在结果字符串中添加实现方式标识
- 保持相同的数据显示格式
- 区分成功、取消和错误状态的标识

## Data Models

### ConfigResult Model
现有的ConfigResult模型无需修改，两种实现方式都将使用相同的数据结构：

```csharp
public class ConfigResult
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string Theme { get; set; }
    public bool EnableNotifications { get; set; }
}
```

### Dialog Parameters
两种实现方式都将使用相同的DialogParameters结构，确保数据传递的一致性。

## Error Handling

### IWizardService 特定错误处理

1. **向导未注册错误**: 
   - 捕获InvalidOperationException
   - 显示友好的错误消息，包含实现方式标识

2. **服务解析错误**:
   - 捕获容器解析异常
   - 提供详细的错误信息

3. **通用异常处理**:
   - 保持与现有IDialogService相同的异常处理模式
   - 在错误消息中标明使用的实现方式

### Error Message Format
```
错误: [错误描述] (IWizardService方式)
堆栈: [堆栈信息]
```

## Testing Strategy

### Unit Testing Approach

1. **MainWindowViewModel测试扩展**:
   - 测试新命令的初始化
   - 测试IWizardService的正确调用
   - 测试结果格式化逻辑

2. **Mock Strategy**:
   - Mock IWizardService接口
   - 验证ShowWizard方法的调用参数
   - 测试各种回调场景（成功、取消、错误）

3. **Integration Testing**:
   - 验证IWizardService与IDialogService的集成
   - 测试向导注册和调用的完整流程

### Test Scenarios

1. **成功场景**:
   - 用户完成向导配置
   - 验证结果显示包含正确的实现方式标识

2. **取消场景**:
   - 用户取消向导
   - 验证取消消息包含实现方式标识

3. **错误场景**:
   - 向导未注册
   - 服务解析失败
   - 验证错误消息的格式和内容

## Implementation Considerations

### Dependency Injection
- IWizardService已在SmartWizardModule中注册为单例
- MainWindowViewModel需要通过构造函数注入IWizardService
- 保持现有的IContainerProvider注入以维持向后兼容性

### Wizard Registration
- ConfigWizard已在DemoModule.OnInitialized中注册
- 无需额外的注册代码
- 使用现有的"ConfigWizard"名称和"ConfigWizardDialogView"视图

### UI Responsiveness
- 两个按钮应具有相同的响应性
- 保持现有的异步处理模式
- 确保UI在向导显示期间保持响应

### Code Reusability
- 最大化代码复用，避免重复逻辑
- 考虑提取公共的结果格式化方法
- 保持现有代码结构的清晰性

## Performance Considerations

### Memory Usage
- IWizardService使用字典缓存向导注册信息
- 两种实现方式都使用相同的底层IDialogService
- 内存开销增加最小

### Execution Performance
- IWizardService增加了一层间接调用
- 性能影响可忽略不计
- 向导注册查找为O(1)操作

## Security Considerations

### Input Validation
- 保持现有的输入验证逻辑
- IWizardService内部已包含参数验证
- 无需额外的安全措施

### Error Information Exposure
- 错误消息应提供足够的调试信息
- 避免暴露敏感的内部实现细节
- 保持与现有错误处理的一致性