//  <copyright file="QAListBox.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Extentions;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAListBoxItems
{
    /// <summary>
    ///     Our own implementation of a ListBox that extends the functionality of
    ///     White ListBox
    /// </summary>
    public class QAListBox : QAItem<ListBox>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAListBox" /> class
        /// </summary>
        /// <param name="listbox">White ListBox</param>
        /// <param name="friendlyName">Friendly name for ListBox</param>
        public QAListBox(ListBox listbox, string friendlyName) : base(listbox, friendlyName)
        {
        }

        /// <summary>
        ///     Gets text of selected item in ListBox
        /// </summary>
        public string SelectedItemText => UIItem.SelectedItemText;

        /// <summary>
        ///     Gets all items belonging to the ListControl
        /// </summary>
        public ListItems Items => UIItem.Items;

        /// <summary>
        ///     Get a parent ListBox of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ListBox</param>
        /// <returns>Parent ListBox</returns>
        public static QAListBox GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAListBox(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ListBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ListBox</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListBox</returns>
        public static QAListBox Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var listBox = FindUIItem(searchCriteria, scope, timeout);
            return new QAListBox(listBox, friendlyName);
        }

        public static QAListBox Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new ListBox(matchingAE, new NullActionListener())
                : null;

            return new QAListBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a ListBox based on AutomationElement condition
        /// </summary>
        /// <param name="condition">AutomationElement identification conditions</param>
        /// <param name="treeScope"> Depth search level for the UI Automation tree</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListBox</returns>
        public static QAListBox Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var listBoxAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = listBoxAE != null
                ? new ListBox(listBoxAE, new NullActionListener())
                : null;

            return new QAListBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a ListBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="automationProperty">AutomationElement property</param>
        /// <param name="automationPropertyValue">Value of AutomationElement property</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListBox</returns>
        public static QAListBox Get(SearchCriteria searchCriteria, AutomationProperty automationProperty, object automationPropertyValue, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var button = FindUIItem(searchCriteria, automationProperty, automationPropertyValue, scope, timeout);

            return new QAListBox(button, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ListBoxes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ListBoxes</returns>
        public static List<QAListBox> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAListBoxes = new List<QAListBox>();
            var listBoxes = FindUIItems(searchCriteria, scope, timeout);

            foreach (var listBox in listBoxes)
            {
                try
                {
                    QAListBoxes.Add(new QAListBox((ListBox) listBox, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAListBoxes;
        }

        /// <summary>
        ///     Check all ListItems contained inside ListBox
        /// </summary>
        public void CheckAllItems()
        {
            ReportAction("CheckAllItems");
            foreach (var listItem in UIItem.Items)
            {
                if (listItem.Checked == false)
                {
                    listItem.Check();
                }
            }
        }

        /// <summary>
        ///     UnCheck all ListItems contained inside ListBox
        /// </summary>
        public void UnCheckAllItems()
        {
            ReportAction("UnCheckAllItems");
            foreach (var listItem in UIItem.Items)
            {
                if (listItem.Checked)
                {
                    listItem.UnCheck();
                }
            }
        }

        /// <summary>
        ///     Selects ListItem by index
        /// </summary>
        /// <param name="zeroBasedIndex">Index value</param>
        public void Select(int zeroBasedIndex)
        {
            ReportActionValue("Select", zeroBasedIndex);
            UIItem.Select(zeroBasedIndex);
        }

        /// <summary>
        ///     Selects ListItem which matches the text
        /// </summary>
        /// <param name="itemText">Text value</param>
        public void Select(string itemText)
        {
            ReportActionValue("Select", itemText);
            UIItem.Select(itemText);
        }

        /// <summary>
        ///     Finds and returns a ListItem from a ListBox using the label of the ListItem
        /// </summary>
        /// <param name="listItemLabel">label of the ListItem</param>
        /// <param name="returnNullIfNotFound">function will not throw an exception, but instead return null if not found</param>
        /// <returns>ListItem with the passed in label</returns>
        public QAListItem FindListItem(string listItemLabel, bool returnNullIfNotFound = false)
        {
            ReportActionValue("FindListItem", listItemLabel);
            ListItem listItemFound = null;

            // Check if ListItem text matches
            foreach (var listItem in UIItem.Items)
            {
                if (listItem.Text.Equals(listItemLabel))
                {
                    listItemFound = listItem;
                    break;
                }
            }

            // Check if label inside scope of ListItem matches
            if (listItemFound == null)
            {
                foreach (var listItem in UIItem.Items)
                {
                    var label = QALabel.Get(SearchCriteria.All, string.Empty, listItem, 2);

                    if (label.UIItem != null && label.Text.Equals(listItemLabel))
                    {
                        listItemFound = listItem;
                        break;
                    }
                }
            }

            return listItemFound == null && returnNullIfNotFound
                ? null
                : new QAListItem(listItemFound, listItemLabel);
        }

        public QAListItem FindListItemByContainsText(string text)
        {
            ReportActionValue("FindListItem", text);
            ListItem listItemFound = null;

            foreach (var listItem in UIItem.Items)
            {
                var textFromListItem = listItem.GetInnerText();
                foreach (var value in textFromListItem)
                {
                    if (value.ToString().Contains(text))
                    {
                        listItemFound = listItem;
                        break;
                    }
                }
            }

            return new QAListItem(listItemFound, text);
        }

        /// <summary>
        ///     Scroll left into a listbox
        /// </summary>
        public void ScrollLeft()
        {
            if (UIItem.ScrollBars.Horizontal.IsScrollable)
            {
                UIItem.ScrollBars.Horizontal.SetToMinimum();
                var position = UIItem.ScrollBars.Horizontal.MinimumValue;

                while (position < UIItem.ScrollBars.Horizontal.MaximumValue)
                {
                    UIItem.ScrollBars.Horizontal.ScrollLeft();
                    position++;
                }
            }
        }

        /// <summary>
        ///     Scroll Down into a listbox
        /// </summary>
        public void ScrollDown()
        {
            if (UIItem.ScrollBars.Vertical.IsScrollable)
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

        /// <summary>
        ///     Scroll right into a listbox
        /// </summary>
        public void ScrollRight()
        {
            if (UIItem.ScrollBars.Horizontal.IsScrollable)
            {
                UIItem.ScrollBars.Horizontal.SetToMinimum();
                var position = UIItem.ScrollBars.Horizontal.MinimumValue;

                while (position < UIItem.ScrollBars.Horizontal.MaximumValue)
                {
                    UIItem.ScrollBars.Horizontal.ScrollRight();
                    position++;
                }
            }
        }
    }
}