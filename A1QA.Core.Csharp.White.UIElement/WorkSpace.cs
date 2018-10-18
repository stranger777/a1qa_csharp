//  <copyright file="WorkSpace.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Xml;
using A1QA.Core.Csharp.White.UIElement.QAItems;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems;
using Castle.Core.Internal;

namespace A1QA.Core.Csharp.White.UIElement
{
    public static class WorkSpace
    {
        private static string _appName;
        private static XmlDocument xmlDoc = new XmlDocument();
        public static QAApplication Application { get; set; }

        public static QAWindow MainWindow { get; set; }

        public static string ApplicationName
        {
            get
            {
                return _appName.IsNullOrEmpty()
                    ? Environment.UserName
                    : _appName;
            } 
            set { _appName = value; }
        }

        public static XmlDocument XmlDoc => MainWindow.GetHierarchy();
    }
}