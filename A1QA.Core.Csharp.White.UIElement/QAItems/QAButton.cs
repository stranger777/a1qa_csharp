//  <copyright file="QAButton.cs" company="A1QA">
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
    ///     Our own implementation of a Button that extends the functionality of
    ///     White Button
    /// </summary>
    public class QAButton : QAItem<Button>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAButton" /> class
        /// </summary>
        /// <param name="button">White Button</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        public QAButton(Button button, string friendlyName) : base(button, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a Button is enabled
        /// </summary>
        public bool Enabled => UIItem.Enabled;

        /// <summary>
        ///     Gets a value indicating the ToggleState of a Button
        /// </summary>
        public ToggleState ToggleState
        {
            get
            {
                object objPattern;
                TogglePattern togPattern;

                if (UIItem.AutomationElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
                {
                    togPattern = objPattern as TogglePattern;
                    return togPattern.Current.ToggleState;
                }

                return ToggleState.Indeterminate;
            }
            set { UIItem.State = value; }
        }

        public void SetToggleState(ToggleState state)
        {
            if (ToggleState != state)
            {
                Click();
            }
        }

        /// <summary>
        ///     Get a parent Button of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <returns>Parent Button</returns>
        public static QAButton GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAButton(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Button based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Button</returns>
        public static QAButton Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var button = FindUIItem(searchCriteria, scope, timeout);
            return new QAButton(button, friendlyName);
        }

        /// <summary>
        ///     Get a Button based on AutomationElement condition
        /// </summary>
        /// <param name="condition">AutomationElement identification conditions</param>
        /// <param name="treeScope"> Depth search level for the UI Automation tree</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Button</returns>
        public static QAButton Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var buttonAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = buttonAE != null
                ? new Button(buttonAE, new NullActionListener())
                : null;

            return new QAButton(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a Button based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="automationProperty">AutomationElement property</param>
        /// <param name="automationPropertyValue">Value of AutomationElement property</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Button</returns>
        public static QAButton Get(SearchCriteria searchCriteria, AutomationProperty automationProperty, object automationPropertyValue, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var button = FindUIItem(searchCriteria, automationProperty, automationPropertyValue, scope, timeout);

            return new QAButton(button, friendlyName);
        }

        public static QAButton Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var buttonAE = GetByXPath(xPath);

            var matchingUIItem = buttonAE != null
                ? new Button(buttonAE, new NullActionListener())
                : null;

            return new QAButton(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Buttons based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Buttons</returns>
        public static List<QAButton> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAButtons = new List<QAButton>();
            var buttons = FindUIItems(searchCriteria, scope, timeout);

            foreach (var button in buttons)
            {
                try
                {
                    QAButtons.Add(new QAButton((Button) button, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAButtons;
        }

        public static object Get(PropertyCondition propertyCondition, object children, string v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}