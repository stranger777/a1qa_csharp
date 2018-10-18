using System.Threading;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement;
using A1QA.Core.Csharp.White.UIElement.QAItems;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A1QA.Core.Csharp.White.UnitTests
{
    [TestClass]
    public sealed class Test
    {
        private readonly string testAppUrl = @"C:\Windows\System32\mspaint.exe";

        [TestInitialize]
        public void Initialize()
        {
            WorkSpace.Application = QAApplication.Launch(@"C:\Program Files\ONLYOFFICE\DesktopEditors\DesktopEditors.exe");
            Thread.Sleep(15000);
            WorkSpace.MainWindow = WorkSpace.Application.MainWindow;
        }

        [TestMethod]
        public void GetElement()
        {
            var button = QAButton.Get(@"//button[contains(@Name,'FILE')]", "Button FILE");
            QAAssert.IsTrue(button.Exists);
        }

        [TestCleanup]
        public void CleanUp()
        {
            WorkSpace.Application.Close();
            WorkSpace.MainWindow = null;
        }
    }
}