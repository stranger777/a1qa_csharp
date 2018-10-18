//  <copyright file="QARadioButton.cs" company="A1QA">
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
    ///     Our own implementation of a RadioButton that extends the functionality of
    ///     White RadioButton
    /// </summary>
    public class QARadioButton : QAItem<RadioButton>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QARadioButton" /> class
        /// </summary>
        /// <param name="radioButton">White Radio Button</param>
        /// <param name="friendlyName">Friendly name for RadioButton</param>
        public QARadioButton(RadioButton radioButton, string friendlyName) : base(radioButton, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a RadioButton is selected
        /// </summary>
        public bool IsSelected => UIItem.IsSelected;

        /// <summary>
        ///     Get a parent RadioButton of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for RadioButton</param>
        /// <returns>Parent RadioButton</returns>
        public static QARadioButton GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QARadioButton(parent, friendlyName);
        }

        public static QARadioButton Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new RadioButton(containerAE, new NullActionListener())
                : null;

            return new QARadioButton(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a RadioButton based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for RadioButton</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching RadioButton</returns>
        public static QARadioButton Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var radioButton = FindUIItem(searchCriteria, scope, timeout);
            return new QARadioButton(radioButton, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* RadioButtons based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching RadioButtons</returns>
        public static List<QARadioButton> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QARadioButtons = new List<QARadioButton>();
            var radioButtons = FindUIItems(searchCriteria, scope, timeout);

            foreach (var radioButton in radioButtons)
            {
                try
                {
                    QARadioButtons.Add(new QARadioButton((RadioButton) radioButton, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QARadioButtons;
        }

        /// <summary>
        ///     Selects RadioButton and then verify
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