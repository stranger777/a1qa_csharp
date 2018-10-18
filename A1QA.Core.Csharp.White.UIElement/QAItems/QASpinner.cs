//  <copyright file="QASpinner.cs" company="A1QA">
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
    ///     Our own implementation of a Spinner that extends the functionality of
    ///     White Spinner
    /// </summary>
    public class QASpinner : QAItem<Spinner>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QASpinner" /> class
        /// </summary>
        /// <param name="spinner">White Spinner</param>
        /// <param name="friendlyName">Friendly name for Spinner</param>
        public QASpinner(Spinner spinner, string friendlyName) : base(spinner, friendlyName)
        {
        }

        public static QASpinner Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Spinner(containerAE, new NullActionListener())
                : null;

            return new QASpinner(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a parent Spinner of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Spinner</param>
        /// <returns>Parent Spinner</returns>
        public static QASpinner GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QASpinner(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Spinner based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Spinner</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Spinner</returns>
        public static QASpinner Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var spinner = FindUIItem(searchCriteria, scope, timeout);
            return new QASpinner(spinner, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Spinners based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Spinners</returns>
        public static List<QASpinner> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QASpinners = new List<QASpinner>();
            var spinners = FindUIItems(searchCriteria, scope, timeout);

            foreach (var spinner in spinners)
            {
                try
                {
                    QASpinners.Add(new QASpinner((Spinner) spinner, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QASpinners;
        }
    }
}