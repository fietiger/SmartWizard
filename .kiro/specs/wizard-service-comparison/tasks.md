# Implementation Plan

- [x] 1. 更新MainWindowViewModel以支持IWizardService





  - 在MainWindowViewModel构造函数中注入IWizardService依赖
  - 添加ShowConfigWizardWithServiceCommand命令属性
  - 实现ExecuteShowConfigWizardWithService方法，使用IWizardService.ShowWizard调用向导
  - 在结果处理中添加"IWizardService方式"标识以区分实现方式
  - _Requirements: 1.2, 1.3, 3.1, 3.2_

- [x] 2. 更新MainWindow.xaml界面布局





  - 修改按钮布局从单个按钮改为两个按钮的水平排列
  - 更新现有按钮文本为"使用IDialogService打开向导"
  - 添加新按钮"使用IWizardService打开向导"，绑定到ShowConfigWizardWithServiceCommand
  - 确保两个按钮具有一致的样式和间距
  - _Requirements: 1.1, 2.1_


- [x] 3. 更新现有IDialogService实现的结果标识




  - 修改ExecuteShowConfigWizard方法中的结果字符串
  - 在成功、取消和错误消息中添加"IDialogService方式"标识
  - 保持现有的数据显示格式不变
  - _Requirements: 2.1, 2.2, 4.2_

- [x] 4. 实现错误处理和异常管理





  - 在ExecuteShowConfigWizardWithService方法中添加try-catch块
  - 处理InvalidOperationException（向导未注册）
  - 处理容器解析异常和其他通用异常
  - 确保错误消息包含"IWizardService方式"标识
  - _Requirements: 3.3, 4.3_

- [x] 5. 验证向导注册和服务集成



  - 确认ConfigWizard已在DemoModule中正确注册到IWizardService
  - 验证IWizardService能够正确解析和调用ConfigWizardDialogView
  - 测试完整的向导流程从调用到结果返回
  - _Requirements: 3.1, 3.2_

- [ ] 6. 测试两种实现方式的功能对等性




  - 验证两种方式都能正确显示相同的向导界面
  - 测试两种方式的成功完成流程，确保返回相同格式的配置数据
  - 测试两种方式的取消操作，验证取消消息的正确标识
  - 测试异常情况下两种方式的错误处理
  - _Requirements: 2.3, 4.1, 4.2, 4.3_