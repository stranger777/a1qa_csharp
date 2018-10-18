//  <copyright file="QACheckBox.cs" company="A1QA">
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

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a CheckBox that extends the functionality of
    ///     White CheckBox
    /// </summary>
    public class QACheckBox : QAItem<CheckBox>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QACheckBox" /> class
        /// </summary>
        /// <param name="checkBox">White CheckBox</param>
        /// <param name="friendlyName">Friendly name for CheckBox</param>
        public QACheckBox(CheckBox checkBox, string friendlyName) : base(checkBox, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a CheckBox is checked
        /// </summary>
        public bool Checked
        {
            get { return UIItem.Checked; }

            set { UIItem.Checked = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether a CheckBox is enabled
        /// </summary>
        public bool Enabled => UIItem.Enabled;

        /// <summary>
        ///     Gets a value indicating whether a CheckBox is off screen
        /// </summary>
        public bool IsOffScreen => UIItem.IsOffScreen;

        public void Set(bool value)
        {
            if (Checked != value)
            {
                Click();
            }
        }

        /// <summary>
        ///     Get a parent CheckBox of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for CheckBox</param>
        /// <returns>Parent CheckBox</returns>
        public static QACheckBox GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QACheckBox(parent, friendlyName);
        }

        /// <summary>
        ///     Get a CheckBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for CheckBox</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching CheckBox</returns>
        public static QACheckBox Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var checkBox = FindUIItem(searchCriteria, scope, timeout);
            return new QACheckBox(checkBox, friendlyName);
        }

        public static QACheckBox Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var completeCondition = new AndCondition(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "check box"), condition);
            var checkboxAE = FindUIItem(completeCondition, treeScope, scope, timeout);
            var matchingUIItem = checkboxAE != null
                ? new CheckBox(checkboxAE, new NullActionListener())
                : null;

            return new QACheckBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* CheckBoxes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching CheckBoxes</returns>
        public static List<QACheckBox> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QACheckBoxes = new List<QACheckBox>();
            var checkBoxes = FindUIItems(searchCriteria, scope, timeout);

            foreach (var checkBox in checkBoxes)
            {
                try
                {
                    QACheckBoxes.Add(new QACheckBox((CheckBox) checkBox, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QACheckBoxes;
        }

        /// <summary>
        ///     Selects a CheckBox
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Select(bool shouldVerify = true)
        {
            ReportAction("Select");
            UIItem.Select();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyCheckBoxSelectMsg);
                QAAssert.AreEqual(true, UIItem.IsSelected, friendlyMessage);
            }
        }

        /// <summary>
        ///     Unselects a CheckBox
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void UnSelect(bool shouldVerify = true)
        {
            ReportAction("UnSelect");
            UIItem.UnSelect();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyCheckBoxUnSelectMsg);
                QAAssert.AreEqual(false, UIItem.IsSelected, friendlyMessage);
            }
        }
    }
}