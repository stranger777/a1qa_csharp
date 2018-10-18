//  <copyright file="UIItemExtentions.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using A1QA.Core.Csharp.White.UIElement.QAItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace A1QA.Core.Csharp.White.UIElement.Extentions
{
    public static class UIItemExtentions
    {
        /// <summary>
        ///     Scrolls UIItem horizontally while input Item is offscreen
        /// </summary>
        /// <param name="item">UIItem that should be on screen</param>
        public static void ScrollToOnScreenHorizontally(this UIItem parent, UIItem item)
        {
            if (parent.ScrollBars.Horizontal.IsScrollable)
            {
                parent.ScrollBars.Horizontal.SetToMinimum();

                while (item.IsOffScreen && Math.Abs(parent.ScrollBars.Horizontal.MaximumValue - parent.ScrollBars.Horizontal.Value) > 0.1)
                {
                    parent.ScrollBars.Horizontal.ScrollRight();
                }
            }
        }

        /// <summary>
        ///     Scrolls UIItem vertically while input Item is offscreen
        /// </summary>
        /// <param name="item">UIItem that should be on screen</param>
        public static void ScrollToOnScreenVertically(this UIItem parent, UIItem item)
        {
            if (parent.ScrollBars.Vertical.IsScrollable)
            {
                parent.ScrollBars.Vertical.SetToMinimum();

                while (item.IsOffScreen && Math.Abs(parent.ScrollBars.Vertical.MaximumValue - parent.ScrollBars.Vertical.Value) > 0.1)
                {
                    parent.ScrollBars.Vertical.ScrollDown();
                }
            }
        }

        /// <summary>
        ///     Scrolls UIItem vertically while input Item clickable point is not in parent bounds
        /// </summary>
        /// <param name="item">Clickable point of this UIItem should be in parent bounds</param>
        /// <param name="scrollUp">Should scroll to up first</param>
        public static void ScrollClickablePointToParentBoundsVertical(this UIItem parent, UIItem item, bool scrollUp = true)
        {
            if (parent.ScrollBars.Vertical.IsScrollable)
            {
                if (scrollUp)
                {
                    parent.ScrollBars.Vertical.SetToMinimum();
                }

                while ((!(item.ClickablePoint.Y > parent.Bounds.Top) || !(item.ClickablePoint.Y < parent.Bounds.Bottom)) && Math.Abs(parent.ScrollBars.Vertical.MaximumValue - parent.ScrollBars.Vertical.Value) > 0.1)
                {
                    parent.ScrollBars.Vertical.ScrollDown();
                }

                parent.ScrollBars.Vertical.ScrollDown();
            }
        }

        /// <summary>
        ///     Scrolls UIItem horizontally while input Item clickable point is not in parent bounds
        /// </summary>
        /// <param name="item">Clickable point of this UIItem should be in parent bounds</param>
        /// <param name="scrollLeft">Should scroll to left first</param>
        public static void ScrollClickablePointToParentBoundsHorizontal(this UIItem parent, UIItem item, bool scrollLeft = true)
        {
            if (parent.ScrollBars.Horizontal.IsScrollable)
            {
                if (scrollLeft)
                {
                    parent.ScrollBars.Horizontal.SetToMinimum();
                }

                while ((!(item.ClickablePoint.X < parent.Bounds.Right) || !(item.ClickablePoint.X > parent.Bounds.Left)) && Math.Abs(parent.ScrollBars.Horizontal.MaximumValue - parent.ScrollBars.Horizontal.Value) > 0.1)
                {
                    parent.ScrollBars.Horizontal.ScrollRight();
                }

                parent.ScrollBars.Horizontal.ScrollRight();
            }
        }

        /// <summary>
        ///     Returns text of inner TextBox or Label
        /// </summary>
        public static string GetInnerText(this UIItem uIItem)
        {
            string nodeName;
            var textItem = QATextBox.Get(SearchCriteria.All, "UIItem text", uIItem, 1);
            if (textItem.Exists)
            {
                nodeName = textItem.Text;
            }
            else
            {
                var label = QALabel.Get(SearchCriteria.All, "UIItem button", uIItem, 1);
                nodeName = label.Exists
                    ? label.Text
                    : string.Empty;
            }
            return nodeName;
        }
    }
}