//  <copyright file="QAImage.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.QAItems
{
    /// <summary>
    ///     Our own implementation of a Image that extends the functionality of
    ///     White Image
    /// </summary>
    public class QAImage : QAItem<Image>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QAImage" /> class
        /// </summary>
        /// <param name="image">White Image</param>
        /// <param name="friendlyName">Friendly name for Image</param>
        public QAImage(Image image, string friendlyName) : base(image, friendlyName)
        {
        }

        /// <summary>
        ///     Get a parent Image of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Image</param>
        /// <returns>Parent Image</returns>
        public static QAImage GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QAImage(parent, friendlyName);
        }

        public static QAImage Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Image(containerAE, new NullActionListener())
                : null;

            return new QAImage(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Get an Image based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Image</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Image</returns>
        public static QAImage Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var image = FindUIItem(searchCriteria, scope, timeout);
            return new QAImage(image, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Images based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Images</returns>
        public static List<QAImage> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QAImages = new List<QAImage>();
            var images = FindUIItems(searchCriteria, scope, timeout);

            foreach (var image in images)
            {
                try
                {
                    QAImages.Add(new QAImage((Image) image, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QAImages;
        }
    }
}