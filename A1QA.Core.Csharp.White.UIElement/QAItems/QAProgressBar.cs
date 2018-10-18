//  <copyright file="QAProgressBar.cs" company="A1QA">
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
    ///     Our own implementation of a ProgressBar that extends the functionality of
    ///     White ProgressBar
    /// </summary>
    public class QAProgressBar : QAItem<ProgressBar>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAProgressBar" /> class
        /// </summary>
        /// <param name="progressBar">White ProgressBar</param>
        /// <param name="friendlyName">Friendly name for ProgressBar</param>
        public QAProgressBar(ProgressBar progressBar, string friendlyName) : base(progressBar, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent ProgressBar of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for ProgressBar</param>
        /// <returns>Parent ProgressBar</returns>
        public static QAProgressBar GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAProgressBar(parent, friendlyName);
        }

        public static QAProgressBar Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new ProgressBar(containerAE, new NullActionListener())
                : null;

            return new QAProgressBar(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a ProgressBar based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for ProgressBar</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ProgressBar</returns>
        public static QAProgressBar Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var progressBar = FindUIItem(searchCriteria, scope, timeout);
            return new QAProgressBar(progressBar, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* ProgressBars based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ProgressBars</returns>
        public static List<QAProgressBar> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAProgressBars = new List<QAProgressBar>();
            var progressBars = FindUIItems(searchCriteria, scope, timeout);

            foreach (var progressBar in progressBars)
            {
                try
                {
                    QAProgressBars.Add(new QAProgressBar((ProgressBar) progressBar, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAProgressBars;
        }
    }
}