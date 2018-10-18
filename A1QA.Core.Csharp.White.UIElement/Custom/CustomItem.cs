//  <copyright file="CustomItem.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Windows.Automation;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Custom;

namespace A1QA.Core.Csharp.White.UIElement.Custom
{
    [ControlTypeMapping(CustomUIItemType.Custom)]
    public class CustomItem : CustomUIItem
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomItem" /> class
        /// </summary>
        /// <param name="automationElement">UI Automation element in UI Automation tree</param>
        /// <param name="actionListener">Action listener</param>
        public CustomItem(AutomationElement automationElement, ActionListener actionListener) : base(automationElement, actionListener)
        {
            Console.Out.WriteLine(automationElement.Current.AutomationId);
        }

        protected CustomItem()
        {
        }
    }
}