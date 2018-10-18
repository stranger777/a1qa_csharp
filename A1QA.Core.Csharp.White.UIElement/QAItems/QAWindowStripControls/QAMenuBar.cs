//  <copyright file="QAMenuBar.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAMenuItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowStripControls;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowStripControls
{
    /// <summary>
    ///     Our own implementation of a MenuBar that extends the functionality of
    ///     White MenuBar
    /// </summary>
    public class QAMenuBar : QAItem<MenuBar>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAMenuBar" /> class
        /// </summary>
        /// <param name="menuBar">White MenuBar</param>
        /// <param name="friendlyName">Friendly name for MenuBar</param>
        public QAMenuBar(MenuBar menuBar, string friendlyName) : base(menuBar, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent MenuBar of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for MenuBar</param>
        /// <returns>Parent MenuBar</returns>
        public static QAMenuBar GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAMenuBar(parent, friendlyName);
        }

        /// <summary>
        ///     Get a MenuBar based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for MenuBar</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching MenuBar</returns>
        public static QAMenuBar Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var menuBar = FindUIItem(searchCriteria, scope, timeout);
            return new QAMenuBar(menuBar, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* MenuBars based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching MenuBars</returns>
        public static List<QAMenuBar> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAMenuBars = new List<QAMenuBar>();
            var menuBars = FindUIItems(searchCriteria, scope, timeout);

            foreach (var menuBar in menuBars)
            {
                try
                {
                    QAMenuBars.Add(new QAMenuBar((MenuBar) menuBar, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAMenuBars;
        }

        /// <summary>
        ///     Clicks a context menu item
        /// </summary>
        /// <param name="path">Path to the menu item</param>
        public void ClickMenuItem(string path)
        {
            ReportAction("ClickMenuItem");
            CaptureImage();
            UIItem.MenuItem(path).Click();
        }

        /// <summary>
        ///     Finds a context menu item
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Menu FindMenuItem(string path)
        {
            ReportAction("FindMenuItem");
            CaptureImage();
            var menuItem = UIItem.TopLevelMenu.Find(path);
            if (menuItem != null)
            {
                return menuItem;
            }
            return null;
        }

        /// <summary>
        ///     Finds a context sub menu item
        /// </summary>
        /// <param name="fullPath">Forward slash separated path to sub menu item e.g. "topLevelMenu/subMenu1/subMenu2"</param>
        /// <returns></returns>
        public Menu FindSubMenuItem(string fullPath)
        {
            var menuLabelList = fullPath.Split('/').ToList();
            var menuItem = FindMenuItem(menuLabelList.FirstOrDefault());

            // Remove the top level menu
            menuLabelList.RemoveAt(0);

            foreach (var menuLabel in menuLabelList)
            {
                menuItem = menuItem.SubMenu(menuLabel);
            }

            return menuItem;
        }

        /// <summary>
        ///     Checks the toggle button of a menu item
        /// </summary>
        /// <param name="path">Path to the menu item</param>
        public void CheckToggleItem(string path)
        {
            ReportAction("CheckToggleItem");
            CaptureImage();

            var menuItem = UIItem.MenuItem(path);
            var menuQAItem = new QAMenu(menuItem, "Menu item");
            var toggleState = menuQAItem.ToogleState;
            if (toggleState == ToggleState.Off)
            {
                menuItem.Click();
            }
        }

        /// <summary>
        ///     Unchecks the toggle button of a menu item
        /// </summary>
        /// <param name="path">Path to the menu item</param>
        public void UncheckToggleItem(string path)
        {
            ReportAction("UncheckToggleItem");
            CaptureImage();

            var menuItem = UIItem.MenuItem(path);
            var menuQAItem = new QAMenu(menuItem, "Menu item");
            var toggleState = menuQAItem.ToogleState;
            if (toggleState == ToggleState.On)
            {
                menuItem.Click();
            }
        }

        /// <summary>
        ///     Clicks a context menuBar item
        /// </summary>
        /// <param name="path">path to the context menu</param>
        public void Select(params string[] list)
        {
            for (var i = 0; i < list.Length; i++)
            {
                ClickMenuItem(list[i]);
            }
        }
    }
}