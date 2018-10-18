//  <copyright file="QALabel.cs" company="A1QA">
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
    ///     Our own implementation of a Label that extends the functionality of
    ///     White Label
    /// </summary>
    public class QALabel : QAItem<Label>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QALabel" /> class
        /// </summary>
        /// <param name="label">White Label</param>
        /// <param name="friendlyName">Friendly name for Label</param>
        public QALabel(Label label, string friendlyName) : base(label, friendlyName)
        {
        }

        /// <summary>
        ///     Gets the text from the Label
        /// </summary>
        public string Text => UIItem.Text;

        /// <summary>
        ///     Get a parent Label of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Label</param>
        /// <returns>Parent Label</returns>
        public static QALabel GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QALabel(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Label based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Label</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Label</returns>
        public static QALabel Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var label = FindUIItem(searchCriteria, scope, timeout);
            return new QALabel(label, friendlyName);
        }

        public static QALabel Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Label(containerAE, new NullActionListener())
                : null;

            return new QALabel(matchingUIItem, friendlyName);
        }

        public static QALabel Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var completedCondition = new AndCondition(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "text"), condition);
            var labelAE = FindUIItem(completedCondition, treeScope, scope, timeout);
            var matchingUIItem = labelAE != null
                ? new Label(labelAE, new NullActionListener())
                : null;

            return new QALabel(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Labels based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Labels</returns>
        public static List<QALabel> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QALabels = new List<QALabel>();
            var labels = FindUIItems(searchCriteria, scope, timeout);

            foreach (var label in labels)
            {
                try
                {
                    QALabels.Add(new QALabel((Label) label, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QALabels;
        }
    }
}