//  <copyright file="QATitleBar.cs" company="A1QA">
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
using TestStack.White.UIItems.WindowItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems
{
    public class QATitleBar : QAItem<TitleBar>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATitleBar" /> class
        /// </summary>
        /// <param name="titleBar">White TitleBar</param>
        /// <param name="friendlyName">Friendly name for TitleBar</param>
        public QATitleBar(TitleBar titleBar, string friendlyName) : base(titleBar, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent TitleBar of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for TitleBar</param>
        /// <returns>Parent TitleBar</returns>
        public static QATitleBar GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QATitleBar(parent, friendlyName);
        }

        /// <summary>
        ///     Get a TitleBar based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TitleBar</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TitleBar</returns>
        public static QATitleBar Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var titleBar = FindUIItem(searchCriteria, scope, timeout);
            return new QATitleBar(titleBar, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* TitleBars based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TitleBars</returns>
        public static List<QATitleBar> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QATitleBars = new List<QATitleBar>();
            var titleBars = FindUIItems(searchCriteria, scope, timeout);

            foreach (var titleBar in titleBars)
            {
                try
                {
                    QATitleBars.Add(new QATitleBar((TitleBar) titleBar, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QATitleBars;
        }

        public static QATitleBar Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new TitleBar(containerAE, new NullActionListener())
                : null;

            return new QATitleBar(matchingUIItem, friendlyName);
        }
    }
}