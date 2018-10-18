//  <copyright file="Log.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Configuration;
using System.Diagnostics;

namespace A1QA.Core.Csharp.White.Basics.Reporting
{
    /// <summary>
    ///     Error logging and tracing functionality
    /// </summary>
    public static class Log
    {
        private static string LogName => ConfigurationManager.AppSettings["LogName"];

        private static string SourceName => ConfigurationManager.AppSettings["LogSourceName"];

        private static int EventId => Convert.ToInt32(ConfigurationManager.AppSettings["LogEventId"]);

        private static bool IsTraceEnabled => Convert.ToBoolean(ConfigurationManager.AppSettings["LogIsTraceEnabled"]);

        /// <summary>
        ///     Report Informational Message
        /// </summary>
        public static void ReportInfo(string infoMessage, params object[] infoArgs)
        {
            ReportMessage(infoMessage, EventLogEntryType.Information, infoArgs);
        }

        /// <summary>
        ///     Report Warning message
        /// </summary>
        public static void ReportWarning(string warningMessage, params object[] warningArgs)
        {
            ReportMessage(warningMessage, EventLogEntryType.Warning, warningArgs);
        }

        /// <summary>
        ///     Report Error Message
        /// </summary>
        public static void ReportError(string errorMessage, params object[] errorArgs)
        {
            ReportMessage(errorMessage, EventLogEntryType.Error, errorArgs);
        }

        /// <summary>
        ///     Add message to Trace
        /// </summary>
        internal static void Trace(string traceMessage, params object[] traceArgs)
        {
            if (IsTraceEnabled)
            {
                try
                {
                    var outputMessage = string.Format(traceMessage, traceArgs);
                    var formatMessage = $"[{SourceName}] {outputMessage}";
                    System.Diagnostics.Trace.WriteLine(formatMessage);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format(Properties.Resources.LogTraceErrorMsg, traceMessage, ex.Message));
                }
            }
        }

        private static void ReportMessage(string message, EventLogEntryType messageType, params object[] messageArgs)
        {
            try
            {
                CreateSource();
                var eventMessage = string.Format(message, messageArgs);
                Trace(eventMessage);
                EventLog.WriteEntry(SourceName, eventMessage, messageType, EventId);
            }
            catch (Exception ex)
            {
                Trace(Properties.Resources.LogReportErrorMsg, message, ex.Message);
            }
        }

        private static void CreateSource()
        {
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
        }
    }
}