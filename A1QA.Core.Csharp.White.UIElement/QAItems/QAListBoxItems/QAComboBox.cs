//  <copyright file="QAComboBox.cs" company="A1QA">
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
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAListBoxItems
{
    /// <summary>
    ///     Our own implementation of a ComboBox that extends the functionality of
    ///     White ComboBox
    /// </summary>
    public class QAComboBox : QAItem<ComboBox>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAComboBox" /> class
        /// </summary>
        /// <param name="button">White ComboBox</param>
        /// <param name="friendlyName">Friendly name for ComboBox</param>
        public QAComboBox(ComboBox comboBox, string friendlyName) : base(comboBox, friendlyName)
        {
        }

        /// <summary>
        ///     Gets all items belonging to the ListControl
        /// </summary>
        public ListItems Items => UIItem.Items;

        public bool IsEditable => UIItem.IsEditable;

        /// <summary>
        ///     Gets HelpText property of UIItem
        /// </summary>
        public string Text => UIItem.HelpText;

        public string SelectedItemText => UIItem.SelectedItemText;

        /// <summary>
        ///     Select item contained inside ComboBox by clicking on it with the cursor
        /// </summary>
        /// <param name="itemText">Item text</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void ClickItemWithText(string itemText, bool shouldVerify = false)
        {
            ReportActionValue("ClickItemWithName", itemText);

            var listItemFound = FindItemWithText(itemText);
            listItemFound.Click();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyComboBoxSelectMsg, itemText);
                QAAssert.IsTrue(QAListItem.GetParent(listItemFound, string.Empty).IsSelected, friendlyMessage);
            }
        }

        public void ClickItemWithTextWithOpening(string itemText)
        {
            ReportActionValue("ClickItemWithName", itemText);

            Click();
            var listItemFound = FindItemWithText(itemText);
            listItemFound.Click();
        }

        public void ClickItemWithTextScrollable(string itemText)
        {
            ReportActionValue("ClickItemWithName Scrollable", itemText);
            UIItem.ScrollBars.Vertical.SetToMinimum();
            var listItemFound = FindItemWithText(itemText);
            while (listItemFound == null && UIItem.ScrollBars.Vertical.Value != UIItem.ScrollBars.Vertical.MaximumValue)
            {
                UIItem.ScrollBars.Vertical.ScrollDownLarge();
                listItemFound = FindItemWithText(itemText);
            }

            listItemFound.Click();
        }

        public ListItem FindItemWithText(string itemText)
        {
            ListItem listItemFound = null;

            var items = UIItem.Items;

            foreach (var listItem in items)
            {
                if (listItem.Text.Equals(itemText))
                {
                    listItemFound = listItem;
                    break;
                }
            }

            if (listItemFound == null)
            {
                foreach (var listItem in items)
                {
                    var label = QALabel.Get(SearchCriteria.All, string.Empty, listItem, 2);

                    if (label.UIItem != null)
                    {
                        if (label.Text.Equals(itemText))
                        {
                            listItemFound = listItem;
                            break;
                        }
                    }
                }
            }

            if (listItemFound == null)
            {
                Click();
                listItemFound = QAListItem.Get(SearchCriteria.ByText(itemText), string.Empty).UIItem;
            }

            return listItemFound;
        }

        /// <summary>
        ///     Select item contained inside ComboBox using Automation
        /// </summary>
        /// <param name="itemText">Item text</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        /// <param name="useReturnForSelection">
        ///     UIItem.Click does not work in all situations,
        ///     for such cases pass the last parameter as true to make sure selection works for those cases
        /// </param>
        public void Select(string itemText, bool shouldVerify = true, bool useReturnForSelection = false)
        {
            ReportAction("Select");
            UIItem.Select(itemText);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyComboBoxSelectMsg, itemText);
                QAAssert.AreEqual(itemText, UIItem.SelectedItemText, friendlyMessage);
            }

            if (useReturnForSelection)
            {
                UIItem.KeyIn(KeyboardInput.SpecialKeys.RETURN);
            }
            else
            {
                UIItem.Click();
            }
        }

        /// <summary>
        ///     Get ListItem contained inside ComboBox
        /// </summary>
        /// <param name="itemText">Text to match</param>
        /// <returns>ListItem matching text</returns>
        public QAListItem Item(string itemText)
        {
            var item = UIItem.Item(itemText);
            return new QAListItem(item, itemText);
        }

        /// <summary>
        ///     Get a parent ComboBox of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ComboBox</param>
        /// <returns>Parent ComboBox</returns>
        public static QAComboBox GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAComboBox(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ComboBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ComboBox</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ComboBox</returns>
        public static QAComboBox Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var comboBox = FindUIItem(searchCriteria, scope, timeout);
            return new QAComboBox(comboBox, friendlyName);
        }

        public static QAComboBox Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new ComboBox(matchingAE, new NullActionListener())
                : null;

            return new QAComboBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ComboBoxes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ComboBoxes</returns>
        public static List<QAComboBox> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAComboBoxes = new List<QAComboBox>();
            var comboBoxes = FindUIItems(searchCriteria, scope, timeout);

            foreach (var comboBox in comboBoxes)
            {
                try
                {
                    QAComboBoxes.Add(new QAComboBox((ComboBox) comboBox, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAComboBoxes;
        }

        /// <summary>
        ///     Get a list of the textual elements in the dropdown
        /// </summary>
        /// <returns>List of strings for the dropdown section of the combobox</returns>
        public List<string> GetListItemsText()
        {
            var result = new List<string>();

            foreach (var item in UIItem.Items)
            {
                result.Add(item.Text);
            }

            return result;
        }

        /// <summary>
        ///     Enter text into a ComboBox using keyboard and then verify.
        /// </summary>
        /// <param name="text">Text to enter</param>
        public void EnterText(string text)
        {
            try
            {
                ReportActionValue("EnterText", text);
                UIItem.Enter(text);
                CaptureImage();
            }
            catch (Exception ex)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyComboBoxNotEditable);
                Report.Output(Report.Level.Debug, friendlyMessage, ex);
            }
        }
    }
}