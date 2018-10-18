//  <copyright file="QATableColumn.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TableItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATableItems
{
    /// <summary>
    ///     Our own implementation of a TableColumn that extends the functionality of
    ///     White ListViewColumn
    /// </summary>
    public class QATableColumn : QAItem<TableColumn>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATableColumn" /> class
        /// </summary>
        /// <param name="tableColumn">White ListViewColumn</param>
        /// <param name="friendlyName">Friendly name for TableColumn</param>
        public QATableColumn(TableColumn tableColumn, string friendlyName) : base(tableColumn, friendlyName)
        {
        }

        /// <summary>
        ///     Get a TableColumn based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TableColumn</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TableColumn</returns>
        public static QATableColumn Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tableColumn = FindUIItem(searchCriteria, scope, timeout);
            return new QATableColumn(tableColumn, friendlyName);
        }

        public static QATableColumn Get(string xPath, string friendlyName, int index, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new TableColumn(matchingAE, new NullActionListener(), index)
                : null;

            return new QATableColumn(matchingUIItem, friendlyName);
        }
    }
}