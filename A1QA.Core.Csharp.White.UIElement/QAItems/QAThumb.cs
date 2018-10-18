//  <copyright file="QAThumb.cs" company="A1QA">
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
    ///     Our own implementation of a Thumb that extends the functionality of
    ///     White Thumb
    /// </summary>
    public class QAThumb : QAItem<Thumb>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAThumb" /> class
        /// </summary>
        /// <param name="thumb">White Thumb</param>
        /// <param name="friendlyName">Friendly name for Thumb</param>
        public QAThumb(Thumb thumb, string friendlyName) : base(thumb, friendlyName)
        {
        }

        public static QAThumb Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Thumb(containerAE, new NullActionListener())
                : null;

            return new QAThumb(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a parent Thumb of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Thumb</param>
        /// <returns>Parent Thumb</returns>
        public static QAThumb GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAThumb(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Thumb based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Thumb</returns>
        public static QAThumb Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var thumb = FindUIItem(searchCriteria, scope, timeout);
            return new QAThumb(thumb, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Thumbs based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Thumbs</returns>
        public static List<QAThumb> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAThumbs = new List<QAThumb>();
            var thumbs = FindUIItems(searchCriteria, scope, timeout);

            foreach (var thumb in thumbs)
            {
                try
                {
                    QAThumbs.Add(new QAThumb((Thumb) thumb, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAThumbs;
        }
    }
}