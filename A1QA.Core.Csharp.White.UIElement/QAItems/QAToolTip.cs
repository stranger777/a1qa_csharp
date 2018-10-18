//  <copyright file="QAToolTip.cs" company="A1QA">
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
    ///     Our own implementation of a ToolTip that extends the functionality of
    ///     White ToolTip
    /// </summary>
    public class QAToolTip : QAItem<ToolTip>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAToolTip" /> class
        /// </summary>
        /// <param name="toolTip">White ToolTip</param>
        /// <param name="friendlyName">Friendly name for ToolTip</param>
        public QAToolTip(ToolTip toolTip, string friendlyName) : base(toolTip, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent ToolTip of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ToolTip</param>
        /// <returns>Parent ToolTip</returns>
        public static QAToolTip GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAToolTip(parent, friendlyName);
        }

        public static QAToolTip Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new ToolTip(containerAE, new NullActionListener())
                : null;

            return new QAToolTip(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a ToolTip based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ToolTip</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ToolTip</returns>
        public static QAToolTip Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var toolTip = FindUIItem(searchCriteria, scope, timeout);
            return new QAToolTip(toolTip, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ToolTips based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ToolTips</returns>
        public static List<QAToolTip> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAToolTips = new List<QAToolTip>();
            var toolTips = FindUIItems(searchCriteria, scope, timeout);

            foreach (var toolTip in toolTips)
            {
                try
                {
                    QAToolTips.Add(new QAToolTip((ToolTip) toolTip, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAToolTips;
        }
    }
}