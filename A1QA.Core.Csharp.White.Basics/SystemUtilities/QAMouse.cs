//  <copyright file="QAMouse.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System.Windows;
using A1QA.Core.Csharp.White.Basics.Reporting;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;

namespace A1QA.Core.Csharp.White.Basics.SystemUtilities
{
    /// <summary>
    ///     Provides methods for mouse device
    /// </summary>
    public static class QAMouse
    {
        public static AttachedMouse Mouse = Desktop.Instance.Mouse;

        public static void Hover(Point point)
        {
            Mouse.Location = point;
        }

        public static void Click(Point point)
        {
            Mouse.Click(point);
        }

        public static void RightClick(Point point)
        {
            Hover(point);
            Mouse.RightClick();
        }

        /// <summary>
        ///     Drag pointer from start location to end location
        /// </summary>
        /// <param name="startLocation">Point to drag from</param>
        /// <param name="endLocation">Point to drag to</param>
        public static void Drag(IUIItem itemFrom, IUIItem itemTo, Point startLocation = default(Point), Point endLocation = default(Point))
        {
            Report.Output(Report.Level.Debug, Properties.Resources.MouseDragMsg);
            if (startLocation == default(Point))
            {
                Mouse.DragAndDrop(itemFrom, itemTo);
            }
            else
            {
                Mouse.DragAndDrop(itemFrom, startLocation, itemTo, endLocation);
            }
        }
    }
}