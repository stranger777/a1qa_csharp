using System;
using System.Diagnostics;
using System.Threading;

namespace A1QA.Core.Csharp.White.Basics.SystemUtilities
{
    public static class QAWait
    {
        /// <summary>
        ///     Wait until condition is fulfilled but do not wait longer than the timeout
        /// </summary>
        /// <param name="condition">Condition to be fulfilled</param>
        /// <param name="timeoutMilliseconds">Timeout value for wait (milliseconds)</param>
        /// <returns>True if condition was fulfilled before timeout, false if not</returns>
        public static bool Until(Func<bool> condition, int timeoutMilliseconds = 500)
        {
            var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (condition() == false && stopwatch.Elapsed < timeout)
            {
                Thread.Sleep(100);
            }

            stopwatch.Stop();

            return stopwatch.Elapsed < timeout;
        }
    }
}