//  <copyright file="QAStatusStrip.cs" company="A1QA">
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
    ///     Our own implementation of a StatusStrip that extends the functionality of
    ///     White StatusStrip
    /// </summary>
    public class QAStatusStrip : QAItem<StatusStrip>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAStatusStrip" /> class
        /// </summary>
        /// <param name="statusStrip">White StatusStrip</param>
        /// <param name="friendlyName">Friendly name for StatusStrip</param>
        public QAStatusStrip(StatusStrip statusStrip, string friendlyName) : base(statusStrip, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent StatusStrip of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for StatusStrip</param>
        /// <returns>Parent StatusStrip</returns>
        public static QAStatusStrip GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAStatusStrip(parent, friendlyName);
        }

        /// <summary>
        ///     Get a StatusStrip based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for StatusStrip</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching StatusStrip</returns>
        public static QAStatusStrip Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var statusStrip = FindUIItem(searchCriteria, scope, timeout);
            return new QAStatusStrip(statusStrip, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* StatusStrips based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching StatusStrips</returns>
        public static List<QAStatusStrip> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAStatusStrips = new List<QAStatusStrip>();
            var statusStrips = FindUIItems(searchCriteria, scope, timeout);

            foreach (var statusStrip in statusStrips)
            {
                try
                {
                    QAStatusStrips.Add(new QAStatusStrip((StatusStrip) statusStrip, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAStatusStrips;
        }
    }
}