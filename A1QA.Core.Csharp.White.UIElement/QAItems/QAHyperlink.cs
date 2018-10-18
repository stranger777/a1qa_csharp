//  <copyright file="QAHyperlink.cs" company="A1QA">
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
    ///     Our own implementation of a Hyperlink that extends the functionality of
    ///     White Hyperlink
    /// </summary>
    public class QAHyperlink : QAItem<Hyperlink>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAHyperlink" /> class
        /// </summary>
        /// <param name="hyperlink">White Hyperlink</param>
        /// <param name="friendlyName">Friendly name for Hyperlink</param>
        public QAHyperlink(Hyperlink hyperlink, string friendlyName) : base(hyperlink, friendlyName)
        {
        }

        public static QAHyperlink Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Hyperlink(containerAE, new NullActionListener())
                : null;

            return new QAHyperlink(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a parent Hyperlink of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Hyperlink</param>
        /// <returns>Parent Hyperlink</returns>
        public static QAHyperlink GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAHyperlink(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Hyperlink based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Hyperlink</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Hyperlink</returns>
        public static QAHyperlink Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var hyperlink = FindUIItem(searchCriteria, scope, timeout);
            return new QAHyperlink(hyperlink, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Hyperlinks based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Hyperlinks</returns>
        public static List<QAHyperlink> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAHyperlinks = new List<QAHyperlink>();
            var hyperlinks = FindUIItems(searchCriteria, scope, timeout);

            foreach (var hyperlink in hyperlinks)
            {
                try
                {
                    QAHyperlinks.Add(new QAHyperlink((Hyperlink) hyperlink, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAHyperlinks;
        }
    }
}