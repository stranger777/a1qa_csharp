//  <copyright file="QAToolStrip.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowStripControls;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowStripControls
{
    /// <summary>
    ///     Our own implementation of a ToolStrip that extends the functionality of
    ///     White ToolStrip
    /// </summary>
    public class QAToolStrip : QAItem<ToolStrip>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAToolStrip" /> class
        /// </summary>
        /// <param name="toolStrip">White ToolStrip</param>
        /// <param name="friendlyName">Friendly name for ToolStrip</param>
        public QAToolStrip(ToolStrip toolStrip, string friendlyName) : base(toolStrip, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent ToolStrip of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ToolStrip</param>
        /// <returns>Parent ToolStrip</returns>
        public static QAToolStrip GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAToolStrip(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ToolStrip based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ToolStrip</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ToolStrip</returns>
        public static QAToolStrip Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var toolStrip = FindUIItem(searchCriteria, scope, timeout);
            return new QAToolStrip(toolStrip, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ToolStrips based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ToolStrips</returns>
        public static List<QAToolStrip> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAToolStrips = new List<QAToolStrip>();
            var toolStrips = FindUIItems(searchCriteria, scope, timeout);

            foreach (var toolStrip in toolStrips)
            {
                try
                {
                    QAToolStrips.Add(new QAToolStrip((ToolStrip) toolStrip, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAToolStrips;
        }
    }
}