using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using System.Xml;
using Castle.Core.Internal;

namespace A1QA.Core.Csharp.White.UIElement.xPathSearcher
{
    internal static class XmlSearcher
    {
        /// <summary>
        ///     Converts XmlNode to AutomationElement from UI
        /// </summary>
        /// <param name="xmlNode">Node you looking for</param>
        /// <param name="xPath">xPath that you use to get node</param>
        /// <returns></returns>
        public static AutomationElement XmlNodeToAutomationElement(XmlNode xmlNode, string xPath)
        {
            var restrictedNode = CreateFromXmlNode(xmlNode);
            var xpathElements = xPath.Replace("//", "/$").Split('/').Where(xPathElement => !xPathElement.IsNullOrEmpty()).ToArray();
            var parent = WorkSpace.MainWindow.Window.AutomationElement;
            var searchInfos = new List<SearchInfo>();
            var newXpath = string.Empty;

            foreach (var xpathElement in xpathElements)
            {
                newXpath = $"{newXpath}/{xpathElement.Replace("$", "/")}";
                var listOfNodes = restrictedNode.SelectNodes(newXpath);

                var searchInfo = new SearchInfo {TreeScope = xpathElement.StartsWith("$")
                        ? TreeScope.Descendants
                        : TreeScope.Children,
                    Condition = GetSearchConditionsFromNode(listOfNodes?[listOfNodes.Count - 1])};

                searchInfos.Add(searchInfo);
            }

            foreach (var searchInfo in searchInfos)
            {
                var newParent = parent.FindFirst(searchInfo.TreeScope, searchInfo.Condition);
                parent = newParent;
            }
            return parent;
        }

        private static XmlDocument CreateFromXmlNode(XmlNode xmlNode)
        {
            var allNodes = GetAllParentNodes(xmlNode);
            var resultXmlDoc = new XmlDocument();

            var firstNode = resultXmlDoc.ImportNode(allNodes[0], false);
            resultXmlDoc.AppendChild(firstNode);
            var lastParent = resultXmlDoc.FirstChild;

            for (var i = 1; i < allNodes.Count; i++)
            {
                var node = resultXmlDoc.ImportNode(allNodes[i], false);
                lastParent.AppendChild(node);
                lastParent = lastParent.FirstChild;
            }

            return resultXmlDoc;
        }

        private static List<XmlNode> GetAllParentNodes(XmlNode xmlNode)
        {
            var allNodes = new List<XmlNode> {xmlNode};

            var parentNode = xmlNode.ParentNode;

            do
            {
                allNodes.Add(parentNode);
                parentNode = parentNode.ParentNode;
            } while (parentNode?.NodeType != XmlNodeType.Document);

            allNodes.Reverse();

            return allNodes;
        }

        private static Condition GetSearchConditionsFromNode(XmlNode curNode)
        {
            var conditions = new List<PropertyCondition>();
            var localisedControlType = curNode.LocalName.Replace("-", " ");
            var automationId = curNode.Attributes["AutomationId"].Value;
            var className = curNode.Attributes["ClassName"].Value;
            var frameworkId = curNode.Attributes["FrameworkId"].Value;
            var name = curNode.Attributes["Name"].Value;
            var helpText = curNode.Attributes["HelpText"].Value;
            var runtimeID = curNode.Attributes["RuntimeID"].Value;
            if (!localisedControlType.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, localisedControlType));
            }
            if (!automationId.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
            }
            if (!className.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
            }
            if (!frameworkId.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.FrameworkIdProperty, frameworkId));
            }
            if (!name.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
            }
            if (!helpText.IsNullOrEmpty())
            {
                conditions.Add(new PropertyCondition(AutomationElement.HelpTextProperty, helpText));
            }
            if (!runtimeID.IsNullOrEmpty())
            {
                var runtimeIdArray = runtimeID.Split(';').Select(h => Int32.Parse(h)).ToArray();
                conditions.Add(new PropertyCondition(AutomationElement.RuntimeIdProperty, runtimeIdArray));
            }

            return conditions.Count > 1
                ? (Condition) new AndCondition(conditions.ToArray())
                : conditions[0];
        }
    }
}