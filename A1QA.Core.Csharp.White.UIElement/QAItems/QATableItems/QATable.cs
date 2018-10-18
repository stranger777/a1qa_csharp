//  <copyright file="QATable.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.SystemUtilities;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TableItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATableItems
{
    /// <summary>
    ///     Our own implementation of a Table that extends the functionality of
    ///     White Table
    /// </summary>
    public class QATable : QAItem<Table>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATable" /> class
        /// </summary>
        /// <param name="customTable">White CustomItem</param>
        /// <param name="friendlyName">Friendly name for Table</param>
        public QATable(Table customTable, string friendlyName) : base(customTable, friendlyName)
        {
        }

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
                var gridPattern = (TablePattern) UIItem.AutomationElement.GetCurrentPattern(TablePattern.Pattern);
                return gridPattern.Current.RowCount;
            }
        }

        /// <summary>
        ///     Gets total number of available columns
        /// </summary>
        public int ColumnCount => UIItem.Header.Columns.Count;

        /// <summary>
        ///     Get a Table based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Table</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching QATable</returns>
        public static QATable Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var table = FindUIItem(searchCriteria, scope, timeout);
            return new QATable(table, friendlyName);
        }

        public static QATable Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new Table(matchingAE, new NullActionListener())
                : null;

            return new QATable(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Find row in table
        /// </summary>
        /// <param name="rowNumber">Number of row</param>
        /// <returns>Row at given row number</returns>
        public QATableRow GetRow(int rowNumber)
        {
            return new QATableRow(UIItem.Rows[rowNumber], rowNumber.ToString());
        }

        /// <summary>
        ///     Find column header in table
        /// </summary>
        /// <param name="columnNumber">Number of column</param>
        /// <returns>Column header at given column number</returns>
        public QATableColumn GetColumnHeader(int columnNumber)
        {
            return new QATableColumn(UIItem.Header.Columns[columnNumber], columnNumber.ToString());
        }

        /// <summary>
        ///     Select a row from the table
        /// </summary>
        /// <param name="zeroBasedRowIndex">Index of row to be selected</param>
        public void SelectFirstRow(int zeroBasedRowIndex)
        {
            var searchCriteria = SearchCriteria.ByAutomationId(string.Format("Row_{0}", zeroBasedRowIndex));
            var row = QATableRow.Get(searchCriteria, string.Empty, UIItem, 5000);
            var firstCell = QAUIItem.Get(SearchCriteria.Indexed(0), string.Empty, row.UIItem, 5000);
            var location = firstCell.Location;
            location.X -= 2;
            location.Y += 2;
            QAMouse.Click(new Point((int) location.X, (int) location.Y));
        }

        /// <summary>
        ///     Find index of a column in the table
        /// </summary>
        /// <param name="columnName">the name of the column</param>
        /// <returns>Index of specified column if it exists, -1 otherwise</returns>
        public int GetHeaderIndex(string columnName)
        {
            var headers = ColumnCount;
            for (var i = 0; i < headers; i++)
            {
                if (UIItem.Header.Columns[i].Name == columnName)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Gets a list of values for the specified column
        /// </summary>
        /// <param name="columnName">the name of the column</param>
        /// <returns></returns>
        public List<string> GetColumnValues(string columnName)
        {
            var values = new List<string>();
            var intColumn = GetHeaderIndex(columnName);
            var rowNumber = VisibleRowCount;
            for (var i = 0; i < rowNumber; i++)
            {
                values.Add(UIItem.Rows[i].Cells[intColumn].Value.ToString());
            }
            return values;
        }

        /// <summary>
        ///     Get row index by column name and value
        /// </summary>
        /// <param name="columnName">the name of the column</param>
        /// <param name="cellValue"></param>
        /// <returns>returns the row index</returns>
        public int GetRowIndex(string columnName, string cellValue)
        {
            int index;
            var intColumn = GetHeaderIndex(columnName);
            var rowNumber = VisibleRowCount;
            for (var i = 0; i < rowNumber; i++)
            {
                if (UIItem.Rows[i].Cells[intColumn].Value.ToString().Equals(cellValue))
                {
                    index = i;
                    return index;
                }
            }
            return -1;
        }

        /// <summary>
        ///     Unselects previously selected row and selects this row.
        ///     If this row is already selected it doesn't have any effect.
        /// </summary>
        /// <param name="zeroBasedRowIndex">Index of row to be selected</param>
        public void Select(int zeroBasedRowIndex)
        {
            GetRow(zeroBasedRowIndex).Click();
        }
    }
}