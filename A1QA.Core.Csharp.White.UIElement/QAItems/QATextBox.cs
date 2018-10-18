//  <copyright file="QATextBox.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a TextBox that extends the functionality of
    ///     White TextBox
    /// </summary>
    public class QATextBox : QAItem<TextBox>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATextBox" /> class
        /// </summary>
        /// <param name="textBox">White TextBox</param>
        /// <param name="friendlyName">Friendly name for TextBox</param>
        public QATextBox(TextBox textBox, string friendlyName) : base(textBox, friendlyName)
        {
        }

        /// <summary>
        ///     Gets or sets the text in the TextBox (this does raise all keyboard events)
        /// </summary>
        public string Text
        {
            get { return UIItem.Text; }

            set { UIItem.Text = value; }
        }

        /// <summary>
        ///     Gets or sets the text in the TextBox (this doesn't raise all keyboard events)
        /// </summary>
        public string BulkText
        {
            get { return UIItem.BulkText; }

            set { UIItem.BulkText = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether a TextBox is enabled
        /// </summary>
        public bool Enabled => UIItem.Enabled;

        public static QATextBox Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new TextBox(containerAE, new NullActionListener())
                : null;

            return new QATextBox(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Returns the read only property of the TextBox
        /// </summary>
        public bool IsReadOnly()
        {
            return UIItem.IsReadOnly;
        }

        /// <summary>
        ///     Get a parent TextBox of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for TextBox</param>
        /// <returns>Parent TextBox</returns>
        public static QATextBox GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QATextBox(parent, friendlyName);
        }

        /// <summary>
        ///     Get a TextBox based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TextBox</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TextBox</returns>
        public static QATextBox Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var textBox = FindUIItem(searchCriteria, scope, timeout);
            return new QATextBox(textBox, friendlyName);
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
        public static QATextBox Get(SearchCriteria searchCriteria, AutomationProperty automationProperty, object automationPropertyValue, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var textBox = FindUIItem(searchCriteria, automationProperty, automationPropertyValue, scope, timeout);
            return new QATextBox(textBox, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* TextBoxes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TextBoxes</returns>
        public static List<QATextBox> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QATextBoxes = new List<QATextBox>();
            var textBoxes = FindUIItems(searchCriteria, scope, timeout);

            foreach (var textBox in textBoxes)
            {
                try
                {
                    QATextBoxes.Add(new QATextBox((TextBox) textBox, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QATextBoxes;
        }

        /// <summary>
        ///     Set value of a TextBox using automation and then verify.
        ///     Only call this method when EnterText() does not work.
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void SetValue(string value, bool shouldVerify = true)
        {
            ReportActionValue("SetValue", value);
            UIItem.SetValue(value);
            Thread.Sleep(100);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTextBoxSetMsg, value);
                QAAssert.AreEqual(value, UIItem.Text, friendlyMessage);
            }
        }

        /// <summary>
        ///     Enter text into a TextBox using keyboard and then verify.
        ///     Always try to use this method instead of SetValue() because this a closer
        ///     implementation to how a user would normally set the value of a TextBox.
        /// </summary>
        /// <param name="text">Text to enter</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void EnterText(string text, bool shouldVerify = true)
        {
            ReportActionValue("EnterText", text);
            UIItem.Enter(text);
            WaitForTextInput(text.Length);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTextBoxSetMsg, text);
                QAAssert.AreEqual(text, UIItem.Text, friendlyMessage);
            }
        }

        /// <summary>
        ///     Set the value of the textbox to a value but without the keyboard events
        ///     This will not represent exactly how a user will enter the value but the EnterText method does not support
        ///     Unicode characters
        /// </summary>
        /// <param name="text">Text to enter</param>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void SetUnicodeText(string text, bool shouldVerify = true)
        {
            ReportActionValue("EnterText", text);
            UIItem.BulkText = text;
            Thread.Sleep(100);
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTextBoxSetMsg, text);
                QAAssert.AreEqual(text, UIItem.Text, friendlyMessage);
            }
        }

        /// <summary>
        ///     Pause execution as text might still be being inputted
        /// </summary>
        /// <param name="textLength">Length of text</param>
        private void WaitForTextInput(int textLength)
        {
            Thread.Sleep(100 + textLength * 10);
        }
    }
}