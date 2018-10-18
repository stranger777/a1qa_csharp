//  <copyright file="QASlider.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a Slider that extends the functionality of
    ///     White Slider
    /// </summary>
    public class QASlider : QAItem<Slider>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QASlider" /> class
        /// </summary>
        /// <param name="slider">White Slider</param>
        /// <param name="friendlyName">Friendly name for Slider</param>
        public QASlider(Slider slider, string friendlyName) : base(slider, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent Slider of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Slider</param>
        /// <returns>Parent Slider</returns>
        public static QASlider GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QASlider(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Slider based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Slider</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Slider</returns>
        public static QASlider Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var slider = FindUIItem(searchCriteria, scope, timeout);
            return new QASlider(slider, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Sliders based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Sliders</returns>
        public static List<QASlider> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QASliders = new List<QASlider>();
            var sliders = FindUIItems(searchCriteria, scope, timeout);

            foreach (var slider in sliders)
            {
                try
                {
                    QASliders.Add(new QASlider((Slider) slider, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QASliders;
        }
    }
}