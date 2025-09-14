using Moq;
using Prism.Events;
using Prism.Services.Dialogs;
using SmartWizard.ViewModels;
using Xunit;

namespace SmartWizard.Tests.ViewModels
{
    public class WizardDialogViewModelTests
    {
        private readonly Mock<IEventAggregator> _mockEventAggregator;
        private readonly WizardDialogViewModel _viewModel;

        public WizardDialogViewModelTests()
        {
            _mockEventAggregator = new Mock<IEventAggregator>();
            _viewModel = new WizardDialogViewModel(_mockEventAggregator.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Assert
            Assert.Equal("配置向导", _viewModel.Title);
            Assert.NotNull(_viewModel.Steps);
            Assert.Equal(0, _viewModel.CurrentStepIndex);
            Assert.NotNull(_viewModel.GoBackCommand);
            Assert.NotNull(_viewModel.GoNextCommand);
            Assert.NotNull(_viewModel.CancelCommand);
        }

        [Fact]
        public void AddStep_ShouldAddStepToCollection()
        {
            // Arrange
            var step = new TestWizardStep();

            // Act
            _viewModel.AddStep(step);

            // Assert
            Assert.Single(_viewModel.Steps);
            Assert.Equal(step, _viewModel.Steps[0]);
        }

        [Fact]
        public void CurrentStep_ShouldReturnCorrectStep()
        {
            // Arrange
            var step1 = new TestWizardStep();
            var step2 = new TestWizardStep();
            _viewModel.AddStep(step1);
            _viewModel.AddStep(step2);

            // Act & Assert
            Assert.Equal(step1, _viewModel.CurrentStep);
        }

        [Fact]
        public void CanGoBack_ShouldReturnFalseForFirstStep()
        {
            // Arrange
            _viewModel.AddStep(new TestWizardStep());

            // Act & Assert
            Assert.False(_viewModel.CanGoBack);
        }

        [Fact]
        public void CanGoNext_ShouldReturnTrueWhenCurrentStepIsValid()
        {
            // Arrange
            var step = new TestWizardStep { IsValid = true };
            _viewModel.AddStep(step);

            // Act & Assert
            Assert.True(_viewModel.CanGoNext);
        }

        [Fact]
        public void IsLastStep_ShouldReturnTrueForLastStep()
        {
            // Arrange
            _viewModel.AddStep(new TestWizardStep());

            // Act & Assert
            Assert.True(_viewModel.IsLastStep);
        }

        [Fact]
        public void NextButtonText_ShouldShowFinishForLastStep()
        {
            // Arrange
            _viewModel.AddStep(new TestWizardStep());

            // Act & Assert
            Assert.Equal("完成", _viewModel.NextButtonText);
        }

        [Fact]
        public void GoBackCommand_ShouldNotExecuteOnFirstStep()
        {
            // Arrange
            _viewModel.AddStep(new TestWizardStep());

            // Act & Assert
            Assert.False(_viewModel.GoBackCommand.CanExecute());
        }

        [Fact]
        public void GoNextCommand_ShouldNotExecuteWhenStepIsInvalid()
        {
            // Arrange
            var step = new TestWizardStep { IsValid = false };
            _viewModel.AddStep(step);

            // Act & Assert
            Assert.False(_viewModel.GoNextCommand.CanExecute());
        }

        private class TestWizardStep : WizardStepViewModel
        {
            public override string Title => "Test Step";

            public override bool ValidateStep() => IsValid;
        }
    }
}