# Requirements Document

## Introduction

本功能旨在在SmartWizard.Demo应用程序中添加一个新的按钮，该按钮使用IWizardService来实现向导功能，而不是直接使用IDialogService。这将允许用户对比两种不同的向导实现方式：直接使用dialogService和使用IWizardService的差异，从而展示SmartWizard库的核心服务功能。

## Requirements

### Requirement 1

**User Story:** 作为开发者，我希望在演示程序中看到使用IWizardService的实现方式，以便了解该服务的使用方法和优势。

#### Acceptance Criteria

1. WHEN 用户启动SmartWizard.Demo应用程序 THEN 系统 SHALL 显示两个按钮：一个使用IDialogService，另一个使用IWizardService
2. WHEN 用户点击"使用IWizardService打开向导"按钮 THEN 系统 SHALL 通过IWizardService显示配置向导
3. WHEN 向导通过IWizardService完成 THEN 系统 SHALL 在结果区域显示配置结果，并标明使用的是IWizardService方式

### Requirement 2

**User Story:** 作为用户，我希望能够清楚地区分两种实现方式的结果，以便理解它们之间的差异。

#### Acceptance Criteria

1. WHEN 用户使用IWizardService完成向导 THEN 系统 SHALL 在结果中明确标识使用的是"IWizardService方式"
2. WHEN 用户使用IDialogService完成向导 THEN 系统 SHALL 在结果中明确标识使用的是"IDialogService方式"
3. WHEN 用户完成任一种方式的向导 THEN 系统 SHALL 显示相同的配置数据格式，便于对比

### Requirement 3

**User Story:** 作为开发者，我希望IWizardService能够正确注册和调用配置向导，以便验证服务的功能完整性。

#### Acceptance Criteria

1. WHEN 应用程序启动 THEN 系统 SHALL 正确注册配置向导到IWizardService
2. WHEN IWizardService调用向导 THEN 系统 SHALL 使用相同的ConfigWizardDialogView和相关ViewModels
3. IF 向导注册失败 THEN 系统 SHALL 在结果区域显示错误信息

### Requirement 4

**User Story:** 作为用户，我希望两种实现方式都能正常处理向导的取消操作，以便保持一致的用户体验。

#### Acceptance Criteria

1. WHEN 用户在IWizardService向导中点击取消 THEN 系统 SHALL 显示"用户取消了配置（IWizardService方式）"
2. WHEN 用户在IDialogService向导中点击取消 THEN 系统 SHALL 显示"用户取消了配置（IDialogService方式）"
3. WHEN 任一方式发生异常 THEN 系统 SHALL 显示详细的错误信息和使用的方式标识