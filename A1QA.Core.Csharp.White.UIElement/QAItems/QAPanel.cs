//  <copyright file="QAPanel.cs" company="A1QA">
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
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a Panel that extends the functionality of
    ///     White Panel
    /// </summary>
    public class QAPanel : QAItem<Panel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAPanel" /> class
        /// </summary>
        /// <param name="panel">White Panel</param>
        /// <param name="friendlyName">Friendly name for Panel</param>
        public QAPanel(Panel panel, string friendlyName) : base(panel, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent Panel of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Panel</param>
        /// <returns>Parent Panel</returns>
        public static QAPanel GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAPanel(parent, friendlyName);
        }

        public static QAPanel Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Panel(containerAE, new NullActionListener())
                : null;

            return new QAPanel(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get a Panel based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Panel</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Panel</returns>
        public static QAPanel Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var panel = FindUIItem(searchCriteria, scope, timeout);
            return new QAPanel(panel, friendlyName);
        }

        /// <summary>
        ///     Get a Panel based on AutomationElement condition
        /// </summary>
        /// <param name="condition">AutomationElement identification conditions</param>
        /// <param name="treeScope"> Depth search level for the UI Automation tree</param>
        /// <param name="friendlyName">Friendly name for Button</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Panel</returns>
        public static QAPanel Get(Condition condition, TreeScope treeScope, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var panelAE = FindUIItem(condition, treeScope, scope, timeout);
            var matchingUIItem = panelAE != null
                ? new Panel(panelAE, new NullActionListener())
                : null;

            return new QAPanel(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Panels based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Panels</returns>
        public static List<QAPanel> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAPanels = new List<QAPanel>();
            var panels = FindUIItems(searchCriteria, scope, timeout);

            foreach (var panel in panels)
            {
                try
                {
                    QAPanels.Add(new QAPanel((Panel) panel, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAPanels;
        }

        /// <summary>
        ///     Scroll down into a panel
        /// </summary>
        public void ScrollDown()
        {
            if (UIItem.ScrollBars.Vertical.IsScrollable)
            {
                UIItem.ScrollBars.Vertical.SetToMinimum();
                var position = UIItem.ScrollBars.Vertical.MinimumValue;

                while (position < UIItem.ScrollBars.Vertical.MaximumValue)
                {
                    UIItem.ScrollBars.Vertical.ScrollDown();
                    position++;
                }
            }
        }

        /// <summary>
        ///     Set value and then verify
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="shouldClick">Should click before setting text (yes/no)</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void SetValue(string value, bool shouldVerify = true)
        {
            ReportActionValue("SetValue", value);
            UIItem.Click();

            WorkSpace.MainWindow.Keyboard.Enter(value);
            WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyDataGridCellSetMsg, value);

                QAAssert.Contains(Value, value, friendlyMessage);
            }
        }

        public QAImage GetImage()
        {
            return QAImage.Get(SearchCriteria.ByControlType(ControlType.Image), "Image", UIItem);
        }

        public QACheckBox GetCheckBox()
        {
            return QACheckBox.Get(SearchCriteria.ByControlType(ControlType.CheckBox), "CheckBox", UIItem);
        }
    }
}