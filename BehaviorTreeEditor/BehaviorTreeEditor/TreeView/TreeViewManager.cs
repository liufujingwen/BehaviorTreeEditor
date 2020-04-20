using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class TreeViewManager
    {
        private Form m_Form;
        private TreeView m_TreeView;
        private List<BehaviorGroupDesigner> m_Groups;
        private List<BehaviorTreeDesigner> m_BehaviorTrees;

        public TreeView TreeView;
        public Dictionary<string, GroupItem> GroupDic = new Dictionary<string, GroupItem>();

        public TreeViewManager(Form form, TreeView treeView, List<BehaviorGroupDesigner> groups, List<BehaviorTreeDesigner> behaviorTrees)
        {
            m_Form = form;
            m_Groups = groups;
            m_BehaviorTrees = behaviorTrees;
            m_TreeView = treeView;
            m_TreeView.ItemDrag += new ItemDragEventHandler(ItemDrag);
            m_TreeView.DragEnter += new DragEventHandler(DragEnter);
            m_TreeView.DragDrop += new DragEventHandler(DragDrop);
            m_TreeView.DragOver += new DragEventHandler(DragOver);
        }

        public string GetTreeNodeName(BehaviorTreeDesigner behaviorTree)
        {
            return string.IsNullOrEmpty(behaviorTree.Name) ? behaviorTree.ID : string.Format("{0} ({1})", behaviorTree.ID, behaviorTree.Name);
        }

        public string GetGroupTreeNodeName(BehaviorGroupDesigner behaviorGroup)
        {
            return string.IsNullOrEmpty(behaviorGroup.Describe) ? behaviorGroup.GroupName : string.Format("{0} ({1})", behaviorGroup.GroupName, behaviorGroup.Describe);
        }

        public void RefreshByTreeNode()
        {
            int behaviorTreeIndex = 0;
            int groupIndex = 0;

            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                ITreeViewItem treeViewItem = treeNode.Tag as ITreeViewItem;
                if (treeViewItem is BehaviorTreeItem behaviorTreeItem)
                {
                    m_BehaviorTrees[behaviorTreeIndex++] = behaviorTreeItem.BehaviorTree;
                }
                else if (treeViewItem is GroupItem groupItem)
                {
                    m_Groups[groupIndex++] = groupItem.Group;
                    RefreshGroupByTreeNode(groupItem);
                }
            }
        }

        public void RefreshGroupByTreeNode(GroupItem groupItem)
        {
            if (groupItem == null)
                throw new System.ArgumentNullException("TreeViewManager.RefreshGroupByTreeNode(), groupItem is null.");

            int tempGroupIndex = 0;
            for (int j = 0; j < groupItem.TreeNode.Nodes.Count; j++)
            {
                TreeNode tempNode = groupItem.TreeNode.Nodes[j];
                if (tempNode.Tag is BehaviorTreeItem tempBehaviorTreeItem)
                {
                    groupItem.Group.BehaviorTrees[tempGroupIndex++] = tempBehaviorTreeItem.BehaviorTree;
                }
            }
        }

        //放到最后
        public void AddLast(BehaviorTreeDesigner behaviorTree)
        {
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner temp = m_BehaviorTrees[i];
                if (temp.ID == behaviorTree.ID)
                {
                    m_BehaviorTrees.RemoveAt(i);
                    break;
                }
            }

            m_BehaviorTrees.Add(behaviorTree);
        }

        public void BindBehaviorTrees()
        {
            m_TreeView.Nodes.Clear();
            GroupDic.Clear();

            //分组优先
            for (int i = 0; i < m_Groups.Count; i++)
            {
                BehaviorGroupDesigner group = m_Groups[i];
                BindGroup(group);
            }

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                BindBehaviorTreeItem(behaviorTree);
            }
        }

        public BehaviorTreeItem AddBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                throw new System.Exception("behaviorTree is null.");

            if (ExistBehaviorTree(behaviorTree.ID))
                return null;

            m_BehaviorTrees.Add(behaviorTree);
            BehaviorTreeItem behaviorTreeItem = BindBehaviorTreeItem(behaviorTree);
            return behaviorTreeItem;
        }

        public bool ExistBehaviorTree(string behaviorTreeId)
        {
            if (string.IsNullOrEmpty(behaviorTreeId))
                throw new System.Exception("behaviorTreeId is null.");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTreeDesigner = m_BehaviorTrees[i];
                if (behaviorTreeDesigner == null)
                    continue;
                if (behaviorTreeDesigner.ID == behaviorTreeId)
                    return true;
            }
            return false;
        }

        public BehaviorTreeItem BindBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            BehaviorTreeItem behaviorTreeItem = new BehaviorTreeItem();
            behaviorTreeItem.BehaviorTree = behaviorTree;

            TreeNode treeNode = m_TreeView.Nodes.Add(GetTreeNodeName(behaviorTree));
            treeNode.Tag = behaviorTreeItem;
            behaviorTreeItem.GroupItem = null;
            behaviorTreeItem.TreeNode = treeNode;

            return behaviorTreeItem;
        }

        public void UpdateBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            BehaviorTreeItem behaviorTreeItem = FindBehaviorTreeItem(behaviorTree);
            if (behaviorTreeItem != null)
                behaviorTreeItem.TreeNode.Text = GetTreeNodeName(behaviorTree);
        }

        public BehaviorTreeItem FindBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                throw new System.ArgumentNullException("TreeViewManager.FindBehaviorTreeItem(), behaviorTree is null.");

            if (behaviorTree != null)
            {
                for (int i = 0; i < m_TreeView.Nodes.Count; i++)
                {
                    TreeNode treeNode = m_TreeView.Nodes[i];
                    if (treeNode.Tag is BehaviorTreeItem)
                    {
                        BehaviorTreeItem behaviorTreeItem = treeNode.Tag as BehaviorTreeItem;
                        if (behaviorTreeItem.BehaviorTree == behaviorTree || behaviorTreeItem.BehaviorTree.ID == behaviorTree.ID)
                            return behaviorTreeItem;
                    }

                    if (treeNode.Nodes.Count > 0)
                    {
                        for (int ii = 0; ii < treeNode.Nodes.Count; ii++)
                        {
                            TreeNode treeNode_ii = treeNode.Nodes[ii];
                            if (treeNode_ii.Tag is BehaviorTreeItem)
                            {
                                BehaviorTreeItem behaviorTreeItem = treeNode_ii.Tag as BehaviorTreeItem;
                                if (behaviorTreeItem.BehaviorTree == behaviorTree)
                                    return behaviorTreeItem;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public GroupItem FindGroup(string groupName)
        {
            GroupItem groupItem = null;
            GroupDic.TryGetValue(groupName, out groupItem);
            return groupItem;
        }

        public bool ExistGroup(string groupName)
        {
            for (int i = 0; i < m_Groups.Count; i++)
            {
                BehaviorGroupDesigner group = m_Groups[i];
                if (group.GroupName == groupName)
                    return true;
            }
            return false;
        }

        public void UpdateGroup(BehaviorGroupDesigner original, BehaviorGroupDesigner group)
        {
            original.GroupName = group.GroupName;
            original.Describe = group.Describe;

            foreach (var kv in GroupDic)
            {
                if (kv.Value.Group == original)
                {
                    kv.Value.TreeNode.Text = GetGroupTreeNodeName(original);
                }
            }
        }

        public BehaviorTreeItem FindBehaviorTreeItem(string behaviorTreeID)
        {
            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                if (treeNode.Tag != null && treeNode.Tag is BehaviorTreeItem)
                {
                    BehaviorTreeItem behaviorTreeItem = treeNode.Tag as BehaviorTreeItem;
                    if (behaviorTreeItem != null && behaviorTreeItem.BehaviorTree.ID == behaviorTreeID)
                        return behaviorTreeItem;
                }

                if (treeNode.Nodes.Count > 0)
                {
                    for (int ii = 0; ii < treeNode.Nodes.Count; ii++)
                    {
                        TreeNode treeNode_ii = treeNode.Nodes[ii];
                        if (treeNode_ii.Tag is BehaviorTreeItem)
                        {
                            BehaviorTreeItem behaviorTreeItem = treeNode_ii.Tag as BehaviorTreeItem;
                            if (behaviorTreeItem != null && behaviorTreeItem.BehaviorTree.ID == behaviorTreeID)
                                return behaviorTreeItem;
                        }
                    }
                }
            }
            return null;
        }

        public GroupItem AddGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName) || GroupDic.ContainsKey(groupName))
                return null;

            BehaviorGroupDesigner group = new BehaviorGroupDesigner(groupName);
            return AddGroup(group);
        }

        public void DeleteGroup(BehaviorGroupDesigner group)
        {
            foreach (var kv in GroupDic)
            {
                if (kv.Value.Group == group)
                    kv.Value.TreeNode.Remove();
            }

            if (GroupDic.ContainsKey(group.GroupName))
                GroupDic.Remove(group.GroupName);

            m_Groups.Remove(group);
        }

        public GroupItem AddGroup(BehaviorGroupDesigner group)
        {
            if (string.IsNullOrEmpty(group.GroupName) || GroupDic.ContainsKey(group.GroupName))
                return null;

            m_Groups.Add(group);

            GroupItem groupItem = new GroupItem();
            groupItem.Group = group;
            GroupDic.Add(group.GroupName, groupItem);

            TreeNode treeNode = m_TreeView.Nodes.Insert(m_Groups.Count - 1, GetGroupTreeNodeName(group));
            treeNode.Tag = groupItem;
            groupItem.TreeNode = treeNode;

            m_TreeView.SelectedNode = treeNode;

            for (int i = 0; i < group.BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = group.BehaviorTrees[i];
                TreeNode tempTreeNode = treeNode.Nodes.Add(behaviorTree.ID);
                BehaviorTreeItem behaviorTreeItem = new BehaviorTreeItem();
                behaviorTreeItem.BehaviorTree = behaviorTree;
                behaviorTreeItem.GroupItem = groupItem;
                behaviorTreeItem.TreeNode = tempTreeNode;
                tempTreeNode.Tag = behaviorTreeItem;
            }

            return groupItem;
        }

        public void BindGroup(BehaviorGroupDesigner group)
        {
            if (string.IsNullOrEmpty(group.GroupName) || GroupDic.ContainsKey(group.GroupName))
                return;

            GroupItem groupItem = new GroupItem();
            groupItem.Group = group;
            groupItem.Group.GroupName = group.GroupName;
            GroupDic.Add(group.GroupName, groupItem);

            TreeNode treeNode = m_TreeView.Nodes.Add(GetGroupTreeNodeName(group));
            treeNode.Tag = groupItem;
            groupItem.TreeNode = treeNode;

            for (int i = 0; i < group.BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = group.BehaviorTrees[i];
                TreeNode tempTreeNode = treeNode.Nodes.Add(GetTreeNodeName(behaviorTree));
                BehaviorTreeItem behaviorTreeItem = new BehaviorTreeItem();
                behaviorTreeItem.BehaviorTree = behaviorTree;
                behaviorTreeItem.GroupItem = groupItem;
                behaviorTreeItem.TreeNode = tempTreeNode;
                tempTreeNode.Tag = behaviorTreeItem;
            }
        }

        public void RemoveBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                throw new System.Exception("TreeViewManager.RemoveBehaviorTree(), behaviorTree is null.");

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner temp = m_BehaviorTrees[i];
                if (temp == behaviorTree)
                {
                    m_BehaviorTrees.RemoveAt(i);
                    break;
                }
            }

            BehaviorTreeItem behaviorTreeItem = FindBehaviorTreeItem(behaviorTree);
            if (behaviorTreeItem == null)
                return;

            behaviorTreeItem.TreeNode.Remove();
        }

        public int GetTreeViewIndex(TreeNode treeNode)
        {
            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode tempTreeNode = m_TreeView.Nodes[i];
                if (tempTreeNode == treeNode)
                    return i;
            }

            return -1;
        }

        public int GetGroupIndex(GroupItem groupItem, TreeNode treeNode)
        {
            for (int i = 0; i < groupItem.TreeNode.Nodes.Count; i++)
            {
                TreeNode tempTreeNode = groupItem.TreeNode.Nodes[i];
                if (tempTreeNode == treeNode)
                    return i;
            }

            return -1;
        }

        public int GetIndex(BehaviorTreeItem behaviorTreeItem)
        {
            if (behaviorTreeItem == null)
                return -1;

            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                if (treeNode.Tag != null && treeNode.Tag == behaviorTreeItem)
                    return i;
            }

            return -1;
        }

        public void SetSelectItem(ITreeViewItem treeViewItem)
        {
            if (treeViewItem == null)
            {
                m_TreeView.SelectedNode = null;
                return;
            }

            m_TreeView.SelectedNode = treeViewItem.TreeNode;
        }

        public void SetSelectItem(string behaviorTreeID)
        {
            if (string.IsNullOrEmpty(behaviorTreeID))
            {
                m_TreeView.SelectedNode = null;
            }
            else
            {
                BehaviorTreeItem behaviorTreeItem = FindBehaviorTreeItem(behaviorTreeID);
                if (behaviorTreeItem != null)
                {
                    m_TreeView.SelectedNode = behaviorTreeItem.TreeNode;
                }
            }
        }

        public void ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode selectNode = e.Item as TreeNode;
            m_TreeView.SelectedNode = selectNode;
            m_Form.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        //将对象拖入控件的边界时
        private void DragEnter(object sender, DragEventArgs e)
        {
            TreeNode enterNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            if (enterNode != null)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void DragDrop(object sender, DragEventArgs e)
        {
            TreeNode selectedNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                selectedNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }

            m_TreeView.SelectedNode = selectedNode;

            Point Position = m_TreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode dropNode = m_TreeView.GetNodeAt(Position);

            if (dropNode == null)
            {
                if (selectedNode.Tag is BehaviorTreeItem)
                {
                    BehaviorTreeItem behaviorTreeItem = selectedNode.Tag as BehaviorTreeItem;
                    //移除分组
                    if (behaviorTreeItem.GroupItem != null)
                    {
                        behaviorTreeItem.GroupItem.RemoveBehaviorTree(behaviorTreeItem.BehaviorTree);

                        string id = behaviorTreeItem.BehaviorTree.ID;
                        while (ExistBehaviorTree(id))
                            id += "_New";

                        behaviorTreeItem.BehaviorTree.ID = id;
                        SetSelectItem(AddBehaviorTree(behaviorTreeItem.BehaviorTree));
                    }
                    //放到最后
                    else
                    {
                        RemoveBehaviorTree(behaviorTreeItem.BehaviorTree);
                        SetSelectItem(AddBehaviorTree(behaviorTreeItem.BehaviorTree));
                    }
                }

                return;
            }

            //本身
            if (selectedNode == dropNode)
                return;

            //拖进指定组
            if (selectedNode.Tag is BehaviorTreeItem && dropNode.Tag is GroupItem)
            {
                BehaviorTreeItem selectedBehaviorTreeItem = selectedNode.Tag as BehaviorTreeItem;
                GroupItem dropGroupItem = dropNode.Tag as GroupItem;

                //相同组
                if (selectedBehaviorTreeItem.GroupItem == dropGroupItem)
                    return;

                //删除原来分组的数据
                if (selectedBehaviorTreeItem.GroupItem != null)
                {
                    selectedBehaviorTreeItem.GroupItem.RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);
                }
                else
                {
                    RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);
                }

                string id = selectedBehaviorTreeItem.BehaviorTree.ID;
                while (dropGroupItem.Group.ExistBehaviorTree(id))
                    id += "_New";

                selectedBehaviorTreeItem.BehaviorTree.ID = id;
                SetSelectItem(dropGroupItem.AddBehaviorTree(selectedBehaviorTreeItem.BehaviorTree));
            }
            //交换组
            else if (selectedNode.Tag is GroupItem && dropNode.Tag is GroupItem)
            {
                int selectedIndex = GetTreeViewIndex(selectedNode);
                int dropIndex = GetTreeViewIndex(dropNode);

                GroupItem selectedGroupItem = selectedNode.Tag as GroupItem;
                GroupItem dropGroupItem = dropNode.Tag as GroupItem;

                selectedGroupItem.TreeNode.Remove();
                dropGroupItem.TreeNode.Remove();

                if (dropIndex > selectedIndex)
                {
                    m_TreeView.Nodes.Insert(selectedIndex, dropNode);
                    m_TreeView.Nodes.Insert(dropIndex, selectedNode);
                }
                else
                {
                    m_TreeView.Nodes.Insert(dropIndex, selectedNode);
                    m_TreeView.Nodes.Insert(selectedIndex, dropNode);
                }

                RefreshByTreeNode();
                SetSelectItem(selectedNode.Tag as ITreeViewItem);
            }
            else if (selectedNode.Tag is BehaviorTreeItem && dropNode.Tag is BehaviorTreeItem)
            {
                BehaviorTreeItem selectedBehaviorTreeItem = selectedNode.Tag as BehaviorTreeItem;
                BehaviorTreeItem dropBehaviorTreeItem = dropNode.Tag as BehaviorTreeItem;

                //没有组的节点拖拽
                if (selectedBehaviorTreeItem.GroupItem == null)
                {
                    //加入组
                    if (dropBehaviorTreeItem.GroupItem != null)
                    {
                        //从没有组的列表中移除
                        RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);

                        GroupItem groupItem = dropBehaviorTreeItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(groupItem, dropNode);

                        string id = selectedBehaviorTreeItem.BehaviorTree.ID;
                        while (groupItem.Group.ExistBehaviorTree(id))
                            id += "_New";
                        selectedBehaviorTreeItem.BehaviorTree.ID = id;
                        BehaviorTreeItem behaviorTreeItem = groupItem.AddBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);
                        SetSelectItem(behaviorTreeItem);
                    }
                    //没组的交换
                    else
                    {
                        int selectedIndex = GetTreeViewIndex(selectedNode);
                        int dropIndex = GetTreeViewIndex(dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();
                        dropBehaviorTreeItem.TreeNode.Remove();

                        if (dropIndex > selectedIndex)
                        {
                            m_TreeView.Nodes.Insert(selectedIndex, dropNode);
                            m_TreeView.Nodes.Insert(dropIndex, selectedNode);
                        }
                        else
                        {
                            m_TreeView.Nodes.Insert(dropIndex, selectedNode);
                            m_TreeView.Nodes.Insert(selectedIndex, dropNode);
                        }

                        RefreshByTreeNode();
                        SetSelectItem(selectedNode.Tag as ITreeViewItem);
                    }
                }
                //有组的节点拖拽
                else if (selectedBehaviorTreeItem.GroupItem != null)
                {
                    //删除组
                    if (dropBehaviorTreeItem.GroupItem == null)
                    {
                        GroupItem groupItem = selectedBehaviorTreeItem.GroupItem as GroupItem;
                        groupItem.RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);

                        string id = selectedBehaviorTreeItem.BehaviorTree.ID;
                        while (ExistBehaviorTree(id))
                            id += "_New";

                        selectedBehaviorTreeItem.BehaviorTree.ID = id;
                        SetSelectItem(AddBehaviorTree(selectedBehaviorTreeItem.BehaviorTree));
                    }
                    //组内交换
                    else if (selectedBehaviorTreeItem.GroupItem.Group.GroupName == dropBehaviorTreeItem.GroupItem.Group.GroupName)
                    {
                        GroupItem groupItem = selectedBehaviorTreeItem.GroupItem as GroupItem;
                        int selectedIndex = GetGroupIndex(groupItem, selectedNode);
                        int dropIndex = GetGroupIndex(groupItem, dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();
                        dropBehaviorTreeItem.TreeNode.Remove();

                        if (dropIndex > selectedIndex)
                        {
                            groupItem.TreeNode.Nodes.Insert(selectedIndex, dropNode);
                            groupItem.TreeNode.Nodes.Insert(dropIndex, selectedNode);
                        }
                        else
                        {
                            groupItem.TreeNode.Nodes.Insert(dropIndex, selectedNode);
                            groupItem.TreeNode.Nodes.Insert(selectedIndex, dropNode);
                        }

                        RefreshGroupByTreeNode(groupItem);
                        SetSelectItem(selectedBehaviorTreeItem);
                    }
                    //把拖拽节点加入Drop的组
                    else
                    {
                        GroupItem dropGroupItem = dropBehaviorTreeItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(dropGroupItem, dropNode);

                        if (selectedBehaviorTreeItem.GroupItem == null)
                        {
                            //从没有分组的列表移除
                            RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);
                        }
                        else
                        {
                            //从原分组中移除
                            selectedBehaviorTreeItem.GroupItem.RemoveBehaviorTree(selectedBehaviorTreeItem.BehaviorTree);
                        }

                        string id = selectedBehaviorTreeItem.BehaviorTree.ID;
                        while (dropGroupItem.Group.ExistBehaviorTree(id))
                            id += "_New";

                        selectedBehaviorTreeItem.BehaviorTree.ID = id;
                        SetSelectItem(dropGroupItem.AddBehaviorTree(selectedBehaviorTreeItem.BehaviorTree));
                    }
                }
            }
            else
            {
                return;
            }
        }

        //将对象拖过控件边缘时
        private void DragOver(object sender, DragEventArgs e)
        {
            //Point Position = m_TreeView.PointToClient(new Point(e.X, e.Y));

            //在拖过的控件前后显示划线效果
            //TreeNode targetNode = m_TreeView.GetNodeAt(Position);
            //if (targetNode != null && targetNode.Level == 1)
            //{
            //    if (targetNode.PrevNode != null)
            //        targetNode.PrevNode.NodeFont = new Font(m_Form.Font, FontStyle.Regular);
            //    if (targetNode.NextNode != null)
            //        targetNode.NextNode.NodeFont = new Font(m_Form.Font, FontStyle.Regular);
            //    targetNode.NodeFont = new Font(m_Form.Font, FontStyle.Underline);
            //}
        }
    }
}
