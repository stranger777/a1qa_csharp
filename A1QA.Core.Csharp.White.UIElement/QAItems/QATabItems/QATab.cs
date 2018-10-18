//  <copyright file="QATab.cs" company="A1QA">
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
    ///     Our own implementation of a Tab that extends the functionality of
    ///     White Tab
    /// </summary>
    public class QATab : QAItem<Tab>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATab" /> class
        /// </summary>
        /// <param name="tab">White Tab</param>
        /// <param name="friendlyName">Friendly name for Tab</param>
        public QATab(Tab tab, string friendlyName) : base(tab, friendlyName)
        {
        }

        /// <summary>
        ///     Gets all pages belonging to the TabControl
        /// </summary>
        public TabPages Pages => UIItem.Pages;

        /// <summary>
        ///     Get a Tab based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Tab</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Tab</returns>
        public static QATab Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tab = FindUIItem(searchCriteria, scope, timeout);
            return new QATab(tab, friendlyName);
        }

        public static QATab Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new Tab(matchingAE, new NullActionListener())
                : null;

            return new QATab(matchingUIItem, friendlyName);
        }

        public void SelectTabPage(string tabTitle, bool shouldVerify = true)
        {
            ReportAction("Select tab ");
            UIItem.SelectTabPage(tabTitle);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTabSelectMsg, tabTitle);

                QAAssert.AreEqual(tabTitle, UIItem.SelectedTab.ToString(), friendlyMessage);
            }
        }
    }
}