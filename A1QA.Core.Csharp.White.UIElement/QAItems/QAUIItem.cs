//  <copyright file="QAUIItem.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a UIItem that extends the functionality of
    ///     White UIItem
    /// </summary>
    public class QAUIItem : QAItem<UIItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAUIItem" /> class
        /// </summary>
        /// <param name="uiItem">White UIItem</param>
        /// <param name="friendlyName">Friendly name for UIItem</param>
        public QAUIItem(UIItem uiItem, string friendlyName) : base(uiItem, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent UIItem of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for parent UIItem</param>
        /// <returns>Parent UIItem</returns>
        public static QAUIItem GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAUIItem(parent, friendlyName);
        }

        /// <summary>
        ///     Get a UIItem based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for UIItem</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItem</returns>
        public static QAUIItem Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var uiItem = FindUIItem(searchCriteria, scope, timeout);
            return new QAUIItem(uiItem, friendlyName);
        }

        /// <summary>
        ///     Get a Button based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="automationProperty">AutomationElement property</param>
        /// <param name="automationPropertyValue">Value of AutomationElement property</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Button</returns>
        public static QAUIItem Get(SearchCriteria searchCriteria, AutomationProperty automationProperty, object automationPropertyValue, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var button = FindUIItem(searchCriteria, automationProperty, automationPropertyValue, scope, timeout);

            return new QAUIItem(button, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* UIItems based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItems</returns>
        public static List<QAUIItem> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAUIItems = new List<QAUIItem>();
            var uiItems = FindUIItems(searchCriteria, scope, timeout);

            foreach (var uiItem in uiItems)
            {
                try
                {
                    QAUIItems.Add(new QAUIItem((UIItem) uiItem, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAUIItems;
        }

        /// <summary>
        ///     Get a UIItem based on Name and ClassName
        /// </summary>
        /// <param name="nameProperty">value for AutomationElement.NameProperty</param>
        /// <param name="classProperty">value for AutomationElement.ClassNameProperty</param>
        /// <param name="friendlyName">friendly name for UIItem</param>
        /// <param name="scope">scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>matching UIItem</returns>
        public static QAUIItem Get(string nameProperty, string classProperty, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var uiItem = FindUIItem(nameProperty, classProperty, friendlyName, scope, timeout);
            return new QAUIItem(uiItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* UIItems based on AutomationProperties conditions
        /// </summary>
        /// <param name="propertiesList">
        ///     A 2-tuple where T1 is AutomationProperty and T2 is the value of AutomationElement property.
        ///     Supports a combination of two or more PropertyConditions objects
        /// </param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItems</returns>
        public static List<QAUIItem> GetMultiple(List<Tuple<AutomationProperty, object>> propertiesList, AutomationElement scope = null, int timeout = 0)
        {
            var QAUIItems = new List<QAUIItem>();
            var uiItems = FindUIItems(propertiesList, scope, timeout);
            foreach (var uiItem in uiItems)
            {
                try
                {
                    QAUIItems.Add(new QAUIItem((UIItem) uiItem, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAUIItems;
        }
    }
}