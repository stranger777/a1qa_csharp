//  <copyright file="QAContainer.cs" company="A1QA">
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
    public class QAContainer : QAItem<CustomItem>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAContainer" /> class
        /// </summary>
        /// <param name="customItem">Custom UIItem</param>
        /// <param name="friendlyName">Friendly name for Container</param>
        public QAContainer(CustomItem customItem, string friendlyName) : base(customItem, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent Container of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Container</param>
        /// <returns>Parent Container</returns>
        public static QAContainer GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAContainer(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Container based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Container</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Container</returns>
        public static QAContainer Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var container = FindUIItem(searchCriteria, scope, timeout);
            return new QAContainer(container, friendlyName);
        }

        /// <summary>
        ///     Get a Container based on Automation Element locators
        /// </summary>
        /// <param name="condition">Automation Element conditions</param>
        /// <param name="treeScope">Search Depth</param>
        /// <param name="friendlyName">Formal Name of the element</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Container</returns>
        public static QAContainer Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var containerAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = containerAE != null
                ? new CustomItem(containerAE, new NullActionListener())
                : null;

            return new QAContainer(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a Container based on xPath
        /// </summary>
        /// <param name="xPath">xPath to the element</param>
        /// <param name="friendlyName">Formal Name of the element</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Container</returns>
        public static QAContainer Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new CustomItem(containerAE, new NullActionListener())
                : null;

            return new QAContainer(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Containers based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Containers</returns>
        public static List<QAContainer> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAContainers = new List<QAContainer>();
            var containers = FindUIItems(searchCriteria, scope, timeout);

            foreach (var container in containers)
            {
                try
                {
                    QAContainers.Add(new QAContainer((CustomItem) container, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAContainers;
        }
    }
}