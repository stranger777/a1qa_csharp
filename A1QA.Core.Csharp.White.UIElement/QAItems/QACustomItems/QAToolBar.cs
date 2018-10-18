//  <copyright file="QAToolBar.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Custom;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QACustomItems
{
    public class QAToolBar : QAItem<CustomItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAToolBar" /> class
        /// </summary>
        /// <param name="customToolbar">White CustomItem</param>
        /// <param name="friendlyName">Friendly name for ToolBar</param>
        public QAToolBar(CustomItem customToolbar, string friendlyName) : base(customToolbar, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent ToolBar of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ToolBar</param>
        /// <returns>Parent ToolBar</returns>
        public static QAToolBar GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAToolBar(parent, friendlyName);
        }

        /// <summary>
        ///     Get a ToolBar based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ToolBar</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching QAToolBar</returns>
        public static QAToolBar Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var toolbar = FindUIItem(searchCriteria, scope, timeout);
            return new QAToolBar(toolbar, friendlyName);
        }

        /// <summary>
        ///     Get a Toolbar based on xPath
        /// </summary>
        /// <param name="xPath">xPath to the element</param>
        /// <param name="friendlyName">Formal Name of the element</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching QAToolBar</returns>
        public static QAToolBar Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new CustomItem(matchingAE, new NullActionListener())
                : null;

            return new QAToolBar(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ToolBars based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ToolBars</returns>
        public static List<QAToolBar> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAToolBars = new List<QAToolBar>();
            var toolBars = FindUIItems(searchCriteria, scope, timeout);

            foreach (var toolBar in toolBars)
            {
                try
                {
                    QAToolBars.Add(new QAToolBar((CustomItem) toolBar, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAToolBars;
        }
    }
}