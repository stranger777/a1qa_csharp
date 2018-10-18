//  <copyright file="WorkSpace.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Xml;
using A1QA.Core.Csharp.White.UIElement.QAItems;
using A1QA.Core.Csharp.White.UIElement.QAItems.QAWindowItems;

namespace A1QA.Core.Csharp.White.UIElement
{
    public static class WorkSpace
    {
        private static XmlDocument xmlDoc = new XmlDocument();
        public static QAApplication Application { get; set; }

        public static QAWindow MainWindow { get; set; }

        public static XmlDocument XmlDoc => MainWindow.GetHierarchy();
    }
}