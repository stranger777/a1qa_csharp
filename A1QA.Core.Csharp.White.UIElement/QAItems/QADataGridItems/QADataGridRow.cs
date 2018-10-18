//  <copyright file="QADataGridRow.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QADataGridItems
{
    /// <summary>
    ///     Our own implementation of a DataGridRow that extends the functionality of
    ///     White ListViewRow
    /// </summary>
    public class QADataGridRow : QAItem<ListViewRow>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QADataGridRow" /> class
        /// </summary>
        /// <param name="listViewRow">White ListViewRow</param>
        /// <param name="friendlyName">Friendly name for DataGridRow</param>
        public QADataGridRow(ListViewRow listViewRow, string friendlyName)
            : base(listViewRow, friendlyName)
        {
        }

        /// <summary>
        ///     Returns all values from row cells as list of strings
        /// </summary>
        /// <param name="parent">Data grid Item or Item you need to scroll right to get all cells. Parent of the row by default.</param>
        /// <returns></returns>
        public List<string> GetTextFromAllCells(UIItem parent = null)
        {
            var rownData = new List<string>();

            if (parent == null)
            {
                parent = QAWatDataGrid.GetParentUIItem(UIItem);
                if (parent.ScrollBars.Horizontal.IsScrollable)
                {
                    QAWatDataGrid.GetParentUIItem(UIItem).ScrollBars.Horizontal.SetToMaximum();
                }
            }

            var cells = UIItem.Cells;

            for (var i = 0; i < cells.Count; i++)
            {
                if (cells[i].ValueOfEquals(AutomationElement.ControlTypeProperty, ControlType.Text))
                {
                    var text = cells[i].Text;
                    rownData.Add(text);
                }
            }
            return rownData;
        }

        /// <summary>
        ///     Get a DataGridRow based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for DataGridRow</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DataGridRow</returns>
        public static QADataGridRow Get(
            SearchCriteria searchCriteria,
            string friendlyName,
            UIItem scope = null,
            int timeout = 0)
        {
            var dataGridRow = FindUIItem(searchCriteria, scope, timeout);
            return new QADataGridRow(dataGridRow, friendlyName);
        }

        /// <summary>
        ///     Select this row
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Select(bool shouldVerify = true)
        {
            ReportAction("Select");
            UIItem.Select();
            if (!UIItem.IsSelected)
            {
                RightClick();
                Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);
                Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.UP);
            }
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyDataGridRowSelectMsg);
                QAAssert.AreEqual(true, UIItem.IsSelected, friendlyMessage);
            }
        }

        /// <summary>
        ///     Set value and then verify
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
                var friendlyMessage = ConstructFriendlyMessage(
                    Resources.FriendlyDataGridCellSetMsg,
                    value);

                // Decimal places are often affixed to the end of the text
                // so ensure expected text is a subset of actual text
                QAAssert.Contains(Value, value, friendlyMessage);
            }
        }

        /// <summary>
        ///     Get value
        /// </summary>
        public IComparable GetValue()
        {
            var value = Value;
            return value != null;
        }

        /// <summary>
        ///     Get the value of the current control
        /// </summary>
        public string GetCurrentValue()
        {
            var currentValue = Value;
            return currentValue;
        }

        /// <summary>
        ///     Get the Foreground attribute from TextPattern
        /// </summary>
        public string GetForegroundColor()
        {
            var value = string.Empty;
            var isAvailable = (bool) UIItem.AutomationElement.GetCurrentPropertyValue(
                AutomationElement.IsTextPatternAvailableProperty);
            if (isAvailable)
            {
                var valuePattern = (TextPattern) UIItem.AutomationElement.GetCurrentPattern(
                    TextPattern.Pattern);
                var foregroundColor =
                    valuePattern.DocumentRange.GetAttributeValue(TextPatternIdentifiers.ForegroundColorAttribute);
                value = foregroundColor.ToString();
            }
            return value;
        }

        public bool IsReadOnly()
        {
            var valuePattern = (ValuePattern) UIItem.AutomationElement.GetCurrentPattern(ValuePattern.Pattern);
            return valuePattern.Current.IsReadOnly;
        }

        /// <summary>
        ///     Get a parent Row of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <returns>Parent Button</returns>
        public static QADataGridRow GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QADataGridRow(parent, friendlyName);
        }

        public string GetValueFromColumn(string columnName, QAWatDataGrid gridWithScroll = null)
        {
            var cell = GetCell(columnName, gridWithScroll);
            return cell.Exists
                ? cell.Value
                : "No such Cell";
        }

        public QAPanel GetCell(string columnName, QAWatDataGrid gridWithScroll = null)
        {
            var panel = QAPanel.Get(SearchCriteria.ByText(columnName), columnName, UIItem, 1);
            if (gridWithScroll != null && panel.Exists)
            {
                gridWithScroll.ScrollToCellHorizontal(panel);
            }
            return panel;
        }
    }
}