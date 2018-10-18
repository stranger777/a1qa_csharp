//  <copyright file="QAListViewRow.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a ListViewRow that extends the functionality of
    ///     White ListViewRow
    /// </summary>
    public class QAListViewRow : QAItem<ListViewRow>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAListViewRow" /> class
        /// </summary>
        /// <param name="listViewRow">White ListViewRow</param>
        /// <param name="friendlyName">Friendly name for ListViewRow</param>
        public QAListViewRow(ListViewRow listViewRow, string friendlyName) : base(listViewRow, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a Tab Page is selected
        /// </summary>
        public bool IsSelected => UIItem.IsSelected;

        public static QAListViewRow Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new ListViewRow(containerAE, new NullActionListener())
                : null;

            return new QAListViewRow(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a parent ListViewRow of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ListViewRow</param>
        /// <returns>Parent ListViewRow</returns>
        public static QAListViewRow GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAListViewRow(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ListViewRow based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ListViewRow</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListViewRow</returns>
        public static QAListViewRow Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var listViewRow = FindUIItem(searchCriteria, scope, timeout);
            return new QAListViewRow(listViewRow, friendlyName);
        }

        /// <summary>
        ///     Select ListViewRow and then verify
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Select(bool shouldVerify = true)
        {
            ReportAction("Select");
            UIItem.Select();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyListItemSelectMsg);
                QAAssert.AreEqual(true, UIItem.IsSelected, friendlyMessage);
            }
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
    }
}