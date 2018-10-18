//  <copyright file="QAWatDataGrid.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.Basics.SystemUtilities;
using A1QA.Core.Csharp.White.UIElement.Extentions;
using A1QA.Core.Csharp.White.UIElement.Properties;
using A1QA.Core.Csharp.White.UIElement.QAItems.QACustomItems;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QADataGridItems
{
    /// <summary>
    ///     Our own implementation of a DataGrid that extends the functionality of
    ///     White ListView
    /// </summary>
    public class QAWatDataGrid : QAItem<ListView>
    {
        private bool hasFoundMismatch;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QAWatDataGrid" /> class
        /// </summary>
        /// <param name="dataGrid">White ListView</param>
        /// <param name="friendlyName">Friendly name for DataGrid</param>
        public QAWatDataGrid(ListView dataGrid, string friendlyName)
            : base(dataGrid, friendlyName)
        {
        }

        /// <summary>
        ///     This is the button in the top left corner of the grid that can be clicked to select all rows
        /// </summary>
        public QAButton SelectAllButton
        {
            get
            {
                var button = QAButton.Get(SearchCriteria.ByAutomationId("selectAllButton"), "SelectAllButton");
                return button;
            }
        }

        public QAWindow CustomParentWindow { get; set; } = null;

        /// <summary>
        ///     Gets total number of visible rows
        /// </summary>
        public int VisibleRowCount => UIItem.Rows.Count;

        /// <summary>
        ///     Gets total number of rows
        /// </summary>
        public int RowCount
        {
            get
            {
                var gridPattern = (GridPattern) UIItem.AutomationElement.GetCurrentPattern(GridPattern.Pattern);
                return gridPattern.Current.RowCount;
            }
        }

        /// <summary>
        ///     Gets the total number of selected rows
        /// </summary>
        public int SelectedRowCount => UIItem.SelectedRows.Count;

        /// <summary>
        ///     Gets total number of available columns
        /// </summary>
        public int ColumnCount => UIItem.Header.Columns.Count;

        /// <summary>
        ///     Gets a value indicating whether a Data Grid is enabled
        /// </summary>
        public bool Enabled => UIItem.Enabled;

        /// <summary>
        ///     Get a DataGrid based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for DataGrid</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DataGrid</returns>
        public static QAWatDataGrid Get(
            SearchCriteria searchCriteria,
            string friendlyName,
            UIItem scope = null,
            int timeout = 0)
        {
            var dataGrid = FindUIItem(searchCriteria, scope, timeout);
            return new QAWatDataGrid(dataGrid, friendlyName);
        }

        public static QAWatDataGrid Get(
            Condition condition,
            TreeScope treeScope,
            string friendlyName,
            UIItem scope = null,
            int timeout = 0)
        {
            var completedCondition =
                new AndCondition(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "datagrid"),
                    condition);
            var dataGridAE = FindUIItem(completedCondition, treeScope, scope, timeout);
            var matchingUIItem = dataGridAE != null
                ? new ListView(dataGridAE, new NullActionListener())
                : null;

            return new QAWatDataGrid(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Find cell in data grid
        /// </summary>
        /// <param name="columnName">Name of column</param>
        /// <param name="rowNumber">Number of row</param>
        /// <returns>Cell at given column name and row number</returns>
        public QAGridCell Cell(string columnName, int rowNumber)
        {
            return new QAGridCell(
                UIItem.Cell(columnName, rowNumber),
                string.Format("Column: {0}, Row: {1}", columnName, rowNumber));
        }

        /// <summary>
        ///     Find row in data grid
        /// </summary>
        /// <param name="rowNumber">Number of row</param>
        /// <returns>Row at given row number</returns>
        public QADataGridRow GetRow(int rowNumber)
        {
            var rows = UIItem.Rows;
            while (rowNumber >= rows.Count)
            {
                rowNumber = rowNumber - rows.Count;
                UIItem.ScrollBars.Vertical.ScrollDownLarge();
                rows = UIItem.Rows;
            }
            return new QADataGridRow(
                rows[rowNumber],
                rowNumber.ToString());
        }

        public QADataGridRow GetRow(string columnName, string value)
        {
            ScrollUpWithHotKeys();
            QADataGridRow row = null;
            while (row == null || !row.Exists)
            {
                var rows = UIItem.Rows;
                foreach (var listViewRow in rows)
                {
                    row = new QADataGridRow(listViewRow, "row").GetValueFromColumn(columnName) == value
                        ? new QADataGridRow(listViewRow, "row")
                        : new QADataGridRow(null, "no such row");
                    if (row.Exists)
                    {
                        return row;
                    }
                    if (listViewRow.Bounds.Bottom > Bounds.Bottom + 10)
                    {
                        break;
                    }
                }
                if (UIItem.ScrollBars.Vertical.IsMaximal())
                {
                    break;
                }
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEDOWN);
            }

            return row;
        }

        public QADataGridRow GetRow(Dictionary<string, string> columnAndValue)
        {
            var trueRow = false;
            var firstValues = columnAndValue.FirstOrDefault();
            var rows = GetRows(firstValues.Key, firstValues.Value);
            foreach (var row in rows)
            {
                IDictionaryEnumerator enumerator = columnAndValue.GetEnumerator();
                enumerator.MoveNext();
                while (enumerator.MoveNext())
                {
                    var actualValue = row.GetValueFromColumn(enumerator.Key.ToString());
                    if (actualValue != enumerator.Value.ToString())
                    {
                        trueRow = false;
                        break;
                    }
                    trueRow = true;
                }
                if (trueRow)
                {
                    return row;
                }
            }
            return new QADataGridRow(null, "no such row");
        }

        public List<QADataGridRow> GetRows(string columnName, string value)
        {
            var rows = new List<QADataGridRow>();
            var allRows = UIItem.Rows;
            foreach (var rowlist in allRows)
            {
                var row = new QADataGridRow(rowlist, string.Empty);
                if (columnName == null && value == null)
                {
                    rows.Add(row);
                }
                else if (row.GetValueFromColumn(columnName) == value)
                {
                    rows.Add(row);
                }
            }

            return rows;
        }

        public List<QADataGridRow> GetRows()
        {
            return GetRows(null, null);
        }

        /// <summary>
        ///     Find column header in data grid
        /// </summary>
        /// <param name="columnNumber">Number of column</param>
        /// <returns>Column header at given column number</returns>
        public QADataGridColumn GetColumnHeader(int columnNumber)
        {
            return new QADataGridColumn(
                UIItem.Header.Columns[columnNumber],
                columnNumber.ToString());
        }

        public QADataGridColumn GetColumnHeader(string columnName)
        {
            return new QADataGridColumn(
                UIItem.Header.Columns.FirstOrDefault(x => x.Name == columnName),
                columnName);
        }

        /// <summary>
        ///     get column name in data grid
        /// </summary>
        /// <param name="columnNumber">Number of column</param>
        /// <returns>Column name at given column number</returns>
        public string GetColumnName(int columnNumber)
        {
            var columnHeader = GetColumnHeader(columnNumber);
            return columnHeader.UIItem.Name;
        }

        public void SelectFirstRow()
        {
            UIItem.Focus();
            Desktop.Instance.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEUP);
            Desktop.Instance.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        public void ScrollUpWithHotKeys()
        {
            UIItem.Focus();
            Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEDOWN);
            Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEUP);
            while (UIItem.ScrollBars.Vertical.IsNotMinimum)
            {
                Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEUP);
            }
        }

        public bool SelectDataGridRowWithHotkeys(string columnName, string textInColumn)
        {
            return GetRowWithHotkeys(columnName, textInColumn).Exists;
        }

        public QADataGridRow GetRowWithHotkeys(string columnName, string textInColumn)
        {
            if (QAWait.Until(() => RowCount > 0, 5000))
            {
                if (!UIItem.ScrollBars.Vertical.IsScrollable)
                {
                    GetRow(0).Click();
                    if (!GetSelectedRow().Exists)
                    {
                        Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                        Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                        SelectFirstRow();
                    }
                }
                else
                {
                    ScrollUpWithHotKeys();
                    SelectFirstRow();
                }
                while (!UIItem.ScrollBars.Vertical.IsMaximal())
                {
                    var row = GetSelectedRow();
                    if (row.GetValueFromColumn(columnName) == textInColumn)
                    {
                        return row;
                    }
                    Desktop.Instance.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                }
                return new QADataGridRow(null, "Row from grid");
            }
            return new QADataGridRow(null, "Row from grid");
        }

        /// <summary>
        ///     Sets UI to a specific row
        ///     <param name="someTextInRow">Some text, which contained in row</param>
        ///     <returns>true if row found</returns>
        /// </summary>
        public bool SelectDataGridRow(string textInRow)
        {
            QAWait.Until(() => RowCount > 0, 5000);
            for (var rowCount = 0; rowCount < RowCount; rowCount++)
            {
                var row = GetRow(rowCount);
                var textForRow = row.GetTextFromAllCells();
                if (textForRow.Contains(textInRow))
                {
                    row.Click();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Sets UI to a specific row
        ///     <param name="someTextInRow">Some text, which contained in row</param>
        ///     <returns>true if row found</returns>
        /// </summary>
        public bool SelectDataGridRows(string textInRow1, string textInRow2)
        {
            QAWait.Until(() => RowCount > 0, 5000);
            for (var rowCount = 0; rowCount < RowCount; rowCount++)
            {
                var row = GetRow(rowCount);
                var textForRow = row.GetTextFromAllCells();
                if (textForRow.Contains(textInRow1) && textForRow.Contains(textInRow2))
                {
                    row.Click();
                    row.RightClick();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Sets UI to a specific row
        ///     <param name="dataGrid">UI data grid</param>
        ///     <returns>true if row found</returns>
        /// </summary>
        public bool SelectDataGridRow(int zeroBasedTargetRow)
        {
            var parent = QAUIItem.GetParent(UIItem, "parent");
            var container = QAContainer.Get(SearchCriteria.ByAutomationId(parent.UIItem.Id), "container");

            var gotoRowTextBox = QATextBox.Get(SearchCriteria.ByAutomationId("dataGridGoToRow"), "(Hidden) GoToRow",
                container.UIItem);

            if (gotoRowTextBox != null && gotoRowTextBox.Exists)
            {
                gotoRowTextBox.UIItem.SetValue(string.Empty);
                Thread.Sleep(1000);
                gotoRowTextBox.UIItem.SetValue(zeroBasedTargetRow.ToString());
                return true;
            }
            Report.Output("FAILURE: Row not found");
            return false;
        }

        /// <summary>
        ///     Sets UI to a specific row
        ///     <param name="dataGrid">UI data grid</param>
        ///     <returns>true if row found</returns>
        /// </summary>
        public bool SelectMultipleDataGridRows(List<int> zeroBasedTargetRows)
        {
            var parent = QAUIItem.GetParent(UIItem, "parent");
            var container = QAContainer.Get(SearchCriteria.ByAutomationId(parent.UIItem.Id), "container");
            var gotoRowTextBox = QATextBox.Get(SearchCriteria.ByAutomationId("dataGridGoToRow"), "(Hidden) GoToRow",
                container.UIItem);

            if (gotoRowTextBox != null && gotoRowTextBox.Exists)
            {
                gotoRowTextBox.UIItem.SetValue(string.Empty);
                Thread.Sleep(1000);
                gotoRowTextBox.UIItem.SetValue(string.Join(",", zeroBasedTargetRows));
                return true;
            }
            Report.Output("FAILURE: Row not found");
            return false;
        }

        /// <summary>
        ///     Selects all rows
        ///     <param name="dataGrid">UI data grid</param>
        ///     <returns>true if row selection sucessfull</returns>
        /// </summary>
        public bool SelectDataGridAllRows(bool selectAll)
        {
            var parent = QAUIItem.GetParent(UIItem, "parent");
            var container = QAContainer.Get(SearchCriteria.ByAutomationId(parent.UIItem.Id), "container");

            var gotoRowTextBox = QATextBox.Get(SearchCriteria.ByAutomationId("dataGridGoToRow"), "(Hidden) GoToRow",
                container.UIItem);

            if (gotoRowTextBox != null && gotoRowTextBox.Exists)
            {
                gotoRowTextBox.UIItem.SetValue(string.Empty);
                Thread.Sleep(1000);
                if (selectAll)
                {
                    gotoRowTextBox.UIItem.SetValue("all");
                }
                else
                {
                    gotoRowTextBox.UIItem.SetValue("none");
                }
                return true;
            }
            if (selectAll)
            {
                Report.Output("FAILURE: Failed to select all rows");
            }
            else
            {
                Report.Output("FAILURE: Failed to unselect all rows");
            }
            return false;
        }

        /// <summary>
        ///     This is to click a row, this will also select the row
        /// </summary>
        /// <param name="zeroBasedIndex"></param>
        private Point GetRowLocationPoint(int zeroBasedIndex)
        {
            var searchCriteria = SearchCriteria.ByAutomationId(string.Format("Row_{0}", zeroBasedIndex));
            var row = QADataGridRow.Get(searchCriteria, string.Empty, UIItem, 5000);
            var firstCell = QAUIItem.Get(SearchCriteria.Indexed(0), string.Empty, row.UIItem, 5000);
            var location = firstCell.Location;
            location.X -= 2;
            location.Y += 2;

            return location;
        }

        private Point GetRowMiddleLocationPoint(int zeroBasedIndex)
        {
            var location = UIItem.Rows.Get(zeroBasedIndex).ClickablePoint;
            location.X += 2;
            location.Y += 2;

            return location;
        }

        private void CopyAllToClipboard()
        {
            Click();
            SelectAllRowsUsingKeyboardShortcut();
            WorkSpace.MainWindow.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            WorkSpace.MainWindow.Keyboard.Enter("C");
            WorkSpace.MainWindow.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        /// <summary>
        ///     Select all rows in data grid using CTRL + A keyboard shortcut
        /// </summary>
        /// <param name="window">Associated window</param>
        public void SelectAllRowsUsingKeyboardShortcut(QAWindow window = null)
        {
            if (window == null)
            {
                window = WorkSpace.MainWindow;
            }
            Click();
            window.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            window.Keyboard.Enter("A");
            window.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        /// <summary>
        ///     Find index of a row in data grid; For use when FindIndex does not work properly.
        ///     Specifically, this is used when Ctrl+C does not copy the table contents to the clipboard,
        ///     such as the Roles and Users tables in UNIFI Security.
        ///     This function may fail to find the row if the row is too far offscreen, so a different solution should be found.
        /// </summary>
        /// <param name="columnName">the column name</param>
        /// <param name="valueText">the value in the column</param>
        /// <returns>Index of specified row if it exists, -1 otherwise</returns>
        public int FindRowIndex(string columnName, string valueText)
        {
            int rowIndex, numberOfRows = RowCount;
            for (rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                var itemCell = Cell(columnName, rowIndex);
                if (itemCell.Value.Equals(valueText))
                {
                    break;
                }
            }

            if (rowIndex < numberOfRows)
            {
                return rowIndex;
            }
            return -1;
        }

        /// <summary>
        ///     Find index of the selected row in data grid; For use when you do not have column name and cell value
        /// </summary>
        /// <returns>Index of specified row if it exists, -1 otherwise</returns>
        [Obsolete("FindSelectedRowIndex is deprecated, please use FindSelectedRowIndexes instead.")]
        public int FindSelectedRowIndex()
        {
            var rowIndex = -1;
            ListViewRow selectedRow = null;
            if (UIItem.SelectedRows.Count > 0)
            {
                selectedRow = UIItem.SelectedRows[0];
                rowIndex = UIItem.Rows.IndexOf(selectedRow);
            }

            return rowIndex;
        }

        /// <summary>
        ///     Find indexes of the selected rows in data grid
        /// </summary>
        /// <returns>Index of specified row if it exists, -1 otherwise</returns>
        public List<int> FindSelectedRowIndexes()
        {
            var rowIndexes = new List<int>();
            if (UIItem.SelectedRows.Count > 0)
            {
                foreach (var selectedRow in UIItem.SelectedRows)
                {
                    rowIndexes.Add(UIItem.Rows.IndexOf(selectedRow));
                }
            }

            return rowIndexes;
        }

        public List<QADataGridRow> GetSelectedRows()
        {
            var rows = new List<QADataGridRow>();
            if (UIItem.SelectedRows.Count > 0)
            {
                foreach (var selectedRow in UIItem.SelectedRows)
                {
                    rows.Add(new QADataGridRow(selectedRow, "row"));
                }
            }

            return rows;
        }

        public QADataGridRow GetSelectedRow()
        {
            var selectedRowAE = UIItem.AutomationElement.FindFirst(TreeScope.Children,
                new PropertyCondition(SelectionItemPattern.IsSelectedProperty, true));
            Log.ReportInfo($"There are at least one selected row: {selectedRowAE != null}");
            return selectedRowAE == null
                ? new QADataGridRow(null, "row")
                : new QADataGridRow(new ListViewRow(selectedRowAE, new NullActionListener()), "row");
        }

        /// <summary>
        ///     Find index of a column in the data grid
        /// </summary>
        /// <param name="valueText">the value in the column</param>
        /// <returns>Index of specified column if it exists, -1 otherwise</returns>
        public int FindColumnIndex(string valueText)
        {
            var columnIndex = UIItem.Header.Columns.FindIndex(c => c.Text == valueText);

            if (columnIndex >= 0)
            {
                return columnIndex;
            }
            columnIndex =
                UIItem.Header.Columns.FindIndex(
                    c =>
                        c.AutomationElement.FindFirst(TreeScope.Children,
                            new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Text)).Current.Name ==
                        valueText);
            if (columnIndex >= 0)
            {
                return columnIndex;
            }
            return -1;
        }

        /// <summary>
        ///     Returns a list of values from certain columns
        /// </summary>
        /// <param name="columNames">the columns name</param>
        /// <returns>the list of values</returns>
        public List<List<string>> GetTableResultsColumns(string[] columNames)
        {
            var rowsValues = new List<List<string>>();

            for (var j = 0; j < RowCount; j++)
            {
                rowsValues.Add(GetLineResult(j, columNames));
            }

            return rowsValues;
        }

        /// <summary>
        ///     Compare two sets of values from a table as List<List<string>>
        /// </summary>
        /// <param name="expected">expected values</param>
        /// <param name="actual">actual values</param>
        public void CompareResults(List<List<string>> expected, List<List<string>> actual)
        {
            CompareTableData(expected, actual);
        }

        /// <summary>
        ///     Scroll down into a data grid
        /// </summary>
        public void ScrollDown()
        {
            UIItem.ScrollBars.Vertical.SetToMinimum();
            var position = UIItem.ScrollBars.Vertical.MinimumValue;

            while (position < UIItem.ScrollBars.Vertical.MaximumValue)
            {
                UIItem.ScrollBars.Vertical.ScrollDown();
                position++;
            }
        }

        public void ScrollUp()
        {
            UIItem.Focus();
            while (UIItem.ScrollBars.Vertical.IsNotMinimum)
            {
                UIItem.ScrollBars.Vertical.SetToMinimum();
            }
        }

        /// <summary>
        ///     Scroll right into a data grid
        /// </summary>
        public void ScrollRight()
        {
            UIItem.ScrollBars.Horizontal.SetToMinimum();
            var position = UIItem.ScrollBars.Horizontal.MinimumValue;

            while (position < UIItem.ScrollBars.Horizontal.MaximumValue)
            {
                UIItem.ScrollBars.Horizontal.ScrollRight();
                position++;
            }
        }

        public void ScrollToCellHorizontal(QAPanel cell)
        {
            var items = cell.UIItem.Items;
            if (items.Count == 0)
            {
                ScrollRight();
            }
        }

        /// <summary>
        ///     Converts the values of the DataGrid to a CSV string
        /// </summary>
        /// <param name="hasIndexCell">Set to true if each row has index cell at beginning</param>
        /// <returns>CSV string</returns>
        public string ToCSV(bool hasIndexCell = false)
        {
            var rowsCSV = string.Empty;
            foreach (var row in UIItem.Rows)
            {
                var index = 0;
                foreach (var cell in row.Cells)
                {
                    // Skip index cell in DataGrid
                    if (index == 1 && hasIndexCell)
                    {
                        index++;
                        continue;
                    }

                    // Column headers have an even index
                    // Values have an odd index
                    //if (IntegerHelper.IsOdd(index))
                    //{
                    //    var value = cell.Text.Replace(",", "[comma]");
                    //    if (firstIteration)
                    //    {
                    //        rowsCSV = string.Format("{0}{1}", rowsCSV, value);
                    //        firstIteration = false;
                    //    }
                    //    else
                    //    {
                    //        rowsCSV = string.Format("{0},{1}", rowsCSV, value);
                    //    }
                    //}

                    index++;
                }

                rowsCSV = string.Format("{0}{1}", rowsCSV, Environment.NewLine);
            }

            return rowsCSV;
        }

        public List<string> GetTextFromColumn(string columnName)
        {
            ScrollUpWithHotKeys();
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.PAGEDOWN);
            var columnData = new List<string>();
            while (columnData.Count < RowCount)
            {
                var rows = UIItem.Rows;
                foreach (var listViewRow in rows)
                {
                    var row = new QADataGridRow(listViewRow, "row");
                    if (row.Exists)
                    {
                        columnData.Add(row.GetValueFromColumn(columnName));
                    }
                    if (listViewRow.Bounds.Bottom > Bounds.Bottom + 10)
                    {
                        break;
                    }
                }
                if (UIItem.ScrollBars.Vertical.IsMaximal())
                {
                    break;
                }
                for (var i = 0; i < rows.Count; i++)
                {
                    Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DOWN);
                }
            }
            return columnData;
        }

        /// <summary>
        ///     Return all of the data as a List of strings for this rowIndex
        /// </summary>
        /// <param name="rowIndex"> row index</param>
        /// <returns></returns>
        public List<string> GetTextForRow(int rowIndex)
        {
            var rownData = new List<string>();

            var cells = UIItem.Rows.Get(rowIndex).Cells;
            var cellText = string.Empty;

            for (var i = 0; i < cells.Count; i++)
            {
                if (
                    UIItem.Rows.Get(rowIndex).Cells[i].ValueOfEquals(AutomationElement.ControlTypeProperty,
                        ControlType.Text) &&
                    UIItem.Rows.Get(rowIndex).Cells[i].ValueOfEquals(AutomationElement.IsOffscreenProperty, false))
                {
                    var text = UIItem.Rows.Get(rowIndex).Cells[i].Text;
                    rownData.Add(text);
                }
            }
            return rownData;
        }

        /// <summary>
        ///     Find index of the row with the specified value in data grid;
        /// </summary>
        /// <returns>Index of specified row if it exists, -1 otherwise</returns>
        public int FindRowIndex(string cellValue)
        {
            int rowIndex, numberOfRows = RowCount;
            for (rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                var itemCell = UIItem.Rows.Get(rowIndex);
                for (var cellItems = 0; cellItems < itemCell.Cells.Count; cellItems++)
                {
                    if (itemCell.Cells[cellItems].Name == cellValue)
                    {
                        return rowIndex;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        ///     Select all rows in data grid
        /// </summary>
        private void SelectAllRows()
        {
            var isFirstRow = true;

            foreach (var row in UIItem.Rows)
            {
                if (isFirstRow)
                {
                    row.Select();
                    isFirstRow = false;
                }
                else
                {
                    row.MultiSelect();
                }
            }
        }

        /// <summary>
        ///     Unselects previously selected row and selects this row.
        ///     If this row is already selected it doesn't have any effect.
        /// </summary>
        /// <param name="zeroBasedRowIndex">Index of row to be selected</param>
        private void Select(int zeroBasedRowIndex)
        {
            UIItem.Select(zeroBasedRowIndex);
        }

        /// <summary>
        ///     Copies data to the clipboard from the grid using data grid instead of keyboard shortcut CTRL-C
        /// </summary>
        /// <returns>bool to indicate whether grid based copy was possible</returns>
        private bool CopyGridToClipboardAsCSV(Window window)
        {
            var parent = QAUIItem.GetParent(UIItem, "parent");
            var container = QAContainer.Get(SearchCriteria.ByAutomationId(parent.UIItem.Id), "container");

            var copyToClipboard = QATextBox.Get(SearchCriteria.ByAutomationId("dataGridCopyToClipboardCSV"),
                "copyToClipboard", container.UIItem);

            if (copyToClipboard.Exists)
            {
                copyToClipboard.SetValue("1", false);
                return true;
            }
            Report.Output("FAILURE: unable to copy to clipboard");
            return false;
        }

        /// <summary>
        ///     Compare values from two tables that are expressed as a list
        /// </summary>
        /// <param name="expectedTableData">Expected values</param>
        /// <param name="actualTableData">Actual values</param>
        private void CompareTableData(
            List<List<string>> expectedTableData,
            List<List<string>> actualTableData)
        {
            hasFoundMismatch = false;
            for (var i = 0; i < actualTableData.Count; i++)
            {
                CompareRowData(expectedTableData, actualTableData, i);
            }

            var friendlyMessage = string.Format(Resources.FriendlyDataGridComparePassMsg, FriendlyName);
            QAAssert.IsFalse(hasFoundMismatch, friendlyMessage, true);
        }

        /// <summary>
        ///     Compare values from two rows
        /// </summary>
        /// <param name="expectedTableData">Expected values</param>
        /// <param name="actualTableData">Actual values</param>
        /// <param name="rowNumber">Row number</param>
        private void CompareRowData(
            List<List<string>> expectedTableData,
            List<List<string>> actualTableData,
            int rowNumber)
        {
            for (var i = 0; i < actualTableData[rowNumber].Count; i++)
            {
                CompareCellData(
                    expectedTableData,
                    actualTableData,
                    rowNumber,
                    i);
            }
        }

        /// <summary>
        ///     Compare values from two cells
        /// </summary>
        /// <param name="expectedTableData">Expected values</param>
        /// <param name="actualTableData">Actual values</param>
        /// <param name="rowNumber">Row number of cell</param>
        /// <param name="columnNumber">Column number of cell</param>
        private void CompareCellData(
            List<List<string>> expectedTableData,
            List<List<string>> actualTableData,
            int rowNumber,
            int columnNumber)
        {
            var actualValue = actualTableData[rowNumber][columnNumber].Replace("[comma]", ",");
            var expectedValue = string.Empty;

            try
            {
                expectedValue = expectedTableData[rowNumber][columnNumber].Replace("[comma]", ",");
            }
            catch (ArgumentOutOfRangeException)
            {
                expectedValue = "<UNDEFINED>";
            }

            QAAssert.AreEqual(expectedValue, actualValue, continueOnFailure: true);
            if (!expectedValue.Equals(actualValue))
            {
                Report.Output(
                    Report.Level.Fail,
                    Resources.DataGridDifferentValueMsg,
                    rowNumber,
                    columnNumber,
                    expectedValue,
                    actualValue);

                hasFoundMismatch = true;
            }
        }

        /// <summary>
        ///     Compare the row count of two tables.
        /// </summary>
        /// <param name="expectedRowCount">Expected table row count</param>
        /// <param name="actualRowCount">Actual table row count</param>
        private void CompareRowCount(int expectedRowCount, int actualRowCount)
        {
            var rowCountMessage = string.Format(
                Resources.FriendlyDataGridCompareRowCountMsg,
                FriendlyName);

            QAAssert.AreEqual(
                expectedRowCount,
                actualRowCount,
                rowCountMessage,
                true);
        }

        /// <summary>
        ///     Returns all the values in the specified columns from a single row
        /// </summary>
        /// <param name="rowLine">the row line</param>
        /// <param name="columnNames">the column names</param>
        /// <returns>All the values in the specified columns from a single row</returns>
        private List<string> GetLineResult(int rowLine, string[] columnNames)
        {
            var rowValues = new List<string>();

            for (var k = 0; k < columnNames.Length; k++)
            {
                var cell_1 = Cell(columnNames[k], rowLine);
                rowValues.Add(cell_1.Value);
            }

            return rowValues;
        }
    }
}