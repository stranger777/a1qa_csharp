//  <copyright file="QATreeNode.cs" company="A1QA">
//   Copyright © 2017 A1QA. All Rights Reserved.
//  </copyright>

using System;
using System.Collections.Generic;
using A1QA.Core.Csharp.White.Basics.Reporting;
using A1QA.Core.Csharp.White.UIElement.Properties;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;

namespace A1QA.Core.Csharp.White.UIElement.QAItems.QATreeItems
{
    /// <summary>
    ///     Our own implementation of a TreeNode that extends the functionality of
    ///     White TreeNode
    /// </summary>
    public class QATreeNode : QAItem<TreeNode>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QATreeNode" /> class
        /// </summary>
        /// <param name="treenode">White TreeNode</param>
        /// <param name="friendlyName">Friendly name for TreeNode</param>
        public QATreeNode(TreeNode treenode, string friendlyName) : base(treenode, friendlyName)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether a TreeNode is selected
        /// </summary>
        public bool IsSelected => UIItem.IsSelected;

        /// <summary>
        ///     Returns text of Inner Text TextBox or Label
        /// </summary>
        public string InnerText
        {
            get
            {
                var textOfNode = QATextBox.Get(SearchCriteria.All, "Tree node text", UIItem, 4);

                if (textOfNode.Exists)
                {
                    return textOfNode.Text;
                }

                var labelOfNode = QALabel.Get(SearchCriteria.All, "Tree node text", UIItem, 4);
                return labelOfNode.UIItemExists
                    ? labelOfNode.Text
                    : string.Empty;
            }
        }

        /// <summary>
        ///     Get a parent TreeNode of a child UIItem
        /// </summary>
        /// <param name="child">Child UIItem</param>
        /// <param name="friendlyName">Friendly name for TreeNode</param>
        /// <returns>Parent TreeNode</returns>
        public static QATreeNode GetParent(UIItem child, string friendlyName)
        {
            var parent = GetParentUIItem(child);
            return new QATreeNode(parent, friendlyName);
        }

        /// <summary>
        ///     Get a TreeNode based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="friendlyName">Friendly name for TreeNode</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TreeNode</returns>
        public static QATreeNode Get(SearchCriteria searchCriteria, string friendlyName, UIItem scope = null, int timeout = 0)
        {
            var treeNode = FindUIItem(searchCriteria, scope, timeout);
            return new QATreeNode(treeNode, friendlyName);
        }

        /// <summary>
        ///     Gets *multiple* TreeNodes based on SearchCriteria
        /// </summary>
        /// <param name="searchCriteria">UIItem identification conditions</param>
        /// <param name="scope">Scope of search</param>
        /// <param name="timeout">Timeout value for search (milliseconds)</param>
        /// <returns>Matching TreeNodes</returns>
        public static List<QATreeNode> GetMultiple(SearchCriteria searchCriteria, UIItem scope = null, int timeout = 0)
        {
            var QATreeNodes = new List<QATreeNode>();
            var treeNodes = FindUIItems(searchCriteria, scope, timeout);

            foreach (var treeNode in treeNodes)
            {
                try
                {
                    QATreeNodes.Add(new QATreeNode((TreeNode) treeNode, string.Empty));
                }
                catch (InvalidCastException ex)
                {
                    Report.Output(Report.Level.Debug, Resources.UIItemInvalidCastMsg, ex.Message);
                }
            }

            return QATreeNodes;
        }

        /// <summary>
        ///     Select TreeNode and then verify
        /// </summary>
        /// <param name="shouldVerify">Indicate whether or not to verify</param>
        public void Select(bool shouldVerify = true)
        {
            ReportAction("Select");
            UIItem.Select();
            CaptureImage();

            if (shouldVerify)
            {
                var friendlyMessage = ConstructFriendlyMessage(Resources.FriendlyTreeNodeSelectMsg);
                QAAssert.AreEqual(true, UIItem.IsSelected, friendlyMessage);
            }
        }

        /// <summary>
        ///     Expand TreeNode
        /// </summary>
        public void Expand()
        {
            ReportAction("Expand Node");
            if (!UIItem.IsExpanded())
            {
                try
                {
                    UIItem.Expand();
                }
                catch (Exception ex)
                {
                    if (ex is AutomationException || ex is NullReferenceException)
                    {
                        return;
                    }

                    CaptureImage();
                    throw;
                }
            }
            CaptureImage();
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
            return QATree.FindTreeNode(UIItem.Nodes, nodePath, itemToScroll, expand);
        }
    }
}