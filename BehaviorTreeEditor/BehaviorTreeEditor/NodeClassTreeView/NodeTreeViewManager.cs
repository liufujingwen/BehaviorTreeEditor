using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class NodeTreeViewManager
    {
        private NodeTemplate m_NodeTemplate;
        private TreeView m_TreeView;

        NodeTypeItem m_CompositeItem;
        NodeTypeItem m_DecoratorItem;
        NodeTypeItem m_ConditionItem;
        NodeTypeItem m_ActionItem;

        public NodeTreeViewManager(TreeView treeView, NodeTemplate nodeTemplate)
        {
            m_TreeView = treeView;
            m_NodeTemplate = nodeTemplate;

            treeView.Nodes.Clear();
            m_CompositeItem = new NodeTypeItem();
            m_CompositeItem.NodeType = NodeType.Composite;
            m_CompositeItem.TreeNode = m_TreeView.Nodes.Add("组合节点");
            m_CompositeItem.TreeNode.Tag = m_CompositeItem;

            m_DecoratorItem = new NodeTypeItem();
            m_DecoratorItem.NodeType = NodeType.Decorator;
            m_DecoratorItem.TreeNode = m_TreeView.Nodes.Add("装饰节点");
            m_DecoratorItem.TreeNode.Tag = m_DecoratorItem;

            m_ConditionItem = new NodeTypeItem();
            m_ConditionItem.NodeType = NodeType.Condition;
            m_ConditionItem.TreeNode = m_TreeView.Nodes.Add("条件节点");
            m_ConditionItem.TreeNode.Tag = m_ConditionItem;

            m_ActionItem = new NodeTypeItem();
            m_ActionItem.NodeType = NodeType.Action;
            m_ActionItem.TreeNode = m_TreeView.Nodes.Add("动作节点");
            m_ActionItem.TreeNode.Tag = m_ActionItem;
        }

        public void BindNodeTemplate()
        {
            for (int i = 0; i < m_NodeTemplate.Nodes.Count; i++)
            {
                NodeDefine nodeDefine = m_NodeTemplate.Nodes[i];
                BindNodeDefine(nodeDefine);
            }

            m_TreeView.ExpandAll();
            m_TreeView.SelectedNode = m_TreeView.Nodes[0];
            m_TreeView.Nodes[0].EnsureVisible();
        }

        public NodeItem BindNodeDefine(NodeDefine nodeDefine)
        {
            NodeTypeItem nodeTypeItem = GetNodeTypeItem(nodeDefine.NodeType);
            if (nodeTypeItem == null)
                return null;

            NodeItem nodeItem = FindNodeItem(nodeDefine);

            //刷新
            if (nodeItem != null)
            {
                if (nodeItem.OldNodeType == nodeDefine.NodeType && nodeItem.OldCategory == nodeDefine.Category)
                {
                    nodeItem.TreeNode.Text = nodeDefine.ClassType + (string.IsNullOrEmpty(nodeDefine.Label) ? string.Empty : " (" + nodeDefine.Label + ")");
                    return nodeItem;
                }
                else
                {
                    nodeItem.TreeNode.Remove();
                }

                if (nodeItem.OldCategory != nodeDefine.Category)
                {
                    CheckRemoveCategory(nodeItem.OldNodeType, nodeItem.OldCategory);
                }
            }

            if (nodeItem == null)
                nodeItem = new NodeItem();

            if (string.IsNullOrEmpty(nodeDefine.Category))
            {
                TreeNode treeNode = nodeTypeItem.TreeNode.Nodes.Add(nodeDefine.ClassType + (string.IsNullOrEmpty(nodeDefine.Label) ? string.Empty : " (" + nodeDefine.Label + ")"));
                treeNode.Tag = nodeItem;
                nodeItem.TreeNode = treeNode;
                nodeItem.CategoryItem = null;
                nodeItem.NodeTypeItem = nodeTypeItem;
                nodeItem.NodeDefine = nodeDefine;

                nodeItem.OldCategory = nodeDefine.Category;
                nodeItem.OldNodeType = nodeDefine.NodeType;
            }
            else
            {
                CategoryItem categoryItem = FindCategoryItem(nodeDefine.NodeType, nodeDefine.Category);

                if (categoryItem == null)
                    categoryItem = BindCategory(nodeTypeItem.TreeNode, nodeDefine.Category);

                TreeNode treeNode = categoryItem.TreeNode.Nodes.Add(nodeDefine.ClassType + (string.IsNullOrEmpty(nodeDefine.Label) ? string.Empty : " (" + nodeDefine.Label + ")"));
                treeNode.Tag = nodeItem;
                nodeItem.TreeNode = treeNode;
                nodeItem.CategoryItem = categoryItem;
                nodeItem.NodeTypeItem = nodeTypeItem;
                nodeItem.NodeDefine = nodeDefine;

                nodeItem.OldCategory = nodeDefine.Category;
                nodeItem.OldNodeType = nodeDefine.NodeType;
            }

            return nodeItem;
        }

        public CategoryItem BindCategory(TreeNode treeNode, string category)
        {
            CategoryItem categoryItem = null;
            TreeNode tempNode = treeNode;
            string[] strs = category.Split('/');
            string content = string.Empty;
            for (int i = 0; i < strs.Length; i++)
            {
                string str = strs[i];
                if (string.IsNullOrEmpty(str))
                    continue;

                content += str;

                categoryItem = FindCategoryItem(treeNode, content);
                if (categoryItem != null)
                {
                    tempNode = categoryItem.TreeNode;
                }
                else
                {
                    categoryItem = new CategoryItem();
                    tempNode = tempNode.Nodes.Add(str);
                    tempNode.Tag = categoryItem;
                    categoryItem.TreeNode = tempNode;
                    categoryItem.CategoryStr = content;
                }



                content += i < strs.Length - 1 ? "/" : string.Empty;
            }

            return categoryItem;
        }

        public bool RemoveNodeDefine(NodeDefine nodeDefine)
        {
            NodeItem nodeItem = FindNodeItem(nodeDefine);
            if (nodeItem != null)
                nodeItem.TreeNode.Remove();

            if (nodeItem.CategoryItem != null)
            {
                TreeNode treeNode = nodeItem.CategoryItem.TreeNode;
                while (treeNode.Tag is CategoryItem)
                {
                    TreeNode parentNode = treeNode.Parent;
                    if (treeNode.Nodes.Count == 0)
                    {
                        treeNode.Remove();
                        treeNode = parentNode;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        public NodeTypeItem GetNodeTypeItem(NodeType nodeType)
        {
            switch (nodeType)
            {
                case NodeType.Composite:
                    return m_CompositeItem;
                case NodeType.Decorator:
                    return m_DecoratorItem;
                case NodeType.Condition:
                    return m_ConditionItem;
                case NodeType.Action:
                    return m_ActionItem;
                default:
                    return null;
            }
        }

        public NodeItem FindNodeItem(NodeDefine nodeDefine)
        {
            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                NodeItem nodeItem = FindNodeItem(treeNode, nodeDefine);
                if (nodeItem != null)
                    return nodeItem;
            }

            return null;
        }

        private NodeItem FindNodeItem(TreeNode treeNode, NodeDefine nodeDefine)
        {
            if (treeNode.Tag is NodeItem)
            {
                NodeItem nodeItem = treeNode.Tag as NodeItem;
                if (nodeItem.NodeDefine.ClassType == nodeDefine.ClassType || nodeItem.NodeDefine == nodeDefine)
                {
                    return nodeItem;
                }
            }

            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                TreeNode tempTreeNode = treeNode.Nodes[i];
                NodeItem nodeItem = FindNodeItem(tempTreeNode, nodeDefine);
                if (nodeItem != null)
                    return nodeItem;
            }

            return null;
        }

        public CategoryItem FindCategoryItem(NodeType nodeType, string categoryStr)
        {
            NodeTypeItem nodeTypeItem = GetNodeTypeItem(nodeType);
            for (int i = 0; i < nodeTypeItem.TreeNode.Nodes.Count; i++)
            {
                TreeNode treeNode = nodeTypeItem.TreeNode.Nodes[i];
                CategoryItem categoryItem = FindCategoryItem(treeNode, categoryStr);
                if (categoryItem != null)
                    return categoryItem;
            }

            return null;
        }

        private CategoryItem FindCategoryItem(TreeNode treeNode, string categoryStr)
        {
            if (treeNode.Tag is CategoryItem)
            {
                CategoryItem categoryItem = treeNode.Tag as CategoryItem;
                if (categoryItem.CategoryStr == categoryStr)
                    return categoryItem;
            }

            for (int i = 0; i < treeNode.Nodes.Count; i++)
            {
                TreeNode tempTreeNode = treeNode.Nodes[i];
                CategoryItem categoryItem = FindCategoryItem(tempTreeNode, categoryStr);
                if (categoryItem != null)
                    return categoryItem;
            }

            return null;
        }

        public void CheckRemoveCategory(NodeType nodeType, string category)
        {
            CategoryItem categoryItem = FindCategoryItem(nodeType, category);

            if (categoryItem != null)
            {
                TreeNode treeNode = categoryItem.TreeNode;
                while (treeNode != null && treeNode.Tag is CategoryItem)
                {
                    TreeNode parentNode = treeNode.Parent;
                    if (treeNode.Nodes.Count == 0)
                    {
                        treeNode.Remove();
                        treeNode = parentNode;
                    }
                    else
                    {
                        treeNode = null;
                    }
                }
            }
        }
    }
}
