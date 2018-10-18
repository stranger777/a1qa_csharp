//  <copyright file="QAClipboard.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Windows;
using A1QA.Core.Csharp.White.Basics.Reporting;
using TestStack.White;
using TestStack.White.WindowsAPI;

namespace A1QA.Core.Csharp.White.Basics.SystemUtilities
{
    /// <summary>
    ///     Provides methods for the system Clipboard
    /// </summary>
    public static class QAClipboard
    {
        /// <summary>
        ///     Gets data from system Clipboard in Text format
        /// </summary>
        public static string Text
        {
            get
            {
                var data = (string) GetData(DataFormats.Text);
                return data.Trim();
            }
        }

        /// <summary>
        ///     Gets data from system Clipboard in CSV format
        /// </summary>
        public static string CSV
        {
            get
            {
                var data = (string) GetData(DataFormats.CommaSeparatedValue);
                return data.Trim();
            }
        }

        private static object GetData(string format)
        {
            Report.Output(Report.Level.Debug, Properties.Resources.ClipboardRetrieve);
            var data = Clipboard.GetData(format);
            return data;
        }

        /// <summary>
        ///     Copy data to system Clipboard using keyboard (CTRL + C)
        /// </summary>
        public static void CopyData()
        {
            Report.Output(Report.Level.Debug, Properties.Resources.ClipboardCopy);
            Desktop.Instance.Keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            Desktop.Instance.Keyboard.Enter("C");
            Desktop.Instance.Keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
        }

        /// <summary>
        ///     Clear any data from system Clipboard
        /// </summary>
        public static void Clear()
        {
            Report.Output(Report.Level.Debug, Properties.Resources.ClipboardClear);
            Clipboard.Clear();
        }
    }
}