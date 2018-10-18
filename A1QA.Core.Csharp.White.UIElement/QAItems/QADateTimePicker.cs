//  <copyright file="QADateTimePicker.cs" company="A1QA">
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
    ///     Our own implementation of a DateTimePicker that extends the functionality of
    ///     White DateTimePicker
    /// </summary>
    public class QADateTimePicker : QAItem<DateTimePicker>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QADateTimePicker" /> class
        /// </summary>
        /// <param name="dateTimePicker">White DateTimePicker</param>
        /// <param name="friendlyName">Friendly name for DateTimePicker</param>
        public QADateTimePicker(DateTimePicker dateTimePicker, string friendlyName) : base(dateTimePicker, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent DateTimePicker of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for DateTimePicker</param>
        /// <returns>Parent DateTimePicker</returns>
        public static QADateTimePicker GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QADateTimePicker(parent, friendlyName);
        }

        /// <summary>
        ///     Get a DateTimePicker based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for DateTimePicker</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DateTimePicker</returns>
        public static QADateTimePicker Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var dateTimePicker = FindUIItem(searchCriteria, scope, timeout);
            return new QADateTimePicker(dateTimePicker, friendlyName);
        }

        public static QADateTimePicker Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new DateTimePicker(containerAE, new NullActionListener())
                : null;

            return new QADateTimePicker(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* DateTimePickers based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching DateTimePickers</returns>
        public static List<QADateTimePicker> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var qaDateTimePickers = new List<QADateTimePicker>();
            var dateTimePickers = FindUIItems(searchCriteria, scope, timeout);

            foreach (var dateTimePicker in dateTimePickers)
            {
                try
                {
                    qaDateTimePickers.Add(new QADateTimePicker((DateTimePicker) dateTimePicker, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return qaDateTimePickers;
        }
    }
}