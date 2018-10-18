//  <copyright file="QATableCell.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Threading;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAListBoxItems;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TableItems;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATableItems
{
    /// <summary>
    ///     Our own implementation of a TableCell that extends the functionality of
    ///     White TableCell
    /// </summary>
    public class QATableCell : QAItem<TableCell>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATableCell" /> class
        /// </summary>
        /// <param name="tableCell">White ListViewCell</param>
        /// <param name="friendlyName">Friendly name for DataGridCell</param>
        public QATableCell(TableCell tableCell, string friendlyName) : base(tableCell, friendlyName)
        {
        }

        /// <summary>
        ///     Get a TableCell based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TableCell</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TableCell</returns>
        public static QATableCell Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tableCell = FindUIItem(searchCriteria, scope, timeout);
            return new QATableCell(tableCell, friendlyName);
        }

        public static QATableCell Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new TableCell(matchingAE, new NullActionListener())
                : null;

            return new QATableCell(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Set value of a TableCell and then verify
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="shouldClick">Should click before setting text (yes/no)</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void SetValue(string value, bool shouldClick = true, bool shouldVerify = true)
        {
            ReportActionValue("SetValue", value);
            if (shouldClick)
            {
                UIItem.Click();
            }

            WorkSpace.MainWindow.Keyboard.Enter(value);
            WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(100);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyDataGridCellSetMsg, value);

                // Decimal places are often affixed to the end of the text
                // so ensure expected text is a subset of actual text
                QAAssert.Contains(Value, value, friendlyMessage);
            }
        }

        /// <summary>
        ///     Set value of a popup from a TableCell and then verify
        /// </summary>
        /// <param name="value">Value to click</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void SetPopupValue(string value, bool shouldVerify = true)
        {
            ReportActionValue("SetPopupValue", value);
            UIItem.Click();
            UIItem.Click();

            var popup = QAWindow.GetModalWindow(SearchCriteria.ByAutomationId(string.Empty).AndByClassName("Popup"), WorkSpace.MainWindow.Window);

            var listItemPopup = QAListItem.Get(SearchCriteria.ByText(value), string.Empty, popup.Window);

            listItemPopup.Click();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyDataGridCellSetMsg, value);
                QAAssert.AreEqual(Value, value, friendlyMessage);
            }
        }
    }
}