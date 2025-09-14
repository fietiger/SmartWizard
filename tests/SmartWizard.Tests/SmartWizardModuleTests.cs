using SmartWizard;
using SmartWizard.ViewModels;
using Xunit;

namespace SmartWizard.Tests
{
    /// <summary>
    /// SmartWizardModule注册测试
    /// </summary>
    public class SmartWizardModuleTests
    {
        [Fact]
        public void SmartWizardModule_ShouldBeInstantiable()
        {
            // Act
            var module = new SmartWizardModule();

            // Assert
            Assert.NotNull(module);
        }

        [Fact]
        public void GenericWizardDialogViewModel_ShouldBeCreatableWithSteps()
        {
            // Arrange
            var steps = new[] { new TestWizardStepViewModel() };
            var options = new WizardOptions { Title = "Test Wizard" };

            // Act
            var viewModel = new GenericWizardDialogViewModel(steps, options);

            // Assert
            Assert.NotNull(viewModel);
            Assert.Equal("Test Wizard", viewModel.Title);
        }

        [Fact]
        public void GenericWizardDialogViewModel_ShouldHaveCorrectOptions()
        {
            // Arrange
            var steps = new[] { new TestWizardStepViewModel() };
            var options = new WizardOptions 
            { 
                Title = "Test Wizard",
                ShowStepIndicator = false,
                NextButtonText = "继续"
            };

            // Act
            var viewModel = new GenericWizardDialogViewModel(steps, options);

            // Assert
            Assert.NotNull(viewModel.Options);
            Assert.Equal("Test Wizard", viewModel.Options.Title);
            Assert.False(viewModel.Options.ShowStepIndicator);
            Assert.Equal("继续", viewModel.Options.NextButtonText);
        }
    }

    /// <summary>
    /// 测试用的简单步骤ViewModel
    /// </summary>
    public class TestWizardStepViewModel : WizardStepViewModel
    {
        public override string Title => "Test Step";

        public override bool ValidateStep()
        {
            return true;
        }
    }
}