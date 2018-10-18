//  <copyright file="WhiteConfigHelper.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using TestStack.White.Configuration;

namespace A1QA.Core.Csharp.White.Basics.SystemUtilities
{
    /// <summary>
    ///     Provides helper methods for White Configuration
    /// </summary>
    public static class WhiteConfigHelper
    {
        private static readonly int originalFindUIItemTimeout = CoreAppXmlConfiguration.Instance.BusyTimeout;
        private static readonly int originalFindWindowTimeout = CoreAppXmlConfiguration.Instance.FindWindowTimeout;

        public static int OriginalFindUIItemTimeout
        {
            get { return originalFindUIItemTimeout; }
            set
            {
                CoreAppXmlConfiguration.Instance.BusyTimeout = value == 0
                    ? originalFindUIItemTimeout
                    : value;
            }
        }

        public static int OriginalFindWindowTimeout
        {
            get { return originalFindWindowTimeout; }
            set
            {
                CoreAppXmlConfiguration.Instance.FindWindowTimeout = value == 0
                    ? originalFindWindowTimeout
                    : value;
            }
        }

        public static void ResetFindUIItemTimeout()
        {
            CoreAppXmlConfiguration.Instance.BusyTimeout = originalFindUIItemTimeout;
        }

        public static void ResetFindWindowTimeout()
        {
            CoreAppXmlConfiguration.Instance.BusyTimeout = originalFindUIItemTimeout;
        }
    }
}