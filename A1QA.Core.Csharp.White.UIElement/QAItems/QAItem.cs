//  <copyright file="QAItem.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Xml;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.Basics.SystemUtilities;
using A1QA.Core.Csharp.White.UIElement.Extentions;
using A1QA.Core.Csharp.White.UIElement.Properties;
using A1QA.Core.Csharp.White.UIElement.xPathSearcher;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WPFUIItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Base class for all QAItems except for QAApplication and QAWindow
    /// </summary>
    /// <typeparam name="T">UIItem type</typeparam>
    public class QAItem<T> where T : UIItem
    {
        private T uiItem;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QAItem{T}" /> class
        /// </summary>
        /// <param name="uiItem">White UIItem</param>
        /// <param name="friendlyName">Friendly name for UIItem</param>
        protected internal QAItem(T uiItem, string friendlyName)
        {
            UIItem = uiItem;
            FriendlyName = friendlyName;
        }

        /// <summary>
        ///     returns tru if the uiItem exists
        /// </summary>
        public bool UIItemExists => uiItem != null;

        /// <summary>
        ///     Gets or sets White UIItem
        /// </summary>
        public T UIItem
        {
            get
            {
                if (uiItem == null)
                {
                    throw new AutomationException(string.Format(Resources.UIItemIsNullMsg, FriendlyName), string.Empty);
                }

                return uiItem;
            }

            set { uiItem = value; }
        }

        /// <summary>
        ///     Gets or sets a friendly name to easily recognize a UIItem
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        ///     Gets a value indicating whether a UIItem exists
        /// </summary>
        public bool Exists
        {
            get
            {
                Report.Output(Report.Level.Debug, Resources.UIItemExistsMsg);

                try
                {
                    if (!UIItem.Visible)
                    {
                        Report.Output(
                            Report.Level.Debug,
                            Resources.UIItemExistsOffScreenMsg,
                            UIItem.PrimaryIdentification,
                            UIItem.Name,
                            UIItem.GetType().BaseType.ToString());

                        return false;
                    }
                }
                catch (AutomationException ex)
                {
                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemExistsFalseMsg,
                        ex.Message);

                    return false;
                }

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemExistsTrueMsg,
                    UIItem.PrimaryIdentification,
                    UIItem.Name,
                    UIItem.GetType().BaseType.ToString());

                return true;
            }
        }

        /// <summary>
        ///     Gets LegacyIAccessible.Value property of UIItem
        ///     via its AutomationElement
        /// </summary>
        public string Value
        {
            get
            {
                var value = string.Empty;
                var isAvailable = (bool) UIItem.AutomationElement.GetCurrentPropertyValue(
                    AutomationElement.IsValuePatternAvailableProperty);

                if (isAvailable)
                {
                    var valuePattern = (ValuePattern) UIItem.AutomationElement.GetCurrentPattern(
                        ValuePattern.Pattern);
                    value = valuePattern.Current.Value;
                }

                return value;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether a UIItem is visible
        /// </summary>
        public bool IsVisible => UIItem.Visible;

        /// <summary>
        ///     Gets HelpText property of UIItem
        /// </summary>
        public string HelpText => UIItem.HelpText;

        /// <summary>
        ///     Gets the Name property of UIItem
        /// </summary>
        public virtual string Name => UIItem.Name;

        /// <summary>
        ///     Gets location of UIItem
        /// </summary>
        public Point Location => UIItem.Location;

        /// <summary>
        ///     Gets bounding rectangle for UIItem
        /// </summary>
        public Rect Bounds => UIItem.Bounds;

        /// <summary>
        ///     Gets a value indicating whether a UIItem is focussed
        /// </summary>
        public bool IsFocussed => UIItem.IsFocussed;

        internal static AutomationElement GetByXPath(string xpath, AutomationElement scope = null, int timeout = 0)
        {
            var topNodesList = scope == null
                ? WorkSpace.XmlDoc.SelectNodes(xpath)
                : scope.XmlHierarhy().SelectNodes(xpath);

            QAAssert.IsNotNull(topNodesList);

            switch (topNodesList.Count)
            {
                case 0:
                    return null;

                case 1:
                    return FindAEwithTimeout(topNodesList[0], xpath, timeout);

                default:
                    throw new NullReferenceException(
                        "There are multiple mathing nodes or node is missed. Check your xPath.");
            }
        }

        /// <summary>
        ///     DoubleClick UIItem
        /// </summary>
        public void DoubleClick()
        {
            ReportAction("DoubleClick");
            CaptureImage();
            UIItem.DoubleClick();
        }

        /// <summary>
        ///     DoubleClick UIItem with an offset on the X and Y axis (this uses cursor)
        /// </summary>
        /// <param name="offsetX">The amount of pixels to offset the UIItem's X coordinate (horizontal)</param>
        /// <param name="offsetY">The amount of pixels to offset the UIItem's Y coordinate (vertical)</param>
        public void DoubleClick(int offsetX, int offsetY)
        {
            ReportActionValue("DoubleClick", string.Format("offsetX = {0}, offsetY = {1}", offsetX, offsetY));
            CaptureImage();
            var coordinateX = Convert.ToInt32(UIItem.Location.X) + offsetX;
            var coordinateY = Convert.ToInt32(UIItem.Location.Y) + offsetY;
            QAMouse.Click(new Point(coordinateX, coordinateY));
            QAMouse.Click(new Point(coordinateX, coordinateY));
        }

        /// <summary>
        ///     RightClick UIItem
        /// </summary>
        public void RightClick()
        {
            ReportAction("RightClick");
            CaptureImage();
            UIItem.RightClick();
        }

        /// <summary>
        ///     RightClick UIItem with an offset on the X and Y axis (this uses cursor)
        /// </summary>
        /// <param name="offsetX">The amount of pixels to offset the UIItem's X coordinate (horizontal)</param>
        /// <param name="offsetY">The amount of pixels to offset the UIItem's Y coordinate (vertical)</param>
        public void RightClick(int offsetX, int offsetY)
        {
            ReportActionValue("RightClick", string.Format("offsetX = {0}, offsetY = {1}", offsetX, offsetY));
            CaptureImage();
            var coordinateX = Convert.ToInt32(UIItem.Location.X) + offsetX;
            var coordinateY = Convert.ToInt32(UIItem.Location.Y) + offsetY;
            QAMouse.RightClick(new Point(coordinateX, coordinateY));
        }

        /// <summary>
        ///     Click UIItem (this uses cursor to click UIItem).
        ///     Always try to use this method instead of RaiseClickEvent() because this is a closer
        ///     implementation to how a user would normally click a UIItem.
        /// </summary>
        public void Click()
        {
            ReportAction("Click");
            CaptureImage();
            UIItem.Click();
        }

        /// <summary>
        ///     Click UIItem with an offset on the X and Y axis (this uses cursor)
        /// </summary>
        /// <param name="offsetX">The amount of pixels to offset the UIItem's X coordinate (horizontal)</param>
        /// <param name="offsetY">The amount of pixels to offset the UIItem's Y coordinate (vertical)</param>
        public void Click(int offsetX, int offsetY)
        {
            ReportActionValue("Click", string.Format("offsetX = {0}, offsetY = {1}", offsetX, offsetY));
            CaptureImage();
            var coordinateX = Convert.ToInt32(UIItem.Location.X) + offsetX;
            var coordinateY = Convert.ToInt32(UIItem.Location.Y) + offsetY;
            QAMouse.Click(new Point(coordinateX, coordinateY));
        }

        /// <summary>
        ///     RaiseClickEvent upon UIItem (this triggers a click event without the use of the cursor).
        ///     Only call this method when Click() does not work.
        /// </summary>
        public void RaiseClickEvent()
        {
            ReportAction("RaiseClickEvent");
            CaptureImage();
            UIItem.RaiseClickEvent();
        }

        /// <summary>
        ///     Capture image of UIItem if action is acceptance criteria or debug execution enabled
        /// </summary>
        public void CaptureImage()
        {
            QAScreenCapture.CaptureImage(UIItem);
        }

        /// <summary>
        ///     Output debug information about a UIItem
        /// </summary>
        public void OutputDebugInfo()
        {
            Report.Output(
                Report.Level.Debug,
                Resources.UIItemDebugMsg,
                UIItem.PrimaryIdentification,
                UIItem.Name,
                UIItem.GetType().BaseType.ToString());
        }

        /// <summary>
        ///     Gets parent UIItem of child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <returns>Parent UIItem</returns>
        protected static T GetParentUIItem(UIItem child)
        {
            return child.GetParent<T>();
        }

        /// <summary>
        ///     Find a UIItem (of a particular type) based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItem</returns>
        protected static T FindUIItem(
            SearchCriteria searchCriteria,
            UIItem scope,
            int timeout)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window;
            }

            Report.Output(
                Report.Level.Debug,
                Resources.UIItemFindUsingSearchCriteriaMsg,
                searchCriteria.ToString(),
                typeof(T).ToString(),
                scope.PrimaryIdentification);

            T matchingUIItem = null;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                stopwatch.Start();
                matchingUIItem = scope.Get<T>(searchCriteria);
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemFoundMsg,
                    elapsedTime);
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemNotFoundMsg,
                    elapsedTime,
                    ex.Message);
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindUIItemTimeout();
                }
            }

            return matchingUIItem;
        }

        /// <summary>
        ///     Find a UIItem (of a particular type) based on SearchCriteria and ExtraCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="automationProperty">AutomationElement property</param>
        /// <param name="automationPropertyValue">Value of AutomationElement property</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItem</returns>
        protected static T FindUIItem(
            SearchCriteria searchCriteria,
            AutomationProperty automationProperty,
            object automationPropertyValue,
            UIItem scope,
            int timeout)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window;
            }

            Report.Output(
                Report.Level.Debug,
                Resources.UIItemFindUsingExtraCriteriaMsg,
                searchCriteria.ToString(),
                typeof(T).ToString(),
                automationProperty.ProgrammaticName,
                automationPropertyValue,
                scope.PrimaryIdentification);

            T matchingUIItem = null;
            var hasFound = true;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                var index = 0;
                stopwatch.Start();

                do
                {
                    matchingUIItem = scope.Get<T>(searchCriteria.AndIndex(index));
                    index++;

                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemCheckUsingExtraCriteriaMsg,
                        matchingUIItem.PrimaryIdentification,
                        matchingUIItem.Name);
                } while (!matchingUIItem.ValueOfEquals(automationProperty, automationPropertyValue));
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemNotFoundMsg,
                    elapsedTime,
                    ex.Message);

                hasFound = false;
                matchingUIItem = null;
            }
            finally
            {
                if (hasFound)
                {
                    elapsedTime = stopwatch.Elapsed;
                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemFoundMsg,
                        elapsedTime);
                }

                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindUIItemTimeout();
                }
            }

            return matchingUIItem;
        }

        protected static AutomationElement FindUIItem(Condition condition, TreeScope treeScope, UIItem scope = null, int timeout = 0)
        {
            scope = scope ?? WorkSpace.MainWindow.Window;

            Report.Output(Report.Level.Debug, Resources.UIItemFindUsingConditionMsg, condition.ToString(),
                typeof(T).ToString(), treeScope.ToString(), scope.PrimaryIdentification);

            return WaitForAE(() => scope.AutomationElement.FindFirst(treeScope, condition), timeout);
        }

        private static AutomationElement FindAEwithTimeout(XmlNode node, string xPath, int timeout)
        {
            WhiteConfigHelper.ResetFindUIItemTimeout();
            timeout = timeout == 0
                ? WhiteConfigHelper.OriginalFindUIItemTimeout
                : timeout;

            return WaitForAE(() => XmlSearcher.XmlNodeToAutomationElement(node, xPath), timeout);
        }

        private static AutomationElement WaitForAE(Func<AutomationElement> functionToGetAE, int timeout)
        {
            WhiteConfigHelper.ResetFindUIItemTimeout();
            timeout = timeout == 0
                ? WhiteConfigHelper.OriginalFindUIItemTimeout
                : timeout;

            AutomationElement matchingAE = null;

            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                do
                {
                    matchingAE = functionToGetAE();
                } while (matchingAE == null && stopwatch.ElapsedMilliseconds < timeout);
            }

            catch (AutomationException ex)
            {
                Report.Output(Report.Level.Debug, Resources.UIItemNotFoundMsg, stopwatch.Elapsed, ex.Message);
            }

            finally
            {
                if (matchingAE != null)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemFoundMsg, stopwatch.Elapsed);
                    Report.Output(Report.Level.Debug, Resources.UIItemCheckUsingExtraCriteriaMsg,
                        matchingAE.Current.AutomationId, matchingAE.Current.Name);
                }

                stopwatch.Stop();
            }

            return matchingAE;
        }

        /// <summary>
        ///     Find *multiple* UIItems based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching UIItems</returns>
        protected static IUIItem[] FindUIItems(
            SearchCriteria searchCriteria,
            UIItem scope,
            int timeout)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window;
            }

            Report.Output(
                Report.Level.Debug,
                Resources.UIItemMultipleFindUsingSearchCriteriaMsg,
                searchCriteria.ToString(),
                typeof(T).ToString(),
                scope.PrimaryIdentification);

            IUIItem[] matchingUIItems = null;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                stopwatch.Start();
                matchingUIItems = scope.GetMultiple(searchCriteria);
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemMultipleFoundMsg,
                    matchingUIItems.Length.ToString(),
                    elapsedTime);
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemMultipleNotFoundMsg,
                    elapsedTime,
                    ex.Message);
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindUIItemTimeout();
                }
            }

            return matchingUIItems;
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
        protected static List<IUIItem> FindUIItems(List<Tuple<AutomationProperty, object>> propertiesList,
            AutomationElement scope, int timeout)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window.AutomationElement;
            }

            Report.Output(
                Report.Level.Debug,
                Resources.UIItemMultipleFindUsingAutomationPropertyCriteriaMsg,
                scope);

            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                var listPropertyCondition = new List<PropertyCondition>();

                foreach (var tplAutomationProperty in propertiesList)
                {
                    listPropertyCondition.Add(new PropertyCondition(tplAutomationProperty.Item1,
                        tplAutomationProperty.Item2));
                }
                var elementConditions = new AndCondition(listPropertyCondition.ToArray());

                stopwatch.Start();
                var collection = scope.FindAll(TreeScope.Descendants, elementConditions);
                elapsedTime = stopwatch.Elapsed;

                if (collection.Count == 0)
                {
                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemIsNullMsg,
                        collection.Count.ToString(),
                        elapsedTime);
                    return null;
                }
                var matchingUIItems = new List<IUIItem>();

                foreach (AutomationElement child in collection)
                {
                    var child1 = new UIItem(child, new NullActionListener());
                    matchingUIItems.Add(child1);
                }

                return matchingUIItems;
            }
            catch (ElementNotAvailableException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemMultipleNotFoundMsg,
                    elapsedTime,
                    ex.Message);

                return null;
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindUIItemTimeout();
                }
            }
        }

        /// <summary>
        ///     Write the action (that was invoked) to output
        /// </summary>
        /// <param name="action">Name of action</param>
        protected void ReportAction(string action)
        {
            Report.Output(
                Report.Level.Action,
                Resources.UIItemActionMsg,
                action,
                FriendlyName,
                UIItem.PrimaryIdentification,
                UIItem.Name,
                UIItem.GetType().BaseType.ToString());
        }

        /// <summary>
        ///     Write the action (that was invoked) to output.
        ///     Also write the value being used.
        /// </summary>
        /// <param name="action">Name of action</param>
        /// <param name="value">Value being used by action</param>
        protected void ReportActionValue(string action, object value)
        {
            Report.Output(
                Report.Level.Action,
                Resources.UIItemActionValueMsg,
                action,
                value,
                FriendlyName,
                UIItem.PrimaryIdentification,
                UIItem.Name,
                UIItem.GetType().BaseType.ToString());
        }

        /// <summary>
        ///     Construct friendly message
        /// </summary>
        /// <param name="friendlyFormatMessage">Friendly format message</param>
        /// <param name="friendlyFormatMessageArgs">Friendly format message arguments</param>
        /// <returns>Friendly message</returns>
        protected string ConstructFriendlyMessage(string friendlyFormatMessage,
            params object[] friendlyFormatMessageArgs)
        {
            var friendlyMessage = string.Empty;

            if (FriendlyName != string.Empty)
            {
                // Add QAItem.FriendlyName to beginning of friendly message arguments
                // and then format friendly message
                var args = new[] {FriendlyName}.Concat(friendlyFormatMessageArgs).ToArray();
                friendlyMessage = string.Format(
                    friendlyFormatMessage,
                    args);
            }

            return friendlyMessage;
        }

        /// <summary>
        ///     Find a UIItem (of a particular type) by Name and ClassName
        /// </summary>
        /// <param name="nameProperty">value for AutomationElement.NameProperty</param>
        /// <param name="classProperty">value for AutomationElement.ClassNameProperty</param>
        /// <param name="friendlyName">friendly name for UIItem</param>
        /// <param name="scope">scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>matching UIItem</returns>
        protected static UIItem FindUIItem(
            string nameProperty,
            string classProperty,
            string friendlyName,
            AutomationElement scope,
            int timeout)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window.AutomationElement;
            }

            Report.Output(
                Report.Level.Debug,
                Resources.UIItemFindUsingSearchCriteriaMsg,
                AutomationElement.NameProperty,
                nameProperty,
                AutomationElement.ClassNameProperty,
                classProperty,
                scope);

            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                Condition condition = new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, nameProperty),
                    new PropertyCondition(AutomationElement.ClassNameProperty, classProperty));

                stopwatch.Start();
                var collection = scope.FindAll(TreeScope.Descendants, condition);
                elapsedTime = stopwatch.Elapsed;

                if (collection.Count == 0)
                {
                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemIsNullMsg,
                        collection.Count.ToString(),
                        elapsedTime);
                    return null;
                }

                if (collection.Count > 1)
                {
                    Report.Output(
                        Report.Level.Debug,
                        Resources.UIItemMultipleFoundMsg,
                        collection.Count.ToString(),
                        elapsedTime);
                }

                return new UIItem(collection[0], new NullActionListener());
            }
            catch (ElementNotAvailableException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(
                    Report.Level.Debug,
                    Resources.UIItemMultipleNotFoundMsg,
                    elapsedTime,
                    ex.Message);

                return null;
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindUIItemTimeout();
                }
            }
        }
    }
}