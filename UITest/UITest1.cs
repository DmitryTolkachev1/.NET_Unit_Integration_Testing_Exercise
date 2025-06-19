using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace UITest
{
    public class UITest1
    {
        private const string AppPath = @"C:\Assessment\Improvement Plan\TestProject1\.NET_Unit_Integration_Testing_Exercise\WinFormsApp\bin\Debug\net8.0-windows\WinFormsApp.exe";
        private Application _app;
        private AutomationBase _automation;

        public UITest1()
        {
            _app = Application.Launch(Path.GetFullPath(AppPath));
            _automation = new UIA3Automation();
        }

        [Fact]
        public void Test1()
        {
            var expected = "Hello, Test User!";
            var window = _app.GetMainWindow(_automation);

            var input = window.FindFirstDescendant(cf => cf.ByAutomationId("NameInput")).AsTextBox();
            var button = window.FindFirstDescendant(cf => cf.ByAutomationId("GreetButton")).AsButton();
            var result = window.FindFirstDescendant(cf => cf.ByAutomationId("ResultLabel")).AsLabel();

            input.Enter("Test User");
            button.Invoke();

            Thread.Sleep(300);

            Assert.Equal(expected, result.Text);
        }
    }
}