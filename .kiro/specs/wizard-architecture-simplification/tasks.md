# 实现计划

- [x] 1. 创建基础配置和选项类




  - 实现WizardOptions类，提供向导的基本配置选项
  - 创建单元测试验证WizardOptions的各种配置组合
  - _需求: 1.1, 5.2_

- [x] 2. 扩展IWizardService接口




  - 在IWizardService接口中添加新的简化方法重载
  - 保持现有方法签名不变，确保向后兼容性
  - _需求: 1.1, 4.1, 4.2_

- [x] 3. 实现GenericWizardDialogViewModel




  - 创建GenericWizardDialogViewModel类，继承自WizardDialogViewModel
  - 实现构造函数接受步骤列表和配置选项
  - 重写InitializeSteps和CollectWizardData方法
  - _需求: 1.1, 3.1, 3.2_
-

- [x] 4. 扩展WizardService实现




  - 在WizardService类中实现新的简化方法
  - 添加ShowWizardInternal私有方法处理通用逻辑
  - 确保新方法与现有方法和谐共存
  - _需求: 1.1, 4.2, 6.1_

- [x] 5. 更新模块注册





  - 修改SmartWizardModule注册GenericWizardDialogViewModel
  - 确保WizardDialogView已正确注册为对话框
  - 验证依赖注入配置的正确性
  - _需求: 1.3, 4.1_

- [x] 6. 创建简化API的单元测试




  - 测试GenericWizardDialogViewModel的各种初始化场景
  - 测试WizardService新方法的参数验证和错误处理
  - 测试数据收集器的正确性
  - _需求: 3.1, 3.2, 3.3_

- [x] 7. 更新Demo项目展示简化用法






  - 在MainWindowViewModel中添加使用简化API的示例方法
  - 创建对比按钮展示新旧两种方式的差异
  - 验证简化方式能正确收集和返回数据
  - _需求: 5.1, 5.2, 6.1_

- [ ] 8. 添加错误处理和验证
  - 在WizardService中添加参数验证逻辑
  - 实现步骤列表为空或无效时的错误处理
  - 添加数据收集器异常的捕获和处理
  - _需求: 3.3, 5.3_

- [ ] 9. 创建集成测试
  - 测试简化API与现有API的兼容性
  - 验证同一项目中可以同时使用两种方式
  - 测试不同步骤组合的向导创建
  - _需求: 4.1, 4.2, 6.4_

- [ ] 10. 优化和文档完善
  - 为新API添加XML文档注释
  - 创建使用示例和最佳实践指南
  - 验证所有功能按预期工作并进行最终调优
  - _需求: 5.1, 5.4_