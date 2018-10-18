//  <copyright file="QAListItem.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAListBoxItems
{
    /// <summary>
    ///     Our own implementation of a ListItem that extends the functionality of
    ///     White ListItem
    /// </summary>
    public class QAListItem : QAItem<ListItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAListItem" /> class
        /// </summary>
        /// <param name="listItem">White ListItem</param>
        /// <param name="friendlyName">Friendly name for ListItem</param>
        public QAListItem(ListItem listItem, string friendlyName) : base(listItem, friendlyName)
        {
        }

        /// <summary>
        ///     Get a value indicating whether a ListItem is selected
        /// </summary>
        public bool IsSelected => UIItem.IsSelected;

        /// <summary>
        ///     Returns List of strings with of item text and inner Label text if exists
        /// </summary>
        public List<string> GetListItemText
        {
            get
            {
                var list = new List<string>();

                list.Add(UIItem.Text);

                var label = QALabel.Get(SearchCriteria.All, string.Empty, UIItem, 1);

                if (label.Exists)
                {
                    list.Add(label.Text);
                }

                return list;
            }
        }

        /// <summary>
        ///     Returns text of inner Label or TextBox
        /// </summary>
        public string GetInnerText
        {
            get
            {
                string nodeName;

                var textOfListItem = QATextBox.Get(SearchCriteria.All, "List node text", UIItem, 1);
                if (textOfListItem.Exists)
                {
                    nodeName = textOfListItem.Text;
                }
                else
                {
                    var label = QALabel.Get(SearchCriteria.All, "List node button", UIItem, 1);
                    nodeName = label.Exists
                        ? label.Text
                        : string.Empty;
                }
                return nodeName;
            }
        }

        /// <summary>
        ///     Get a parent ListItem of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ListItem</param>
        /// <returns>Parent ListItem</returns>
        public static QAListItem GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAListItem(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ListItem based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ListItem</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListItem</returns>
        public static QAListItem Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var listItem = FindUIItem(searchCriteria, scope, timeout);
            return new QAListItem(listItem, friendlyName);
        }

        public static UIItem Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            return matchingAE != null
                ? new UIItem(matchingAE, new NullActionListener())
                : new UIItem(null, new NullActionListener());
        }

        public static UIItem Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var completeCondition = new AndCondition(condition, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "list item"));
            var listitemAe = FindUIItem(completeCondition, treeScope, scope, timeout);
            return listitemAe != null
                ? new UIItem(listitemAe, new NullActionListener())
                : null;
        }

        /// <summary>
        ///     Gets *multiple* ListItems based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListItems</returns>
        public static List<QAListItem> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAListItems = new List<QAListItem>();
            var listItems = FindUIItems(searchCriteria, scope, timeout);

            foreach (var listItem in listItems)
            {
                try
                {
                    QAListItems.Add(new QAListItem((ListItem) listItem, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAListItems;
        }

        /// <summary>
        ///     Select ListItem and then verify
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
        ///     Check ListItem and then verify
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Check(bool shouldVerify = true)
        {
            ReportAction("Check");
            UIItem.Check();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyListItemCheckMsg);
                QAAssert.AreEqual(true, UIItem.Checked, friendlyMessage);
            }
        }

        /// <summary>
        ///     UnCheck ListItem and then verify
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void UnCheck(bool shouldVerify = true)
        {
            ReportAction("UnCheck");
            UIItem.UnCheck();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyListItemUnCheckMsg);
                QAAssert.AreEqual(false, UIItem.Checked, friendlyMessage);
            }
        }
    }
}