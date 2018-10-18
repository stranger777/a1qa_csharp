//  <copyright file="ScrollBarExtentions.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using TestStack.White.UIItems.Scrolling;

namespace A1QA.Core.Csharp.White.UIElement.Extentions
{
    public static class ScrollBarExtentions
    {
        /// <summary>
        ///     is scrolled to maximal value
        /// </summary>
        /// <param name="scrollBar"></param>
        /// <returns></returns>
        public static bool IsMaximal(this IScrollBar scrollBar)
        {
            return scrollBar.Value == scrollBar.MaximumValue || scrollBar.Value == -1;
        }
    }
}