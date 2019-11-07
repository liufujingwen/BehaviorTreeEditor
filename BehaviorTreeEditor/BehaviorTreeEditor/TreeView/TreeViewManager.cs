using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class TreeViewManager
    {
        private Form m_Form;
        private TreeView m_TreeView;
        private List<Group> m_Groups;
        private List<BehaviorTreeDesigner> m_BehaviorTrees;

        public TreeView TreeView;
        public Dictionary<string, GroupItem> GroupDic = new Dictionary<string, GroupItem>();

        public TreeViewManager(Form form, TreeView treeView, List<Group> groups, List<BehaviorTreeDesigner> behaviorTrees)
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

        //把group1插入到group2的前面或者后面
        public void InsertGroup(Group group1, Group group2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < m_Groups.Count; i++)
            {
                Group group = m_Groups[i];
                if (group.GroupName == group1.GroupName)
                    index1 = i;

                if (group.GroupName == group2.GroupName)
                    index2 = i;
            }

            m_Groups.RemoveAt(index1);
            m_Groups.Insert(index2, group1);
        }

        //把行为树1插入到行为树2的前面或者后面
        public void InsertBehaviorTree(BehaviorTreeDesigner behaviorTree1, BehaviorTreeDesigner behaviorTree2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree.ID == behaviorTree1.ID)
                    index1 = i;

                if (behaviorTree.ID == behaviorTree2.ID)
                    index2 = i;
            }

            m_BehaviorTrees.RemoveAt(index1);
            m_BehaviorTrees.Insert(index2, behaviorTree1);
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

        //交换行为树
        public void SwapBehaviorTree(BehaviorTreeDesigner behaviorTree1, BehaviorTreeDesigner behaviorTree2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree.ID == behaviorTree1.ID)
                    index1 = i;
                if (behaviorTree.ID == behaviorTree2.ID)
                    index2 = i;
            }

            BehaviorTreeDesigner tempBehaviorTreeDisigner = m_BehaviorTrees[index1];
            m_BehaviorTrees[index1] = m_BehaviorTrees[index2];
            m_BehaviorTrees[index2] = tempBehaviorTreeDisigner;
        }

        public void BindBehaviorTrees()
        {
            m_TreeView.Nodes.Clear();
            GroupDic.Clear();

            for (int i = 0; i < m_Groups.Count; i++)
            {
                Group group = m_Groups[i];
                BindGroup(group);
            }

            //分组优先
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (!string.IsNullOrEmpty(behaviorTree.GroupName))
                    BindBehaviorTreeItem(behaviorTree);
            }

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (string.IsNullOrEmpty(behaviorTree.GroupName))
                    BindBehaviorTreeItem(behaviorTree);
            }
        }

        public BehaviorTreeItem AddBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            m_BehaviorTrees.Add(behaviorTree);
            BehaviorTreeItem behaviorTreeItem = BindBehaviorTreeItem(behaviorTree);
            m_TreeView.SelectedNode = behaviorTreeItem.TreeNode;
            return behaviorTreeItem;
        }

        public BehaviorTreeItem BindBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            BehaviorTreeItem behaviorTreeItem = new BehaviorTreeItem();
            behaviorTreeItem.BehaviorTree = behaviorTree;

            if (string.IsNullOrEmpty(behaviorTree.GroupName))
            {
                TreeNode treeNode = m_TreeView.Nodes.Add(behaviorTree.ID);
                treeNode.Tag = behaviorTreeItem;
                behaviorTreeItem.GroupItem = null;
                behaviorTreeItem.TreeNode = treeNode;
            }
            else
            {
                GroupItem groupItem = FindGroup(behaviorTree.GroupName);
                //添加分组
                if (groupItem == null)
                {
                    groupItem = AddGroup(behaviorTree.GroupName);
                }
                TreeNode treeNode = groupItem.TreeNode.Nodes.Add(behaviorTree.ID);
                treeNode.Tag = behaviorTreeItem;
                behaviorTreeItem.GroupItem = groupItem;
                behaviorTreeItem.TreeNode = treeNode;
            }

            return behaviorTreeItem;
        }

        public void UpdateBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            BehaviorTreeItem behaviorTreeItem = FindBehaviorTreeItem(behaviorTree);
            if (behaviorTreeItem != null)
                behaviorTreeItem.TreeNode.Text = behaviorTree.ID;
        }

        public BehaviorTreeItem FindBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
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
                                if (behaviorTreeItem.BehaviorTree == behaviorTree || behaviorTreeItem.BehaviorTree.ID == behaviorTree.ID)
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
                Group group = m_Groups[i];
                if (group.GroupName == groupName)
                    return true;
            }
            return false;
        }

        public void UpdateGroup(string oldName, Group group)
        {
            foreach (var kv in GroupDic)
            {
                if (kv.Value.Group == group)
                {
                    kv.Value.TreeNode.Text = group.GroupName;
                }
            }

            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree.GroupName == oldName)
                {
                    behaviorTree.GroupName = group.GroupName;
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

            Group group = new Group(groupName);
            return AddGroup(group);
        }

        public void DeleteGroup(Group group)
        {
            foreach (var kv in GroupDic)
            {
                if (kv.Value.Group == group)
                    kv.Value.TreeNode.Remove();
            }

            if (GroupDic.ContainsKey(group.GroupName))
                GroupDic.Remove(group.GroupName);

            m_Groups.Remove(group);

            for (int i = m_BehaviorTrees.Count - 1; i >= 0; i--)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree.GroupName == group.GroupName)
                    m_BehaviorTrees.RemoveAt(i);
            }
        }

        public GroupItem AddGroup(Group group)
        {
            if (string.IsNullOrEmpty(group.GroupName) || GroupDic.ContainsKey(group.GroupName))
                return null;

            m_Groups.Add(group);

            GroupItem groupItem = new GroupItem();
            groupItem.Group = group;
            GroupDic.Add(group.GroupName, groupItem);

            TreeNode treeNode = m_TreeView.Nodes.Add(group.GroupName);
            treeNode.Tag = groupItem;
            groupItem.TreeNode = treeNode;

            m_TreeView.SelectedNode = treeNode;

            return groupItem;
        }

        public void BindGroup(Group group)
        {
            if (string.IsNullOrEmpty(group.GroupName) || GroupDic.ContainsKey(group.GroupName))
                return;

            GroupItem groupItem = new GroupItem();
            groupItem.Group = group;
            groupItem.Group.GroupName = group.GroupName;
            GroupDic.Add(group.GroupName, groupItem);

            TreeNode treeNode = m_TreeView.Nodes.Add(group.GroupName);
            treeNode.Tag = groupItem;
            groupItem.TreeNode = treeNode;
        }

        public void RemoveBehaviorTree(string behaviorTreeID)
        {
            for (int i = 0; i < m_BehaviorTrees.Count; i++)
            {
                BehaviorTreeDesigner behaviorTree = m_BehaviorTrees[i];
                if (behaviorTree.ID == behaviorTreeID)
                {
                    m_BehaviorTrees.RemoveAt(i);
                    break;
                }
            }

            BehaviorTreeItem behaviorTreeItem = FindBehaviorTreeItem(behaviorTreeID);
            if (behaviorTreeItem == null)
                return;

            int index = GetIndex(behaviorTreeItem);
            m_TreeView.Nodes.Remove(behaviorTreeItem.TreeNode);

            if (index >= 0 && index < m_TreeView.Nodes.Count)
            {
                m_TreeView.SelectedNode = m_TreeView.Nodes[index];
            }
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
                    if (behaviorTreeItem.GroupItem != null)
                    {
                        behaviorTreeItem.TreeNode.Remove();
                        behaviorTreeItem.GroupItem = null;
                        behaviorTreeItem.BehaviorTree.GroupName = null;
                        m_TreeView.Nodes.Add(behaviorTreeItem.TreeNode);
                        m_TreeView.SelectedNode = selectedNode;

                        AddLast(behaviorTreeItem.BehaviorTree);
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

                selectedBehaviorTreeItem.TreeNode.Remove();
                selectedBehaviorTreeItem.GroupItem = dropGroupItem;
                selectedBehaviorTreeItem.BehaviorTree.GroupName = dropGroupItem.Group.GroupName;
                dropGroupItem.TreeNode.Nodes.Add(selectedBehaviorTreeItem.TreeNode);

                AddLast(selectedBehaviorTreeItem.BehaviorTree);
            }
            //交换组
            else if (selectedNode.Tag is GroupItem && dropNode.Tag is GroupItem)
            {
                int selectedIndex = GetTreeViewIndex(selectedNode);
                int dropIndex = GetTreeViewIndex(dropNode);

                GroupItem selectedGroupItem = selectedNode.Tag as GroupItem;
                GroupItem dropGroupItem = dropNode.Tag as GroupItem;

                selectedGroupItem.TreeNode.Remove();

                m_TreeView.Nodes.Insert(dropIndex, selectedNode);

                //把group1插入到group2的前面或者后面
                InsertGroup(selectedGroupItem.Group, dropGroupItem.Group);
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
                        GroupItem groupItem = dropBehaviorTreeItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(groupItem, dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();
                        groupItem.TreeNode.Nodes.Add(selectedNode);
                        selectedBehaviorTreeItem.GroupItem = groupItem;
                        selectedBehaviorTreeItem.BehaviorTree.GroupName = groupItem.Group.GroupName;

                        AddLast(selectedBehaviorTreeItem.BehaviorTree);
                    }
                    //没组的交换
                    else
                    {
                        int selectedIndex = GetTreeViewIndex(selectedNode);
                        int dropIndex = GetTreeViewIndex(dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();
                        m_TreeView.Nodes.Insert(dropIndex, selectedNode);

                        //把行为树1插入行为树2的前面或者后面
                        InsertBehaviorTree(selectedBehaviorTreeItem.BehaviorTree, dropBehaviorTreeItem.BehaviorTree);
                    }
                }
                //有组的节点拖拽
                else if (selectedBehaviorTreeItem.GroupItem != null)
                {
                    //删除组
                    if (dropBehaviorTreeItem.GroupItem == null)
                    {
                        GroupItem groupItem = selectedBehaviorTreeItem.GroupItem as GroupItem;
                        int dropIndex = GetTreeViewIndex(dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();

                        BehaviorTreeDesigner lastBehaviorTree = null;
                        if (dropIndex < m_TreeView.Nodes.Count)
                        {
                            BehaviorTreeItem behaviorTreeItem = m_TreeView.Nodes[dropIndex].Tag as BehaviorTreeItem;
                            lastBehaviorTree = behaviorTreeItem.BehaviorTree;
                        }

                        m_TreeView.Nodes.Add(selectedBehaviorTreeItem.TreeNode);
                        selectedBehaviorTreeItem.GroupItem = null;
                        selectedBehaviorTreeItem.BehaviorTree.GroupName = null;

                        AddLast(selectedBehaviorTreeItem.BehaviorTree);
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

                        //把行为树1插入行为树2的前面或者后面
                        InsertBehaviorTree(selectedBehaviorTreeItem.BehaviorTree, dropBehaviorTreeItem.BehaviorTree);
                    }
                    //把拖拽节点加入Drop的组
                    else
                    {
                        GroupItem dropGroupItem = dropBehaviorTreeItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(dropGroupItem, dropNode);

                        selectedBehaviorTreeItem.TreeNode.Remove();

                        BehaviorTreeDesigner lastBehaviorTree = null;
                        if (dropIndex < dropGroupItem.TreeNode.Nodes.Count)
                        {
                            BehaviorTreeItem behaviorTreeItem = dropGroupItem.TreeNode.Nodes[dropIndex].Tag as BehaviorTreeItem;
                            lastBehaviorTree = behaviorTreeItem.BehaviorTree;
                        }

                        dropGroupItem.TreeNode.Nodes.Insert(dropIndex, selectedBehaviorTreeItem.TreeNode);
                        selectedBehaviorTreeItem.GroupItem = dropGroupItem;
                        selectedBehaviorTreeItem.BehaviorTree.GroupName = dropGroupItem.Group.GroupName;

                        AddLast(selectedBehaviorTreeItem.BehaviorTree);
                    }
                }
            }
            else
            {
                return;
            }

            m_TreeView.SelectedNode = selectedNode;
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
