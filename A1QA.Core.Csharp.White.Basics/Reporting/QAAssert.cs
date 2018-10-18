//  <copyright file="QAAssert.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using A1QA.Core.Csharp.White.Basics.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace A1QA.Core.Csharp.White.Basics.Reporting
{
    /// <summary>
    ///     Provides methods for verifying conditions
    /// </summary>
    public static class QAAssert
    {
        /// <summary>
        ///     Check if value is True
        /// </summary>
        /// <param name="condition">Value to check</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsTrue(bool condition, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("IsTrue", true, condition);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsTrue, condition)
                : friendlyMessage;

            try
            {
                Assert.IsTrue(condition);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if value is False
        /// </summary>
        /// <param name="condition">Value to check</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsFalse(bool condition, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("IsFalse", false, condition);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsFalse, condition)
                : friendlyMessage;

            try
            {
                Assert.IsFalse(condition);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if value is null
        /// </summary>
        /// <param name="condition">Value to check</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsNull(object condition, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("IsNull", "Null", condition == null
                ? "Null"
                : "NotNull");

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsNull, condition)
                : friendlyMessage;

            try
            {
                Assert.IsNull(condition);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if value is not null
        /// </summary>
        /// <param name="condition">Value to check</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsNotNull(object condition, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("IsNotNull", "NotNull", condition == null
                ? "Null"
                : "NotNull");

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsNotNull, condition)
                : friendlyMessage;

            try
            {
                Assert.IsNotNull(condition);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value is greater than expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsGreater<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false) where T : IComparable
        {
            var assertionMessage = ConstructAssertionMessage("IsGreater", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsGreater, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.IsTrue(actual.CompareTo(expected) > 0);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value is greater or equal to expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsGreaterOrEqual<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false) where T : IComparable
        {
            var assertionMessage = ConstructAssertionMessage("IsGreaterOrEqual", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsGreaterOrEqual, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.IsTrue(actual.CompareTo(expected) >= 0);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value is less than expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsLess<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false) where T : IComparable
        {
            var assertionMessage = ConstructAssertionMessage("IsLess", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsLess, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.IsTrue(actual.CompareTo(expected) < 0);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value is less or equal to expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void IsLessOrEqual<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false) where T : IComparable
        {
            var assertionMessage = ConstructAssertionMessage("IsLess", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertIsLessOrEqual, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.IsTrue(actual.CompareTo(expected) <= 0);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value equal to expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void AreEqual<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("AreEqual", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertAreEqual, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.AreEqual(expected, actual);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual collection equal to expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void CollectionsAreEqual(List<string> expected, List<string> actual, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("AreEqual", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertCollectionsAreEqual, expected.Count, actual.Count)
                : friendlyMessage;

            try
            {
                CollectionAssert.AreEqual(expected, actual);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if actual value not equal to expected
        /// </summary>
        /// <param name="expected">Value to compare with</param>
        /// <param name="actual">Actual value</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void AreNotEqual<T>(T expected, T actual, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("AreNotEqual", expected, actual);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertAreNotEqual, expected, actual)
                : friendlyMessage;

            try
            {
                Assert.AreNotEqual(expected, actual);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if substring is in string
        /// </summary>
        /// <param name="value">Where to search</param>
        /// <param name="substring">String to search</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void Contains(string value, string substring, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("Contains", value, substring);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertContains, value, substring)
                : friendlyMessage;

            try
            {
                StringAssert.Contains(value, substring);
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Check if substring is not in string
        /// </summary>
        /// <param name="value">Where to search</param>
        /// <param name="substring">String to search</param>
        /// <param name="friendlyMessage">Description for validation</param>
        /// <param name="continueOnFailure">Soft Assertation</param>
        public static void DoesNotContain(string value, string substring, string friendlyMessage = "", bool continueOnFailure = false)
        {
            var assertionMessage = ConstructAssertionMessage("DoesNotContain", value, substring);

            friendlyMessage = string.IsNullOrEmpty(friendlyMessage)
                ? string.Format(DefaultFriendlyMessage.AssertDoesNotContain, value, substring)
                : friendlyMessage;

            try
            {
                Assert.IsFalse(value.Contains(substring), string.Format(Properties.Resources.AssertDoesNotContainFailureMessage, value, substring));
                ReportFriendlyMessage(friendlyMessage, true);
                ReportAssertionMessage(assertionMessage, true);
            }
            catch (AssertFailedException ex)
            {
                HandleFailure(friendlyMessage, assertionMessage, ex.Message, continueOnFailure);
            }
        }

        /// <summary>
        ///     Fail Test with incoclusive result
        /// </summary>
        /// <param name="message">Description for validation</param>
        public static void Inconclusive(string message = "")
        {
            var messageToReport = $"{Properties.Resources.AssertInconclusiveMsg} : {message}";

            Report.Output(Report.Level.Debug, messageToReport);
            Assert.Inconclusive(messageToReport);
        }

        /// <summary>
        ///     Fails the assertion without checking any conditions.
        /// </summary>
        /// <param name="message">Description for validation</param>
        public static void Fail(string message = "")
        {
            var messageToReport = $"{Properties.Resources.AssertForceFailMsg} : {message}";

            Report.Output(Report.Level.Debug, messageToReport);
            Assert.Fail(messageToReport);
        }

        private static void HandleFailure(string friendlyMessage, string assertionMessage, string exceptionMessage, bool continueOnFailure)
        {
            var assertionAndExceptionMessage = $"{assertionMessage} : {exceptionMessage}";

            ReportFriendlyMessage(friendlyMessage, false);
            ReportAssertionMessage(assertionAndExceptionMessage, false);

            var exception = new Exception(assertionAndExceptionMessage);

            if (continueOnFailure)
            {
                ReportExcp.Current.AddException("The test step failed because of errors.", exception, friendlyMessage);
            }
            else
            {
                Fail(assertionAndExceptionMessage);
            }
        }

        private static string ConstructAssertionMessage<T>(string assertName, T expectedValue, T actualValue)
        {
            return string.Format(Properties.Resources.AssertBaseMsg, assertName, expectedValue, actualValue);
        }

        private static void ReportAssertionMessage(string assertionMessage, bool assertionResult)
        {
            var reportLevel = assertionResult
                ? Report.Level.Pass
                : Report.Level.Fail;

            Report.Output(reportLevel, assertionMessage);
        }

        private static void ReportFriendlyMessage(string friendlyMessage, bool assertionResult)
        {
            if (!string.IsNullOrEmpty(friendlyMessage))
            {
                var reportLevel = assertionResult
                    ? Report.Level.Pass
                    : Report.Level.Fail;

                Report.Output(reportLevel, friendlyMessage);
            }
        }
    }
}