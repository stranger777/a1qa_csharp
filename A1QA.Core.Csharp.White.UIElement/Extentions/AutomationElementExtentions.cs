using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Xml;
using A1QA.Core.Csharp.White.Basics.Reporting;
using Castle.Core.Internal;

namespace A1QA.Core.Csharp.White.UIElement.Extentions
{
    public static class AutomationElementExtentions
    {
        /// <summary>
        ///     Returns Outer Xml Hierarhy for an Automation Element
        /// </summary>
        public static XmlDocument XmlHierarhy(this AutomationElement element)
        {
            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement(element.Current.LocalizedControlType.Replace(" ", "-"));
            xmlDoc.AppendChild(rootNode);
            AddAttributes(element, xmlDoc, rootNode);
            FindChildElement(element, xmlDoc, rootNode);

            return xmlDoc;
        }

        private static void FindChildElement(AutomationElement element, XmlDocument xmlDoc, XmlNode node)
        {
            try
            {
                var childElementCollection = element.FindAll(TreeScope.Children, Condition.TrueCondition);
                foreach (AutomationElement childElement in childElementCollection)
                {
                    XmlNode childNode;
                    var nodeType = string.IsNullOrWhiteSpace(childElement.Current.LocalizedControlType)
                        ? "Unknown"
                        : childElement.Current.LocalizedControlType.Replace(" ", "-");
                    childNode = xmlDoc.CreateElement(nodeType);
                    node.AppendChild(childNode);
                    AddAttributes(childElement, xmlDoc, childNode);
                    Console.WriteLine($"Adding new node child with '{nodeType}' localized type");
                    FindChildElement(childElement, xmlDoc, childNode);
                }
            }
            catch (InvalidOperationException e)
            {
                Log.ReportWarning(e.Message);
            }
            catch (ArgumentException e)
            {
                Log.ReportWarning(e.Message);
            }
        }

        private static void AddAttributes(AutomationElement element, XmlDocument xmlDoc, XmlNode node)
        {
            try
            {
                var automationIdAttr = xmlDoc.CreateAttribute("AutomationId");
                automationIdAttr.Value = element.Current.AutomationId;
                node.Attributes.Append(automationIdAttr);

                var classAttr = xmlDoc.CreateAttribute("ClassName");
                classAttr.Value = element.Current.ClassName;
                node.Attributes.Append(classAttr);

                var frameworkAttr = xmlDoc.CreateAttribute("FrameworkId");
                frameworkAttr.Value = element.Current.FrameworkId;
                node.Attributes.Append(frameworkAttr);

                var nameAttr = xmlDoc.CreateAttribute("Name");
                nameAttr.Value = element.Current.Name;
                node.Attributes.Append(nameAttr);

                var helpTextAttr = xmlDoc.CreateAttribute("HelpText");
                helpTextAttr.Value = element.Current.HelpText;
                node.Attributes.Append(helpTextAttr);

                var runtimeProperty = (int[])element.GetCurrentPropertyValue(AutomationElement.RuntimeIdProperty);
                var runtimeId = xmlDoc.CreateAttribute("RuntimeID");
                runtimeId.Value = string.Join(";",runtimeProperty);
                node.Attributes.Append(runtimeId);

                object patternObj;
                if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
                {
                    var textAttr = xmlDoc.CreateAttribute("Text");
                    var textPattern = (TextPattern) patternObj;
                    textAttr.Value = textPattern.DocumentRange.GetText(-1).TrimEnd('\r');
                    Console.WriteLine("Text attribute added..");
                    node.Attributes.Append(textAttr);
                }
            }
            catch (Exception e)
            {
                Log.ReportWarning(e.Message);
            }
        }
    }
}