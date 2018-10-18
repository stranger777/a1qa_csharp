//  <copyright file="QAMenu.cs" company="A1QA">
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
using TestStack.White.UIItems.MenuItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAMenuItems
{
    /// <summary>
    ///     Our own implementation of a Menu that extends the functionality of
    ///     White Menu
    /// </summary>
    public class QAMenu : QAItem<Menu>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAMenu" /> class
        /// </summary>
        /// <param name="menu">White Menu</param>
        /// <param name="friendlyName">Friendly name for Menu</param>
        public QAMenu(Menu menu, string friendlyName) : base(menu, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating the ToggleState of a menu item
        /// </summary>
        public ToggleState ToogleState
        {
            get
            {
                var togglePattern = (TogglePattern) UIItem.AutomationElement.GetCurrentPattern(TogglePattern.Pattern);
                return togglePattern.Current.ToggleState;
            }
        }

        public QAMenu GetMainMenu()
        {
            return Get(SearchCriteria.All, "Menu");
        }

        /// <summary>
        ///     Get a parent Menu of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Menu</param>
        /// <returns>Parent Menu</returns>
        public static QAMenu GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAMenu(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Menu based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Menu</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Menu</returns>
        public static QAMenu Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var menu = FindUIItem(searchCriteria, scope, timeout);
            return new QAMenu(menu, friendlyName);
        }

        public static QAMenu Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var menuAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = menuAE != null
                ? new Menu(menuAE, new NullActionListener())
                : null;

            return new QAMenu(matchingUIItem, friendlyName);
        }

        public static QAMenu Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var matchingAE = GetByXPath(xPath);

            var matchingUIItem = matchingAE != null
                ? new Menu(matchingAE, new NullActionListener())
                : null;

            return new QAMenu(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Menus based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Menus</returns>
        public static List<QAMenu> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAMenus = new List<QAMenu>();
            var menus = FindUIItems(searchCriteria, scope, timeout);

            foreach (var menu in menus)
            {
                try
                {
                    QAMenus.Add(new QAMenu((Menu) menu, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAMenus;
        }

        /// <summary>
        ///     Clicks a context menu item
        /// </summary>
        /// <param name="path">Path to the menu item</param>
        public void ClickSelectItem(string path)
        {
            var menu = FindMenu(path);
            if (menu != null)
            {
                menu.Click();
            }
        }

        /// <summary>
        ///     Finds a context menu item
        /// </summary>
        /// <param name="path">Path to the menu item</param>
        public Menu FindMenu(string path)
        {
            for (var i = 0; i < UIItem.ChildMenus.Count; i++)
            {
                if (string.Equals(UIItem.ChildMenus[i].HelpText, path) || string.Equals(UIItem.ChildMenus[i].Name, path))
                {
                    return UIItem.ChildMenus[i];
                }
            }
            return null;
        }
    }
}