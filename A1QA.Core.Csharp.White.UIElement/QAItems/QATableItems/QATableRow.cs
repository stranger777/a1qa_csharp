//  <copyright file="QATableRow.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TableItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATableItems
{
    /// <summary>
    ///     Our own implementation of a TableRow that extends the functionality of
    ///     White TableRow
    /// </summary>
    public class QATableRow : QAItem<TableRow>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATableRow" /> class
        /// </summary>
        /// <param name="tableRow">White ListViewRow</param>
        /// <param name="friendlyName">Friendly name for TableRow</param>
        public QATableRow(TableRow tableRow, string friendlyName) : base(tableRow, friendlyName)
        {
        }

        /// <summary>
        ///     Get a TableRow based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TableRow</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TableRow</returns>
        public static QATableRow Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tableRow = FindUIItem(searchCriteria, scope, timeout);
            return new QATableRow(tableRow, friendlyName);
        }

        /// <summary>
        ///     Select this row
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Select(bool shouldVerify = true)
        {
            ReportAction("Select");
            UIItem.Select();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTableRowSelectMsg);
                QAAssert.AreEqual(true, UIItem.IsFocussed, friendlyMessage);
            }
        }
    }
}