//  <copyright file="QAGroupBox.cs" company="A1QA">
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
    ///     Our own implementation of a GroupBox that extends the functionality of
    ///     White GroupBox
    /// </summary>
    public class QAGroupBox : QAItem<GroupBox>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAGroupBox" /> class
        /// </summary>
        /// <param name="groupBox">White GroupBox</param>
        /// <param name="friendlyName">Friendly name for GroupBox</param>
        public QAGroupBox(GroupBox groupBox, string friendlyName) : base(groupBox, friendlyName)
        {
        }

        /// <summary>
        ///     Gets all items belonging to the Collection
        /// </summary>
        public UIItemCollection Items => UIItem.Items;

        public static QAGroupBox Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new GroupBox(containerAE, new NullActionListener())
                : null;

            return new QAGroupBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a parent GroupBox of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for GroupBox</param>
        /// <returns>Parent GroupBox</returns>
        public static QAGroupBox GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAGroupBox(parent, friendlyName);
        }

        /// <summary>
        ///     Get a GroupBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for GroupBox</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching GroupBox</returns>
        public static QAGroupBox Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var groupBox = FindUIItem(searchCriteria, scope, timeout);
            return new QAGroupBox(groupBox, friendlyName);
        }

        /// <summary>
        ///     Get a GroupBox based on AutomationElement condition
        /// </summary>
        /// <param name="condition">AutomationElement identification conditions</param>
        /// <param name="treeScope"> Depth search level for the UI Automation tree</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching GroupBox</returns>
        public static QAGroupBox Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var groupBoxAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = groupBoxAE != null
                ? new GroupBox(groupBoxAE, new NullActionListener())
                : null;

            return new QAGroupBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* GroupBoxes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching GroupBoxes</returns>
        public static List<QAGroupBox> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAGroupBoxes = new List<QAGroupBox>();
            var groupBoxes = QAButton.FindUIItems(searchCriteria, scope, timeout);

            foreach (var groupBox in groupBoxes)
            {
                try
                {
                    QAGroupBoxes.Add(new QAGroupBox((GroupBox) groupBox, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAGroupBoxes;
        }
    }
}