//  <copyright file="QADataGridColumn.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListViewItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QADataGridItems
{
    /// <summary>
    ///     Our own implementation of a DataGridColumn that extends the functionality of
    ///     White ListViewColumn
    /// </summary>
    public class QADataGridColumn : QAItem<ListViewColumn>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QADataGridColumn" /> class
        /// </summary>
        /// <param name="listViewColumn">White ListViewColumn</param>
        /// <param name="friendlyName">Friendly name for DataGridColumn</param>
        public QADataGridColumn(ListViewColumn listViewColumn, string friendlyName) : base(listViewColumn, friendlyName)
        {
        }

        /// <summary>
        ///     Get a DataGridColumn based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for DataGridColumn</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DataGridColumn</returns>
        public static QADataGridColumn Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var dataGridColumn = FindUIItem(searchCriteria, scope, timeout);
            return new QADataGridColumn(dataGridColumn, friendlyName);
        }
    }
}