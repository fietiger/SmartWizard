using System.Windows;
using System.Windows.Controls;
using SmartWizard.ViewModels;
using Xunit;

namespace SmartWizard.Tests.ViewModels
{
    public class WizardOptionsTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var options = new WizardOptions();

            // Assert
            Assert.Null(options.Title);
            Assert.Null(options.Width);
            Assert.Null(options.Height);
            Assert.True(options.ShowStepIndicator);
            Assert.Equal("下一步", options.NextButtonText);
            Assert.Equal("上一步", options.BackButtonText);
            Assert.Equal("取消", options.CancelButtonText);
            Assert.Equal("完成", options.FinishButtonText);
            Assert.Null(options.WindowStyle);
            Assert.Null(options.CustomStepTemplate);
        }

        [Fact]
        public void Title_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedTitle = "测试向导";

            // Act
            options.Title = expectedTitle;

            // Assert
            Assert.Equal(expectedTitle, options.Title);
        }

        [Fact]
        public void Width_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedWidth = 800.0;

            // Act
            options.Width = expectedWidth;

            // Assert
            Assert.Equal(expectedWidth, options.Width);
        }

        [Fact]
        public void Height_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedHeight = 600.0;

            // Act
            options.Height = expectedHeight;

            // Assert
            Assert.Equal(expectedHeight, options.Height);
        }

        [Fact]
        public void ShowStepIndicator_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();

            // Act
            options.ShowStepIndicator = false;

            // Assert
            Assert.False(options.ShowStepIndicator);
        }

        [Fact]
        public void NextButtonText_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedText = "继续";

            // Act
            options.NextButtonText = expectedText;

            // Assert
            Assert.Equal(expectedText, options.NextButtonText);
        }

        [Fact]
        public void BackButtonText_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedText = "返回";

            // Act
            options.BackButtonText = expectedText;

            // Assert
            Assert.Equal(expectedText, options.BackButtonText);
        }

        [Fact]
        public void CancelButtonText_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedText = "退出";

            // Act
            options.CancelButtonText = expectedText;

            // Assert
            Assert.Equal(expectedText, options.CancelButtonText);
        }

        [Fact]
        public void FinishButtonText_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedText = "结束";

            // Act
            options.FinishButtonText = expectedText;

            // Assert
            Assert.Equal(expectedText, options.FinishButtonText);
        }

        [Fact]
        public void WindowStyle_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedStyle = new Style();

            // Act
            options.WindowStyle = expectedStyle;

            // Assert
            Assert.Equal(expectedStyle, options.WindowStyle);
        }

        [Fact]
        public void CustomStepTemplate_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var options = new WizardOptions();
            var expectedTemplate = new DataTemplate();

            // Act
            options.CustomStepTemplate = expectedTemplate;

            // Assert
            Assert.Equal(expectedTemplate, options.CustomStepTemplate);
        }

        [Fact]
        public void AllProperties_ShouldWorkTogetherInComplexConfiguration()
        {
            // Arrange
            var options = new WizardOptions();
            var title = "复杂配置向导";
            var width = 1000.0;
            var height = 700.0;
            var showStepIndicator = false;
            var nextText = "前进";
            var backText = "后退";
            var cancelText = "关闭";
            var finishText = "保存";
            var style = new Style();
            var template = new DataTemplate();

            // Act
            options.Title = title;
            options.Width = width;
            options.Height = height;
            options.ShowStepIndicator = showStepIndicator;
            options.NextButtonText = nextText;
            options.BackButtonText = backText;
            options.CancelButtonText = cancelText;
            options.FinishButtonText = finishText;
            options.WindowStyle = style;
            options.CustomStepTemplate = template;

            // Assert
            Assert.Equal(title, options.Title);
            Assert.Equal(width, options.Width);
            Assert.Equal(height, options.Height);
            Assert.Equal(showStepIndicator, options.ShowStepIndicator);
            Assert.Equal(nextText, options.NextButtonText);
            Assert.Equal(backText, options.BackButtonText);
            Assert.Equal(cancelText, options.CancelButtonText);
            Assert.Equal(finishText, options.FinishButtonText);
            Assert.Equal(style, options.WindowStyle);
            Assert.Equal(template, options.CustomStepTemplate);
        }

        [Fact]
        public void NullableProperties_ShouldHandleNullValues()
        {
            // Arrange
            var options = new WizardOptions();

            // Act
            options.Width = null;
            options.Height = null;
            options.WindowStyle = null;
            options.CustomStepTemplate = null;
            options.Title = null;

            // Assert
            Assert.Null(options.Width);
            Assert.Null(options.Height);
            Assert.Null(options.WindowStyle);
            Assert.Null(options.CustomStepTemplate);
            Assert.Null(options.Title);
        }

        [Fact]
        public void ButtonTexts_ShouldHandleEmptyStrings()
        {
            // Arrange
            var options = new WizardOptions();

            // Act
            options.NextButtonText = "";
            options.BackButtonText = "";
            options.CancelButtonText = "";
            options.FinishButtonText = "";

            // Assert
            Assert.Equal("", options.NextButtonText);
            Assert.Equal("", options.BackButtonText);
            Assert.Equal("", options.CancelButtonText);
            Assert.Equal("", options.FinishButtonText);
        }

        [Fact]
        public void Dimensions_ShouldHandleZeroValues()
        {
            // Arrange
            var options = new WizardOptions();

            // Act
            options.Width = 0.0;
            options.Height = 0.0;

            // Assert
            Assert.Equal(0.0, options.Width);
            Assert.Equal(0.0, options.Height);
        }

        [Fact]
        public void Dimensions_ShouldHandleNegativeValues()
        {
            // Arrange
            var options = new WizardOptions();

            // Act
            options.Width = -100.0;
            options.Height = -50.0;

            // Assert
            Assert.Equal(-100.0, options.Width);
            Assert.Equal(-50.0, options.Height);
        }
    }
}