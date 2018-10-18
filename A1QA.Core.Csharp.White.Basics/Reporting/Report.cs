//  <copyright file="Report.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using OpenQA.Selenium;

namespace A1QA.Core.Csharp.White.Basics.Reporting
{
    /// <summary>
    ///     Reporting functionality
    /// </summary>
    public static class Report
    {
        /// <summary>
        ///     Specifies the level of reporting
        ///     Debug - message on the debug report
        ///     DebugImage - image of the UI object on the debug report
        ///     Info - message on the formal report. Use it to add context to the test
        ///     Action - message on the debug report about the action made on a UI object
        ///     Warning - message on the formal report. Use it to log a warning.
        ///     Error -  message of the formal report. Use it to log an error.
        ///     AssertPass - message on the debug report with details about the passed assertion
        ///     AssertFail - message on the debug report with details about the failed assertion
        ///     Pass - message on the formal report with passed acceptance criteria
        ///     Fail - message on the formal report with failed acceptance criteria
        ///     StepImage - image on the formal report as objective evidence for a step
        /// </summary>
        public enum Level
        {
            Debug,
            DebugImage,
            Info,
            Action,
            Warning,
            Error,
            AssertPass,
            AssertFail,
            Pass,
            Fail,
            StepImage
        }

        public static IWebDriver Driver { get; set; }

        /// <summary>
        ///     Write a message to the standard output stream
        /// </summary>
        /// <param name="message">Message to output</param>
        /// <param name="messageArgs">Message arguments</param>
        public static void Output(string message, params object[] messageArgs)
        {
            var formattedMessage = string.Format(message, messageArgs);
            Log.Trace(formattedMessage);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        ///     Write a message to the standard output stream
        /// </summary>
        /// <param name="level">Reporting level</param>
        /// <param name="message">Message to output</param>
        /// <param name="messageArgs">Message arguments</param>
        public static void Output(Level level, string message, params object[] messageArgs)
        {
            var taggedMessage = AssignTags(level, message);
            Output(taggedMessage, messageArgs);
        }

        /// <summary>
        ///     Gets report level for image
        /// </summary>
        /// <param name="isStepImage">Whether or not image is step image</param>
        /// <returns>Report level for image</returns>
        public static Level GetImageReportLevel(bool isStepImage)
        {
            if (isStepImage)
            {
                return Level.StepImage;
            }

            return Level.DebugImage;
        }

        /// <summary>
        ///     Write start time to the standard output stream
        /// </summary>
        public static void RecordStartTime()
        {
            Output("[start]{0}[/start]", DateTime.Now.ToString());
        }

        /// <summary>
        ///     Write finish time to the standard output stream
        /// </summary>
        public static void RecordFinishTime()
        {
            Output("[finish]{0}[/finish]", DateTime.Now.ToString());
        }

        /// <summary>
        ///     Write application details to the standard output stream
        /// </summary>
        /// <param name="appName">Name of application, read from App.config if not passed in</param>
        /// <param name="appVersion">Version of application, read from App.config if not passed in</param>
        /// <param name="appType">Type of application, read from App.config if not passed in</param>
        public static void RecordApplicationDetails(string appName = "", string appVersion = "", string appType = "")
        {
            //if (string.IsNullOrEmpty(appName))
            //    appName = AppConfigHelper.ApplicationName;
            //
            //if (string.IsNullOrEmpty(appVersion))
            //    appVersion = AppConfigHelper.ApplicationVersion;
            //
            //if (string.IsNullOrEmpty(appType))
            //    appType = AppConfigHelper.ApplicationType;
            //
            //Output("[app]{0}[/app]", appName);
            //Output("[vers]{0}[/vers]", appVersion);
            //Output("[intype]{0}[/intype]", appType);
        }

        /// <summary>
        ///     Write performer details to the standard output stream
        /// </summary>
        /// <param name="performerName">Name of performer, read from App.config if not passed in</param>
        public static void RecordPerformerDetails(string performerName = "")
        {
            //if (string.IsNullOrEmpty(performerName))
            //    performerName = AppConfigHelper.PerformerName;
            //
            //Output("[performer]{0}[/performer]", performerName);
        }

        /// <summary>
        ///     Write system details to the standard output stream
        /// </summary>
        public static void RecordSystemDetails()
        {
            //var cpu = new CPU();
            //Output("[cpu]{0}[/cpu]", cpu.Name);
            //Output("[computername]{0}[/computername]", SystemHelper.ComputerName);
            //Output("[os]{0}[/os]", SystemHelper.OperatingSystem);
            //Output("[currentuser]{0}[/currentuser]", SystemHelper.CurrentUser);
        }

        /// <summary>
        ///     Write any defect details to the standard output stream
        ///     and throw any pending exceptions
        /// </summary>
        //public static void RecordAnyDefects()
        //{
        //    var hasDefect = false;
        //    foreach (var tag in ScenarioContext.Current.ScenarioInfo.Tags)
        //    {
        //        if (tag.Trim().ToUpper().StartsWith("DEFECT-"))
        //        {
        //            // Remove 'defect-' from beginning of tag
        //            var defectNumber = tag.Trim().Remove(0, 7);
        //
        //            hasDefect = true;
        //            if (ScenarioContext.Current.TestError != null || ReportExcp.Current.HasExceptions)
        //            {
        //                RecordDefect(Defect.Known, Properties.Resources.ReportKnownDefectMsg, defectNumber);
        //            }
        //
        //            if (ScenarioContext.Current.TestError == null && !ReportExcp.Current.HasExceptions)
        //            {
        //                RecordDefect(Defect.Unreproducible, Properties.Resources.ReportUnrepDefectMsg, defectNumber);
        //            }
        //        }
        //    }
        //
        //    if (ScenarioContext.Current.TestError != null || ReportExcp.Current.HasExceptions)
        //    {
        //        if (!hasDefect)
        //        {
        //            RecordDefect(Defect.Unknown, Properties.Resources.ReportUnknownDefectMsg);
        //        }
        //
        //        QAScreenCapture.CaptureDesktop();
        //    }
        //
        //    ReportExcp.Current.ThrowPendingExceptions();
        //}

        /// <summary>
        ///     Write a defect to the standard output stream
        /// </summary>
        /// <param name="defect">Defect type</param>
        /// <param name="message">Message to output</param>
        /// <param name="defectNumber">Defect number</param>
       //private static void RecordDefect(Defect defect, string message, string defectNumber = "")
       //{
       //    var taggedMessage = AssignDefectTags(defect, message);
       //    Output(taggedMessage, FeatureContext.Current.FeatureInfo.Title, ScenarioContext.Current.ScenarioInfo.Title, defectNumber);
       //}

        /// <summary>
        ///     Format message with tags that correspond to level of reporting
        /// </summary>
        /// <param name="level">Reporting level</param>
        /// <param name="message">Message to format</param>
        /// <returns>Formatted message</returns>
        private static string AssignTags(Level level, string message)
        {
            var taggedMessage = string.Empty;

            switch (level)
            {
                case Level.Debug:
                    taggedMessage = string.Format("[debug]{0}[/debug]", message);
                    break;
                case Level.DebugImage:
                    taggedMessage = string.Format("[debugimage]{0}[/debugimage]", message);
                    break;
                case Level.Info:
                    taggedMessage = string.Format("[info]{0}[/info]", message);
                    break;
                case Level.Action:
                    taggedMessage = string.Format("[action]{0}[/action]", message);
                    break;
                case Level.Warning:
                    taggedMessage = string.Format("[warning]{0}[/warning]", message);
                    break;
                case Level.Error:
                    taggedMessage = string.Format("[error]{0}[/error]", message);
                    break;
                case Level.AssertPass:
                    taggedMessage = string.Format("[assertpass]{0}[/assertpass]", message);
                    break;
                case Level.AssertFail:
                    taggedMessage = string.Format("[assertfail]{0}[/assertfail]", message);
                    break;
                case Level.Pass:
                    taggedMessage = string.Format("[pass]{0}[/pass]", message);
                    break;
                case Level.Fail:
                    taggedMessage = string.Format("[fail]{0}[/fail]", message);
                    break;
                case Level.StepImage:
                    taggedMessage = string.Format("[stepimage]{0}[/stepimage]", message);
                    break;
            }

            return taggedMessage;
        }

        /// <summary>
        ///     Format message with tags that correspond to type of defect
        /// </summary>
        /// <param name="defect">Defect type</param>
        /// <param name="message">Message to format</param>
        /// <returns>Formatted message</returns>
        private static string AssignDefectTags(Defect defect, string message)
        {
            var taggedMessage = string.Empty;

            switch (defect)
            {
                case Defect.Known:
                    taggedMessage = string.Format("[defectknown]{0}[/defectknown]", message);
                    break;
                case Defect.Unknown:
                    taggedMessage = string.Format("[defectunknown]{0}[/defectunknown]", message);
                    break;
                case Defect.Unreproducible:
                    taggedMessage = string.Format("[defectunrep]{0}[/defectunrep]", message);
                    break;
            }

            return taggedMessage;
        }

        /// <summary>
        ///     Specifies the type of defect
        /// </summary>
        private enum Defect
        {
            Known,
            Unknown,
            Unreproducible
        }
    }
}