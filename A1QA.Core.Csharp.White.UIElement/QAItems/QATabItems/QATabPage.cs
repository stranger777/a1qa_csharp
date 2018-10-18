//  <copyright file="QATabPage.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATabItems
{
    /// <summary>
    ///     Our own implementation of a TabPage that extends the functionality of
    ///     White TabPage
    /// </summary>
    public class QATabPage : QAItem<TabPage>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATabPage" /> class
        /// </summary>
        /// <param name="tabPage">White TabPage</param>
        /// <param name="friendlyName">Friendly name for TabPage</param>
        public QATabPage(TabPage tabPage, string friendlyName) : base(tabPage, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a Tab Page is selected
        /// </summary>
        public bool IsSelected => UIItem.IsSelected;

        /// <summary>
        ///     Get a parent TabPage of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for TabPage</param>
        /// <returns>Parent TabPage</returns>
        public static QATabPage GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QATabPage(parent, friendlyName);
        }

        /// <summary>
        ///     Get a TabPage based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TabPage</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TabPage</returns>
        public static QATabPage Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tabPage = FindUIItem(searchCriteria, scope, timeout);
            return new QATabPage(tabPage, friendlyName);
        }

        public static QATabPage Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new TabPage(matchingAE, new NullActionListener())
                : null;

            return new QATabPage(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Select TabPage and then verify
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
    }
}