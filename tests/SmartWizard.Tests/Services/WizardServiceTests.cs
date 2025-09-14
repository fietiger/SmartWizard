using Moq;
using Prism.Services.Dialogs;
using Prism.Events;
using SmartWizard.Services;
using SmartWizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartWizard.Tests.Services
{
    public class WizardServiceTests
    {
        private readonly Mock<IDialogService> _mockDialogService;
        private readonly Mock<IEventAggregator> _mockEventAggregator;
        private readonly WizardService _wizardService;

        public WizardServiceTests()
        {
            _mockDialogService = new Mock<IDialogService>();
            _mockEventAggregator = new Mock<IEventAggregator>();
            _wizardService = new WizardService(_mockDialogService.Object, _mockEventAggregator.Object);
        }

        private class TestStep : WizardStepViewModel
        {
            public override string Title { get; }
            public string Data { get; set; }

            public TestStep(string title, string data = null)
            {
                Title = title;
                Data = data;
            }

            public override bool ValidateStep() => true;
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithNullDialogService_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WizardService(null));
        }

        [Fact]
        public void Constructor_WithValidDialogService_ShouldInitialize()
        {
            // Act
            var service = new WizardService(_mockDialogService.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithEventAggregator_ShouldInitialize()
        {
            // Act
            var service = new WizardService(_mockDialogService.Object, _mockEventAggregator.Object);

            // Assert
            Assert.NotNull(service);
        }

        #endregion

        #region Traditional API Tests

        [Fact]
        public void RegisterWizard_WithValidParameters_ShouldRegisterWizard()
        {
            // Arrange
            var wizardName = "TestWizard";
            var viewName = "TestView";

            // Act
            _wizardService.RegisterWizard(wizardName, viewName);

            // Assert - No exception should be thrown
        }

        [Fact]
        public void RegisterWizard_WithNullWizardName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.RegisterWizard(null, "TestView"));
        }

        [Fact]
        public void RegisterWizard_WithEmptyWizardName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.RegisterWizard("", "TestView"));
        }

        [Fact]
        public void RegisterWizard_WithNullViewName_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.RegisterWizard("TestWizard", null));
        }

        [Fact]
        public void ShowWizard_WithRegisteredWizard_ShouldCallDialogService()
        {
            // Arrange
            var wizardName = "TestWizard";
            var viewName = "TestView";
            var parameters = new DialogParameters();
            Action<IDialogResult> callback = result => { };

            _wizardService.RegisterWizard(wizardName, viewName);

            // Act
            _wizardService.ShowWizard(wizardName, parameters, callback);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(viewName, parameters, callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_WithUnregisteredWizard_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var wizardName = "UnregisteredWizard";
            var parameters = new DialogParameters();
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _wizardService.ShowWizard(wizardName, parameters, callback));
        }

        [Fact]
        public void ShowWizard_WithNullWizardName_ShouldThrowArgumentException()
        {
            // Arrange
            var parameters = new DialogParameters();
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard(null, parameters, callback));
        }

        #endregion

        #region Simplified API Tests - Parameter Validation

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithNullTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard(null, steps, callback));
        }

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithEmptyTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard("", steps, callback));
        }

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithNullSteps_ShouldThrowArgumentNullException()
        {
            // Arrange
            Action<IDialogResult> callback = result => { };
            IEnumerable<WizardStepViewModel> nullSteps = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _wizardService.ShowWizard("Test", nullSteps, callback));
        }

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithEmptySteps_ShouldThrowArgumentException()
        {
            // Arrange
            var emptySteps = new List<WizardStepViewModel>();
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard("Test", emptySteps, callback));
        }

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithNullCallback_ShouldThrowArgumentNullException()
        {
            // Arrange
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _wizardService.ShowWizard("Test", steps, null));
        }

        #endregion

        #region Simplified API Tests - Generic Version

        [Fact]
        public void ShowWizard_GenericAPI_WithValidParameters_ShouldCallDialogService()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel>
            {
                new TestStep("Step 1", "data1"),
                new TestStep("Step 2", "data2")
            };
            Action<IDialogResult> callback = result => { };
            Func<IEnumerable<WizardStepViewModel>, string> dataCollector = stepList =>
                string.Join(",", stepList.Cast<TestStep>().Select(s => s.Data));

            // Act
            _wizardService.ShowWizard<string>(title, steps, callback, dataCollector);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.Is<IDialogParameters>(p => 
                    p.ContainsKey("Steps") && 
                    p.ContainsKey("Title") &&
                    p.ContainsKey("Options") &&
                    p.ContainsKey("DataCollector") &&
                    p.GetValue<string>("Title") == title),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_GenericAPI_WithNullDataCollector_ShouldCallDialogService()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard<string>(title, steps, callback, null);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.IsAny<IDialogParameters>(),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_GenericAPI_WithOptions_ShouldPassOptionsToDialog()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            var options = new WizardOptions { Width = 800, Height = 600 };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard<object>(title, steps, callback, null, options);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.Is<IDialogParameters>(p => 
                    p.ContainsKey("Options") &&
                    ((WizardOptions)p.GetValue<object>("Options")).Width == 800 &&
                    ((WizardOptions)p.GetValue<object>("Options")).Height == 600 &&
                    ((WizardOptions)p.GetValue<object>("Options")).Title == title),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_GenericAPI_WithComplexDataCollector_ShouldWork()
        {
            // Arrange
            var title = "Complex Wizard";
            var steps = new List<WizardStepViewModel>
            {
                new TestStep("Step 1", "value1"),
                new TestStep("Step 2", "value2")
            };
            Action<IDialogResult> callback = result => { };
            Func<IEnumerable<WizardStepViewModel>, Dictionary<string, string>> dataCollector = stepList =>
                stepList.Cast<TestStep>().ToDictionary(s => s.Title, s => s.Data);

            // Act
            _wizardService.ShowWizard(title, steps, callback, dataCollector);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.IsAny<IDialogParameters>(),
                callback), Times.Once);
        }

        #endregion

        #region Simplified API Tests - Non-Generic Version

        [Fact]
        public void ShowWizard_NonGenericAPI_WithValidParameters_ShouldCallDialogService()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard(title, steps, callback);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.Is<IDialogParameters>(p => 
                    p.ContainsKey("Steps") && 
                    p.ContainsKey("Title") &&
                    p.ContainsKey("Options") &&
                    p.ContainsKey("DataCollector")),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_NonGenericAPI_WithOptions_ShouldPassOptionsToDialog()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            var options = new WizardOptions { ShowStepIndicator = false };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard(title, steps, callback, options);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.Is<IDialogParameters>(p => 
                    p.ContainsKey("Options") &&
                    !((WizardOptions)p.GetValue<object>("Options")).ShowStepIndicator &&
                    ((WizardOptions)p.GetValue<object>("Options")).Title == title),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_NonGenericAPI_WithNullOptions_ShouldUseDefaultOptions()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard(title, steps, callback, null);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.Is<IDialogParameters>(p => 
                    p.ContainsKey("Options") &&
                    ((WizardOptions)p.GetValue<object>("Options")).ShowStepIndicator == true), // Default value
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_NonGenericAPI_WithMultipleSteps_ShouldWork()
        {
            // Arrange
            var title = "Multi-Step Wizard";
            var steps = new List<WizardStepViewModel>
            {
                new TestStep("Step 1"),
                new TestStep("Step 2"),
                new TestStep("Step 3"),
                new TestStep("Step 4")
            };
            Action<IDialogResult> callback = result => { };

            // Act
            _wizardService.ShowWizard(title, steps, callback);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.IsAny<IDialogParameters>(),
                callback), Times.Once);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithWhitespaceTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard("   ", steps, callback));
        }

        [Fact]
        public void ShowWizard_SimplifiedAPI_WithStepsContainingNulls_ShouldStillWork()
        {
            // Arrange
            var title = "Test Wizard";
            var steps = new List<WizardStepViewModel>
            {
                new TestStep("Step 1"),
                null,
                new TestStep("Step 2")
            };
            Action<IDialogResult> callback = result => { };

            // Act - Should not throw
            _wizardService.ShowWizard(title, steps, callback);

            // Assert
            _mockDialogService.Verify(x => x.ShowDialog(
                "WizardDialogView",
                It.IsAny<IDialogParameters>(),
                callback), Times.Once);
        }

        [Fact]
        public void ShowWizard_GenericAPI_ParameterValidation_ShouldFollowSameRulesAsNonGeneric()
        {
            // Arrange
            var steps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> callback = result => { };
            Func<IEnumerable<WizardStepViewModel>, string> dataCollector = stepList => "test";

            // Act & Assert - All these should throw the same exceptions as non-generic version
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard<string>(null, steps, callback, dataCollector));
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard<string>("", steps, callback, dataCollector));
            Assert.Throws<ArgumentNullException>(() => _wizardService.ShowWizard<string>("Test", null, callback, dataCollector));
            Assert.Throws<ArgumentException>(() => _wizardService.ShowWizard<string>("Test", new List<WizardStepViewModel>(), callback, dataCollector));
            Assert.Throws<ArgumentNullException>(() => _wizardService.ShowWizard<string>("Test", steps, null, dataCollector));
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void ShowWizard_BothAPIs_ShouldCoexistWithoutConflict()
        {
            // Arrange
            var wizardName = "TraditionalWizard";
            var viewName = "TraditionalView";
            var traditionalParams = new DialogParameters();
            Action<IDialogResult> traditionalCallback = result => { };

            var simplifiedTitle = "Simplified Wizard";
            var simplifiedSteps = new List<WizardStepViewModel> { new TestStep("Step 1") };
            Action<IDialogResult> simplifiedCallback = result => { };

            _wizardService.RegisterWizard(wizardName, viewName);

            // Act - Use both APIs
            _wizardService.ShowWizard(wizardName, traditionalParams, traditionalCallback);
            _wizardService.ShowWizard(simplifiedTitle, simplifiedSteps, simplifiedCallback);

            // Assert - Both should work
            _mockDialogService.Verify(x => x.ShowDialog(viewName, traditionalParams, traditionalCallback), Times.Once);
            _mockDialogService.Verify(x => x.ShowDialog("WizardDialogView", It.IsAny<IDialogParameters>(), simplifiedCallback), Times.Once);
        }

        #endregion
    }
}