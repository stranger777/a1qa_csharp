//  <copyright file="ResourceExtentions.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Collections;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace A1QA.Core.Csharp.White.Basics.SystemUtilities.Extentions
{
    public static class ResourceExtentions
    {
        private static string GetKeyByENValue(this ResourceManager resourceManager, string value)
        {
            var resourceSet = resourceManager.GetResourceSet(CultureInfo.GetCultureInfo("EN"), true, true);
            return resourceSet.OfType<DictionaryEntry>().FirstOrDefault(e => e.Value.ToString() == value).Key.ToString();
        }

        public static string GetCurrentCultureValue(this ResourceManager resourceManager, string ENvalue)
        {
            return resourceManager.GetString(resourceManager.GetKeyByENValue(ENvalue));
        }
    }
}