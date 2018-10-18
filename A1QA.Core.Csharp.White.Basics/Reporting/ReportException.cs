//  <copyright file="ReportException.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Text;

namespace A1QA.Core.Csharp.White.Basics.Reporting
{
    public class ReportException : Exception
    {
        public ReportException(string message, string stackTrace) : base(message)
        {
            StackTrace = stackTrace;
        }

        public override string StackTrace { get; }
    }

    /// <summary>
    ///     Reporting functionality
    /// </summary>
    public class ReportExcp
    {
        private readonly List<ExceptionDetails> exceptions = new List<ExceptionDetails>();

        static ReportExcp()
        {
            Current = new ReportExcp();
        }

        private ReportExcp()
        {
        }

        public static ReportExcp Current { get; private set; }

        public bool HasExceptions => exceptions.Count > 0;

        public int ExceptionsCount => exceptions.Count;

        public void AddException(string message, Exception e, string friendlyMessage)
        {
            exceptions.Add(new ExceptionDetails {Message = message, Exception = e, StackTrace = new StackTrace(true), FriendlyMessage = friendlyMessage});
        }

        public void ClearExceptions()
        {
            exceptions.Clear();
        }

        public void ThrowPendingExceptions()
        {
            if (exceptions.Count > 0)
            {
                var message = new StringBuilder();

                message.AppendLine("\r\n There were failures in the test. Details below.\n");

                foreach (var e in exceptions)
                {
                    if (e.Exception == null)
                    {
                        message.AppendLine($"EXCEPTION [{exceptions.IndexOf(e)}] No exception info available.");
                        continue;
                    }
                    message.AppendLine($"EXCEPTION [{exceptions.IndexOf(e)}] {e.Message ?? string.Empty}\n - {e.FriendlyMessage}\n - {e.Exception.Message}\r\n");
                    message.AppendLine(ShortStackTrace(e.StackTrace));
                }

                ClearExceptions();

                var wrappedException = new ReportException(message.ToString(), string.Empty);
                ExceptionDispatchInfo.Capture(wrappedException).Throw();
            }
        }

        private string ShortStackTrace(StackTrace stackTrace)
        {
            var text = stackTrace.ToString().Replace("\r\n", "\n").Split('\n');
            var shortStackText = new StringBuilder();

            foreach (var t in text)
            {
                if (t.Contains(":line "))
                {
                    shortStackText.AppendLine(t);
                }
            }

            return shortStackText.ToString();
        }

        private struct ExceptionDetails
        {
            public string Message;
            public Exception Exception;
            public StackTrace StackTrace;
            public string FriendlyMessage;
        }
    }
}