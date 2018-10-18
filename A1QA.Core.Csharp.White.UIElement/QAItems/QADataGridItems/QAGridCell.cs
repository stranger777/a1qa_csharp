//  <copyright file="QADataGridCell.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Threading;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QADataGridItems
{
    public class QAGridCell : QAItem<ListViewCell>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAGridCell" /> class
        /// </summary>
        /// <param name="gridCell">White ListViewCell</param>
        /// <param name="friendlyName">Friendly name for DataGridCell</param>
        public QAGridCell(ListViewCell gridCell, string friendlyName)
            : base(gridCell, friendlyName)
        {
        }

        /// <summary>
        ///     Get a GridCell based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for DataGridCell</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DataGridCell</returns>
        public static QAGridCell Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var dataGridCell = FindUIItem(searchCriteria, scope, timeout);
            return new QAGridCell(dataGridCell, friendlyName);
        }

        /// <summary>
        ///     Set value of a DataGridCell and then verify
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="shouldClick">Should click before setting text (yes/no)</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        /// <param name="window">Window to perform text entry on</param>
        public void SetValue(string value, bool shouldClick = true, bool shouldVerify = true, Window window = null)
        {
            ReportActionValue("SetValue", value);

            if (window == null)
            {
                window = WorkSpace.MainWindow.Window;
            }

            if (shouldClick)
            {
                UIItem.Click();
            }

            window.Keyboard.Enter(value);
            window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);

            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyDataGridCellSetMsg,value);

                QAAssert.Contains(Value, value, friendlyMessage);
            }
        }
    }
}