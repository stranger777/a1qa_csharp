//  <copyright file="QAWindow.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using System.Xml;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.Basics.SystemUtilities;
using A1QA.Core.Csharp.White.UIElement.Extentions;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems
{
    /// <summary>
    ///     Our own implementation of a Window that extends the functionality of
    ///     White Window
    /// </summary>
    public class QAWindow
    {
        private Window window;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QAWindow" /> class
        /// </summary>
        /// <param name="window">White Window</param>
        public QAWindow(Window window)
        {
            Window = window;
        }

        /// <summary>
        ///     Gets or sets White Window
        /// </summary>
        public Window Window
        {
            get
            {
                if (window == null)
                {
                    throw new AutomationException(Resources.WindowIsNullMsg, string.Empty);
                }

                return window;
            }

            set { window = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether a Window exists
        /// </summary>
        public bool Exists
        {
            get
            {
                Report.Output(Report.Level.Debug, Resources.WindowExistsMsg);

                try
                {
                    if (!Window.Visible)
                    {
                        Report.Output(Report.Level.Debug, Resources.WindowExistsOffScreenMsg, Window.PrimaryIdentification, Window.Name);

                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is AutomationException || ex is ElementNotAvailableException)
                    {
                        Report.Output(Report.Level.Debug, Resources.WindowExistsFalseMsg, ex.Message);

                        return false;
                    }
                }

                Report.Output(Report.Level.Debug, Resources.WindowExistsTrueMsg, Window.PrimaryIdentification, Window.Name);

                return true;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether a Window is visible
        /// </summary>
        public bool IsVisible => Window.Visible;

        /// <summary>
        ///     Gets the name of a Window
        /// </summary>
        public string Name => Window.Name;

        /// <summary>
        ///     Gets the title of a Window
        /// </summary>
        public string Title => Window.Title;

        /// <summary>
        ///     Gets the Keyboard property of a Window
        /// </summary>
        public AttachedKeyboard Keyboard => Window.Keyboard;

        /// <summary>
        ///     Gets a value indicating whether a Window can be moved
        /// </summary>
        public bool CanMove => TransformPattern.Current.CanMove;

        /// <summary>
        ///     Gets a value indicating whether a Window can be resized
        /// </summary>
        public bool CanResize => TransformPattern.Current.CanResize;

        /// <summary>
        ///     Gets a value indicating whether a Window can be rotated
        /// </summary>
        public bool CanRotate => TransformPattern.Current.CanRotate;

        /// <summary>
        ///     Gets the TransformPattern which is used to support controls that can be moved, resized
        ///     or rotated in a two-dimensional space
        /// </summary>
        private TransformPattern TransformPattern => (TransformPattern) Window.AutomationElement.GetCurrentPattern(TransformPattern.Pattern);

        /// <summary>
        ///     Get a Window based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">Window identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Window</returns>
        public static QAWindow GetWindow(SearchCriteria searchCriteria, Application scope = null, int timeout = 0)
        {
            var window = FindWindow(searchCriteria, scope, timeout);
            return new QAWindow(window);
        }

        /// <summary>
        ///     Get a ModalWindow based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">Window identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ModalWindow</returns>
        public static QAWindow GetModalWindow(SearchCriteria searchCriteria, Window scope = null, int timeout = 0)
        {
            var window = FindModalWindow(searchCriteria, scope, timeout);
            return new QAWindow(window);
        }

        /// <summary>
        ///     Get a ModalWindow based on title
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching ModalWindow</returns>
        public static QAWindow GetModalWindow(string title, Window scope = null, int timeout = 0)
        {
            var window = FindModalWindow(title, scope, timeout);
            return new QAWindow(window);
        }

        /// <summary>
        ///     Maximize a Window
        /// </summary>
        public void Maximize()
        {
            Report.Output(Report.Level.Debug, Resources.WindowMaximizeMsg, Window.Name);

            Window.Focus();
            var canMaximize = (bool) Window.AutomationElement.GetCurrentPropertyValue(WindowPattern.CanMaximizeProperty);
            if (Window.DisplayState != DisplayState.Maximized && canMaximize)
            {
                Window.DisplayState = DisplayState.Maximized;
            }
        }

        /// <summary>
        ///     Minimize a Window
        /// </summary>
        public void Minimize()
        {
            Window.Focus();
            Window.DisplayState = DisplayState.Minimized;
        }

        /// <summary>
        ///     Restore a Window
        /// </summary>
        public void Restore()
        {
            Window.Focus();
            Window.DisplayState = DisplayState.Restored;
        }

        /// <summary>
        ///     Moves a Window
        /// </summary>
        /// <param name="x">The absolute screen coordinates of the left side of the Window</param>
        /// <param name="y">The absolute screen coordinates of the top of the Window</param>
        public void Move(double x, double y)
        {
            if (CanMove)
            {
                TransformPattern.Move(x, y);
            }
        }

        /// <summary>
        ///     Resizes a Window
        /// </summary>
        /// <param name="width">The new width of the Window in pixels</param>
        /// <param name="height">The new height of the Window in pixels</param>
        public void Resize(double width, double height)
        {
            if (CanResize)
            {
                TransformPattern.Resize(width, height);
            }
        }

        /// <summary>
        ///     Rotates a Window
        /// </summary>
        /// <param name="degrees">
        ///     The number of degrees to rotate the Window.
        ///     Positive number for clockwise; negative number for counterclockwise
        /// </param>
        public void Rotate(double degrees)
        {
            if (CanRotate)
            {
                TransformPattern.Rotate(degrees);
            }
        }

        /// <summary>
        ///     Close a Window
        /// </summary>
        public void Close()
        {
            Report.Output(Report.Level.Debug, Resources.WindowCloseMsg, Window.Name);

            Window.Focus();
            Window.Close();
        }

        /// <summary>
        ///     Waits for this modal window to close. Times out after 30 seconds.
        /// </summary>
        public void WaitUntilModalWindowClosed()
        {
            Window.WaitTill(() => Window.IsClosed || !Exists || !IsVisible, TimeSpan.FromMilliseconds(30000));
        }

        /// <summary>
        ///     Find a Window based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">Window identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Window</returns>
        private static Window FindWindow(SearchCriteria searchCriteria, Application scope = null, int timeout = 0)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.Application.Application;
            }

            Report.Output(Report.Level.Debug, Resources.WindowFindUsingSearchCriteriaMsg, searchCriteria.ToString(), scope.Name);

            Window matchingWindow = null;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                stopwatch.Start();
                matchingWindow = scope.GetWindow(searchCriteria, InitializeOption.NoCache);
                elapsedTime = stopwatch.Elapsed;
                matchingWindow.WaitWhileBusy();

                Report.Output(Report.Level.Debug, Resources.WindowFoundMsg, elapsedTime);
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(Report.Level.Debug, Resources.WindowNotFoundMsg, elapsedTime, ex.Message);
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindWindowTimeout();
                }
            }

            return matchingWindow;
        }

        /// <summary>
        ///     Find a Modal Window based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">Window identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Window</returns>
        private static Window FindModalWindow(SearchCriteria searchCriteria, Window scope = null, int timeout = 0)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window;
            }

            Report.Output(Report.Level.Debug, Resources.ModalWindowFindUsingSearchCriteriaMsg, searchCriteria.ToString(), scope.PrimaryIdentification);

            Window matchingWindow = null;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                stopwatch.Start();
                matchingWindow = scope.ModalWindow(searchCriteria);
                elapsedTime = stopwatch.Elapsed;
                matchingWindow.WaitWhileBusy();

                Report.Output(Report.Level.Debug, Resources.ModalWindowFoundMsg, elapsedTime);
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(Report.Level.Debug, Resources.ModalWindowNotFoundMsg, elapsedTime, ex.Message);
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindWindowTimeout();
                }
            }

            return matchingWindow;
        }

        /// <summary>
        ///     Find a Modal Window based on SearchCriteria
        /// </summary>
        /// <param name="title">title of the window</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Window</returns>
        private static Window FindModalWindow(string title, Window scope = null, int timeout = 0)
        {
            if (timeout > 0)
            {
                WhiteConfigHelper.OriginalFindUIItemTimeout = timeout;
            }

            if (scope == null)
            {
                scope = WorkSpace.MainWindow.Window;
            }

            Report.Output(Report.Level.Debug, Resources.ModalWindowFindUsingTitleMsg, title, scope.PrimaryIdentification);

            Window matchingWindow = null;
            var stopwatch = new Stopwatch();
            var elapsedTime = new TimeSpan();

            try
            {
                stopwatch.Start();
                matchingWindow = scope.ModalWindow(title);
                elapsedTime = stopwatch.Elapsed;
                matchingWindow.WaitWhileBusy();

                Report.Output(Report.Level.Debug, Resources.ModalWindowFoundMsg, elapsedTime);
            }
            catch (AutomationException ex)
            {
                elapsedTime = stopwatch.Elapsed;

                Report.Output(Report.Level.Debug, Resources.ModalWindowNotFoundMsg, elapsedTime, ex.Message);
            }
            finally
            {
                stopwatch.Stop();

                if (timeout > 0)
                {
                    WhiteConfigHelper.ResetFindWindowTimeout();
                }
            }

            return matchingWindow;
        }

        /// <summary>
        ///     Enter Text from keyboard
        /// </summary>
        /// <param name="value">Value to enter</param>
        public static void EnterKeyboardValue(string value)
        {
            WorkSpace.MainWindow.Keyboard.Enter(value);
            WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(100);
        }

        public static List<Window> GetAllModals()
        {
            return WorkSpace.MainWindow.Window.ModalWindows();
        }

        /// <summary>
        ///     Press special keyboard key like Enter, Tab, Delete etc.
        /// </summary>
        /// <param name="specialKey">Special key to press</param>
        public static void PressSpecialKey(string specialKey)
        {
            switch (specialKey.ToUpper())
            {
                case "ENTER":
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    break;
                case "TAB":
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    break;
                case "ESC":
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.ESCAPE);
                    break;
                case "BACKSPACE":
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.BACKSPACE);
                    break;
                case "DELETE":
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE);
                    break;
                default:
                    WorkSpace.MainWindow.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
                    break;
            }
        }

        /// <summary>
        ///     Getting AutomationElement proerties and child AutomationElements..
        ///     Create XMLDocument of the AutomationElement hierarchy..
        /// </summary>
        public XmlDocument GetHierarchy()
        {
            return Window.AutomationElement.XmlHierarhy();
        }
    }
}