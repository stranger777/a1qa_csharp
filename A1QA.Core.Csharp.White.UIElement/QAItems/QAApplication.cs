//  <copyright file="QAApplication.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems;
using TestStack.White;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of an Application that extends the functionality of
    ///     White Application
    /// </summary>
    public class QAApplication
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAApplication" /> class
        /// </summary>
        /// <param name="application">White Application</param>
        public QAApplication(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException("application");
            }
            Application = application;
        }

        /// <summary>
        ///     Gets or sets White Application
        /// </summary>
        public Application Application { get; set; }

        public QAWindow MainWindow
        {
            get
            {
                try
                {
                    var window = Application.GetWindows().FirstOrDefault();
                    while (window.IsClosed)
                    {
                        window = Application.GetWindows().FirstOrDefault();
                    }
                    return new QAWindow(window);
                }
                catch (ElementNotAvailableException)
                {
                    return MainWindow;
                }
            }
        }

        /// <summary>
        ///     Launch an application and then return its QAApplication object
        /// </summary>
        /// <param name="applicationFilePath">Path of executable</param>
        /// <returns>QAApplication object</returns>
        public static QAApplication Launch(string applicationFilePath)
        {
            var application = LaunchApplication(applicationFilePath);
            return new QAApplication(application);
        }

        /// <summary>
        ///     Attach an existing process to a QAApplication object and then return it
        /// </summary>
        /// <param name="processName">Name of existing process</param>
        /// <returns>QAApplication object</returns>
        public static QAApplication Attach(string processName)
        {
            var application = AttachProcess(processName);
            return new QAApplication(application);
        }

        /// <summary>
        ///     Attach an existing process to a QAApplication object and then return it
        /// </summary>
        /// <param name="processName">Name of existing process</param>
        /// <returns>QAApplication object</returns>
        public static QAApplication Attach(Process processName)
        {
            var application = AttachProcess(processName);
            return new QAApplication(application);
        }

        /// <summary>
        ///     Attach an existing process to a QAApplication object and then return it
        /// </summary>
        /// <param name="processId">Id of existing process</param>
        /// <returns>QAApplication object</returns>
        public static QAApplication Attach(int processId)
        {
            var application = AttachProcess(processId);
            return new QAApplication(application);
        }

        /// <summary>
        ///     Tries to find the main window, then close it.
        ///     If it hasn't closed in 5 seconds, kill the process.
        /// </summary>
        public void Close()
        {
            try
            {
                Report.Output(Report.Level.Debug, Resources.ApplicationCloseMsg, Application.Name);

                Application.Close();
            }
            catch (InvalidOperationException)
            {
                Report.Output(Report.Level.Debug, "App was already closed");
            }

            Report.Output(Report.Level.Debug, Resources.ApplicationCloseSuccessMsg);
        }

        /// <summary>
        ///     Runs the process identified by the executable and creates Application
        ///     object for this executable
        /// </summary>
        /// <param name="applicationFilePath">Path of executable</param>
        /// <returns>White Application object</returns>
        private static Application LaunchApplication(string applicationFilePath)
        {
            Report.Output(Report.Level.Debug, Resources.ApplicationLaunchMsg, applicationFilePath);
            Application application;

            try
            {
                var stopwatch = Stopwatch.StartNew();
                application = Application.Launch(applicationFilePath);

                Report.Output(Report.Level.Debug, Resources.ApplicationLaunchSuccessMsg, stopwatch.Elapsed);

                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                Report.Output(Report.Level.Debug, Resources.ApplicationLaunchFailureMsg, ex.Message);

                throw ex;
            }

            return application;
        }

        /// <summary>
        ///     Attaches with existing process
        /// </summary>
        /// <param name="processName">Name of existing process</param>
        /// <returns>White Application object</returns>
        private static Application AttachProcess(string processName)
        {
            Report.Output(Report.Level.Debug, Resources.ApplicationAttachMsg, processName);

            return Application.Attach(processName);
        }

        /// <summary>
        ///     Attaches with existing process
        /// </summary>
        /// <param name="processName">Name of existing process</param>
        /// <returns>White Application object</returns>
        private static Application AttachProcess(Process processName)
        {
            Report.Output(Report.Level.Debug, Resources.ApplicationAttachMsg, processName);

            return Application.Attach(processName);
        }

        /// <summary>
        ///     Attaches with existing process
        /// </summary>
        /// <param name="processId">Id of existing process</param>
        /// <returns>White Application object</returns>
        private static Application AttachProcess(int processId)
        {
            Report.Output(Report.Level.Debug, Resources.ApplicationAttachMsg, processId);

            return Application.Attach(processId);
        }
    }
}