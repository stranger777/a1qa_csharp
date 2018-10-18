//  <copyright file="QATree.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Extentions;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Actions;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATreeItems
{
    /// <summary>
    ///     Our own implementation of a Tree that extends the functionality of
    ///     White Tree
    /// </summary>
    public class QATree : QAItem<Tree>
    {
        private static readonly List<string> IgnoreItems = new List<string> {"Waters.UI.WatBreadcrumbControl.WatBreadcrumbItem", "Waters.UI.FieldBinding", "Waters.CompoundLibrary.UI.ItemTags.ItemTagUI", "Waters.AnalyticalFW.Composites.Common.ScientificLibraryImport.ItemTagUI"};

        /// <summary>
        ///     Initializes a new instance of the <see cref="QATree" /> class
        /// </summary>
        /// <param name="tree">White Tree</param>
        /// <param name="friendlyName">Friendly name for Tree</param>
        public QATree(Tree tree, string friendlyName) : base(tree, friendlyName)
        {
        }

        /// <summary>
        ///     Gets QATreeNode : the selected tree node
        /// </summary>
        public QATreeNode GetSelectedTreeNode
        {
            get
            {
                var selectedNode = UIItem.SelectedNode;
                return new QATreeNode(selectedNode, string.Empty);
            }
        }

        /// <summary>
        ///     Get a parent Tree of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for Tree</param>
        /// <returns>Parent Tree</returns>
        public static QATree GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QATree(parent, friendlyName);
        }

        /// <summary>
        ///     Get a Tree based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for Menu</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Tree</returns>
        public static QATree Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var tree = FindUIItem(searchCriteria, scope, timeout);
            return new QATree(tree, friendlyName);
        }

        public static QATree Get(string xPath, string friendlyName, AutomationElement scope = null, int timeout = 0)
        {
            var containerAE = GetByXPath(xPath);

            var matchingUIItem = containerAE != null
                ? new Tree(containerAE, new NullActionListener())
                : null;

            return new QATree(matchingUIItem, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* Trees based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching Trees</returns>
        public static List<QATree> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QATrees = new List<QATree>();
            var trees = FindUIItems(searchCriteria, scope, timeout);

            foreach (var tree in trees)
            {
                try
                {
                    QATrees.Add(new QATree((Tree) tree, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QATrees;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string GetSelectedTreeNodePath()
        {
            var sb = new StringBuilder();
            var allNodes = UIItem.Nodes;
            foreach (var node in allNodes)
            {
                sb.Append(node.Name);
                sb.Append("\\");
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Finds and returns a Node from a Tree using the path of the node
        /// </summary>
        /// <param name="nodePath">Path of the Node</param>
        /// <param name="returnNullIfNotFound">function will not throw an exception, but instead return null if not found</param>
        /// <returns>Node that has the path as parameter</returns>
        public QATreeNode FindNodeTree(string[] nodePath, bool returnNullIfNotFound = false)
        {
            ReportActionValue("FindNodeTree", nodePath.ToString());
            TreeNode current = null;
            var allNodes = UIItem.Nodes;

            //set current to be the first matching node in the list
            current = GetNodeFromTreeNodes(allNodes, nodePath[0]);

            //now search down the child nodes
            for (var i = 0; i < nodePath.Length - 1; i++)
            {
                current = TreeSelectItem(current, nodePath[i + 1]);
                if (current == null)
                {
                    break;
                }

                current.Focus();
            }

            if (current == null)
            {
                var exceptionMessage = string.Format(Resources.TreeNodeNotFoundErrorMsg, nodePath, UIItem.PrimaryIdentification);

                if (returnNullIfNotFound)
                {
                    ReportActionValue("FindNodeTree", exceptionMessage);
                    return null;
                }

                throw new AutomationException(exceptionMessage, string.Empty);
            }

            return new QATreeNode(current, string.Join(",", nodePath));
        }

        /// <summary>
        ///     Returns QATreeNode based on node path
        /// </summary>
        /// <param name="nodePath">path to node</param>
        /// <param name="itemToScroll">UIItem can be scrolled to make node onscreen</param>
        /// <param name="expand">If you want to search even in collapsed nodes</param>
        public QATreeNode FindTreeNode(string[] nodePath, UIItem itemToScroll = null, bool expand = true)
        {
            ReportActionValue("FindTreeNode", string.Join("/", nodePath));
            return FindTreeNode(UIItem.Nodes, nodePath, itemToScroll, expand);
        }

        /// <summary>
        ///     Returns true of false if the node exists in the tree
        /// </summary>
        /// <param name="nodePath">Path of the Node</param>
        /// <returns>true or false</returns>
        public bool IsNodeAvailable(string[] nodePath)
        {
            ReportActionValue("IsNodeAvailable", nodePath.ToString());
            TreeNode current = null;
            var allNodes = UIItem.Nodes;

            //set current to be the first matching node in the list
            current = GetNodeFromTreeNodes(allNodes, nodePath[0]);

            //now search down the child nodes
            for (var i = 0; i < nodePath.Length - 1; i++)
            {
                current = TreeSelectItem(current, nodePath[i + 1]);
                if (current == null)
                {
                    break;
                }
            }

            if (current == null)
            {
                return false;
            }

            return true;
        }

        public bool IsHaveNodes()
        {
            ReportActionValue("IsHaveNodes", UIItem);
            return GetNumberOfMainNodes() != 0;
        }

        /// <summary>
        ///     Gets the number of main nodes in the tree
        /// </summary>
        /// <returns>number of main nodes</returns>
        public int GetNumberOfMainNodes()
        {
            if (UIItem.Nodes.Count != 0)
            {
                return UIItem.Nodes[0].Nodes.Count;
            }
            return 0;
        }

        /// <summary>
        ///     Select Tree item based on node name and path
        /// </summary>
        /// <param name="currentNode">cuurent node item</param>
        /// <param name="path">node Help Text OR Lable</param>
        private TreeNode TreeSelectItem(TreeNode currentNode, string path)
        {
            if (currentNode != null)
            {
                if (!currentNode.IsExpanded())
                {
                    try
                    {
                        currentNode.Expand();
                    }
                    catch (AutomationException)
                    {
                        return null;
                    }
                }
                return GetNodeFromTreeNodes(currentNode.Nodes, path);
            }
            return null;
        }

        /// <summary>
        ///     Get node from the treeNodes
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private TreeNode GetNodeFromTreeNodes(TreeNodes treeNodes, string path)
        {
            foreach (var subNode in treeNodes)
            {
                if (subNode.HelpText != string.Empty && IgnoreItems.Contains(subNode.HelpText) == false)
                {
                    if (subNode.HelpText.Contains(path))
                    {
                        return subNode;
                    }
                }
                else if (subNode.Text != string.Empty && IgnoreItems.Contains(subNode.Text) == false)
                {
                    if (subNode.Text.Contains(path))
                    {
                        return subNode;
                    }
                }
                else
                {
                    var lblnode = QALabel.Get(SearchCriteria.ByText(path), string.Empty, subNode, 500);
                    if (lblnode != null && lblnode.UIItemExists && lblnode.IsVisible)
                    {
                        return subNode;
                    }
                }
            }

            return null;
        }

        internal static QATreeNode FindTreeNode(TreeNodes nodes, string[] nodePath, UIItem itemToScroll = null, bool expand = true)
        {
            var parent = new QATreeNode(null, string.Join("/", nodePath));
            var allNodes = nodes;
            foreach (var nodePathPart in nodePath)
            {
                if (allNodes.Count == 0)
                {
                    parent = new QATreeNode(null, string.Join("/", nodePath));
                    break;
                }
                foreach (var node in allNodes)
                {
                    var nodeQA = new QATreeNode(node, $"Tree node {string.Join("/", nodePath)}");

                    itemToScroll?.ScrollClickablePointToParentBoundsVertical(node);

                    if (expand)
                    {
                        nodeQA.Expand();
                    }

                    if (nodeQA.UIItem.GetInnerText() == nodePathPart || nodeQA.UIItem.Text == nodePathPart)
                    {
                        parent = nodeQA;
                        break;
                    }

                    parent = new QATreeNode(null, string.Join("/", nodePath));
                }
                if (parent.UIItemExists)
                {
                    allNodes = parent.UIItem.Nodes;
                }
            }

            return parent;
        }
    }
}