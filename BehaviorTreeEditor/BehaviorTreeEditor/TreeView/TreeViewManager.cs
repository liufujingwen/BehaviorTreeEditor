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
        private List<AgentDesigner> m_Agents;

        public TreeView TreeView;
        public Dictionary<string, GroupItem> GroupDic = new Dictionary<string, GroupItem>();

        public TreeViewManager(Form form, TreeView treeView, List<Group> groups, List<AgentDesigner> agents)
        {
            m_Form = form;
            m_Groups = groups;
            m_Agents = agents;
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

        //把agent1插入到agent2的前面或者后面
        public void InsertAgent(AgentDesigner agent1, AgentDesigner agent2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent.AgentID == agent1.AgentID)
                    index1 = i;

                if (agent.AgentID == agent2.AgentID)
                    index2 = i;
            }

            m_Agents.RemoveAt(index1);
            m_Agents.Insert(index2, agent1);
        }

        //放到最后
        public void AddLast(AgentDesigner agent)
        {
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner temp = m_Agents[i];
                if (temp.AgentID == agent.AgentID)
                {
                    m_Agents.RemoveAt(i);
                    break;
                }
            }

            m_Agents.Add(agent);
        }

        //交换Agent
        public void SwapAgent(AgentDesigner agent1, AgentDesigner agent2)
        {
            int index1 = 0;
            int index2 = 0;

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent.AgentID == agent1.AgentID)
                    index1 = i;
                if (agent.AgentID == agent2.AgentID)
                    index2 = i;
            }

            AgentDesigner tempAgentDisigner = m_Agents[index1];
            m_Agents[index1] = m_Agents[index2];
            m_Agents[index2] = tempAgentDisigner;
        }

        public void BindAgents()
        {
            m_TreeView.Nodes.Clear();
            GroupDic.Clear();

            for (int i = 0; i < m_Groups.Count; i++)
            {
                Group group = m_Groups[i];
                BindGroup(group);
            }

            //分组优先
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (!string.IsNullOrEmpty(agent.GroupName))
                    BindAgent(agent);
            }

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (string.IsNullOrEmpty(agent.GroupName))
                    BindAgent(agent);
            }
        }

        public AgentItem AddAgent(AgentDesigner agent)
        {
            m_Agents.Add(agent);
            AgentItem agentItem = BindAgent(agent);
            m_TreeView.SelectedNode = agentItem.TreeNode;
            return agentItem;
        }

        public AgentItem BindAgent(AgentDesigner agent)
        {
            AgentItem agentItem = new AgentItem();
            agentItem.Agent = agent;

            if (string.IsNullOrEmpty(agent.GroupName))
            {
                TreeNode treeNode = m_TreeView.Nodes.Add(agent.AgentID);
                treeNode.Tag = agentItem;
                agentItem.GroupItem = null;
                agentItem.TreeNode = treeNode;
            }
            else
            {
                GroupItem groupItem = FindGroup(agent.GroupName);
                //添加分组
                if (groupItem == null)
                {
                    groupItem = AddGroup(agent.GroupName);
                }
                TreeNode treeNode = groupItem.TreeNode.Nodes.Add(agent.AgentID);
                treeNode.Tag = agentItem;
                agentItem.GroupItem = groupItem;
                agentItem.TreeNode = treeNode;
            }

            return agentItem;
        }

        public void UpdateAgent(AgentDesigner agent)
        {
            AgentItem agentItem = FindAgent(agent);
            if (agentItem != null)
                agentItem.TreeNode.Text = agent.AgentID;
        }

        public AgentItem FindAgent(AgentDesigner agent)
        {
            if (agent != null)
            {
                for (int i = 0; i < m_TreeView.Nodes.Count; i++)
                {
                    TreeNode treeNode = m_TreeView.Nodes[i];
                    if (treeNode.Tag is AgentItem)
                    {
                        AgentItem agentItem = treeNode.Tag as AgentItem;
                        if (agentItem.Agent == agent || agentItem.Agent.AgentID == agent.AgentID)
                            return agentItem;
                    }

                    if (treeNode.Nodes.Count > 0)
                    {
                        for (int ii = 0; ii < treeNode.Nodes.Count; ii++)
                        {
                            TreeNode treeNode_ii = treeNode.Nodes[ii];
                            if (treeNode_ii.Tag is AgentItem)
                            {
                                AgentItem agentItem = treeNode_ii.Tag as AgentItem;
                                if (agentItem.Agent == agent || agentItem.Agent.AgentID == agent.AgentID)
                                    return agentItem;
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

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent.GroupName == oldName)
                {
                    agent.GroupName = group.GroupName;
                }
            }
        }

        public AgentItem FindAgentItem(string agentID)
        {
            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                if (treeNode.Tag != null && treeNode.Tag is AgentItem)
                {
                    AgentItem agentItem = treeNode.Tag as AgentItem;
                    if (agentItem != null && agentItem.Agent.AgentID == agentID)
                        return agentItem;
                }

                if (treeNode.Nodes.Count > 0)
                {
                    for (int ii = 0; ii < treeNode.Nodes.Count; ii++)
                    {
                        TreeNode treeNode_ii = treeNode.Nodes[ii];
                        if (treeNode_ii.Tag is AgentItem)
                        {
                            AgentItem agentItem = treeNode_ii.Tag as AgentItem;
                            if (agentItem != null && agentItem.Agent.AgentID == agentID)
                                return agentItem;
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

            for (int i = m_Agents.Count - 1; i >= 0; i--)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent.GroupName == group.GroupName)
                    m_Agents.RemoveAt(i);
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

        public void RemoveAgent(string agentID)
        {
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent.AgentID == agentID)
                {
                    m_Agents.RemoveAt(i);
                    break;
                }
            }

            AgentItem agentItem = FindAgentItem(agentID);
            if (agentItem == null)
                return;

            int index = GetIndex(agentItem);
            m_TreeView.Nodes.Remove(agentItem.TreeNode);

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


        public int GetIndex(AgentItem agentItem)
        {
            if (agentItem == null)
                return -1;

            for (int i = 0; i < m_TreeView.Nodes.Count; i++)
            {
                TreeNode treeNode = m_TreeView.Nodes[i];
                if (treeNode.Tag != null && treeNode.Tag == agentItem)
                    return i;
            }

            return -1;
        }

        public void SetSelectItem(string agentID)
        {
            if (string.IsNullOrEmpty(agentID))
            {
                m_TreeView.SelectedNode = null;
            }
            else
            {
                AgentItem agentItem = FindAgentItem(agentID);
                if (agentItem != null)
                {
                    m_TreeView.SelectedNode = agentItem.TreeNode;
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
                if (selectedNode.Tag is AgentItem)
                {
                    AgentItem agentItem = selectedNode.Tag as AgentItem;
                    if (agentItem.GroupItem != null)
                    {
                        agentItem.TreeNode.Remove();
                        agentItem.GroupItem = null;
                        agentItem.Agent.GroupName = null;
                        m_TreeView.Nodes.Add(agentItem.TreeNode);
                        m_TreeView.SelectedNode = selectedNode;

                        AddLast(agentItem.Agent);
                    }
                }

                return;
            }

            //本身
            if (selectedNode == dropNode)
                return;

            //拖进指定组
            if (selectedNode.Tag is AgentItem && dropNode.Tag is GroupItem)
            {
                AgentItem selectedAgentItem = selectedNode.Tag as AgentItem;
                GroupItem dropGroupItem = dropNode.Tag as GroupItem;

                //相同组
                if (selectedAgentItem.GroupItem == dropGroupItem)
                    return;

                selectedAgentItem.TreeNode.Remove();
                selectedAgentItem.GroupItem = dropGroupItem;
                selectedAgentItem.Agent.GroupName = dropGroupItem.Group.GroupName;
                dropGroupItem.TreeNode.Nodes.Add(selectedAgentItem.TreeNode);

                AddLast(selectedAgentItem.Agent);
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
            else if (selectedNode.Tag is AgentItem && dropNode.Tag is AgentItem)
            {
                AgentItem selectedAgentItem = selectedNode.Tag as AgentItem;
                AgentItem dropAgentItem = dropNode.Tag as AgentItem;

                //没有组的节点拖拽
                if (selectedAgentItem.GroupItem == null)
                {
                    //加入组
                    if (dropAgentItem.GroupItem != null)
                    {
                        GroupItem groupItem = dropAgentItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(groupItem, dropNode);

                        selectedAgentItem.TreeNode.Remove();
                        groupItem.TreeNode.Nodes.Add(selectedNode);
                        selectedAgentItem.GroupItem = groupItem;
                        selectedAgentItem.Agent.GroupName = groupItem.Group.GroupName;

                        AddLast(selectedAgentItem.Agent);
                    }
                    //没组的交换
                    else
                    {
                        int selectedIndex = GetTreeViewIndex(selectedNode);
                        int dropIndex = GetTreeViewIndex(dropNode);

                        selectedAgentItem.TreeNode.Remove();
                        m_TreeView.Nodes.Insert(dropIndex, selectedNode);

                        //把agent1插入agent2的前面或者后面
                        InsertAgent(selectedAgentItem.Agent, dropAgentItem.Agent);
                    }
                }
                //有组的节点拖拽
                else if (selectedAgentItem.GroupItem != null)
                {
                    //删除组
                    if (dropAgentItem.GroupItem == null)
                    {
                        GroupItem groupItem = selectedAgentItem.GroupItem as GroupItem;
                        int dropIndex = GetTreeViewIndex(dropNode);

                        selectedAgentItem.TreeNode.Remove();

                        AgentDesigner lastAgent = null;
                        if (dropIndex < m_TreeView.Nodes.Count)
                        {
                            AgentItem agentItem = m_TreeView.Nodes[dropIndex].Tag as AgentItem;
                            lastAgent = agentItem.Agent;
                        }

                        m_TreeView.Nodes.Add(selectedAgentItem.TreeNode);
                        selectedAgentItem.GroupItem = null;
                        selectedAgentItem.Agent.GroupName = null;

                        AddLast(selectedAgentItem.Agent);
                    }
                    //组内交换
                    else if (selectedAgentItem.GroupItem.Group.GroupName == dropAgentItem.GroupItem.Group.GroupName)
                    {
                        GroupItem groupItem = selectedAgentItem.GroupItem as GroupItem;
                        int selectedIndex = GetGroupIndex(groupItem, selectedNode);
                        int dropIndex = GetGroupIndex(groupItem, dropNode);

                        selectedAgentItem.TreeNode.Remove();
                        dropAgentItem.TreeNode.Remove();

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

                        //把agent1插入agent2的前面或者后面
                        InsertAgent(selectedAgentItem.Agent, dropAgentItem.Agent);
                    }
                    //把拖拽节点加入Drop的组
                    else
                    {
                        GroupItem dropGroupItem = dropAgentItem.GroupItem as GroupItem;
                        int dropIndex = GetGroupIndex(dropGroupItem, dropNode);

                        selectedAgentItem.TreeNode.Remove();

                        AgentDesigner lastAgent = null;
                        if (dropIndex < dropGroupItem.TreeNode.Nodes.Count)
                        {
                            AgentItem agentItem = dropGroupItem.TreeNode.Nodes[dropIndex].Tag as AgentItem;
                            lastAgent = agentItem.Agent;
                        }

                        dropGroupItem.TreeNode.Nodes.Insert(dropIndex, selectedAgentItem.TreeNode);
                        selectedAgentItem.GroupItem = dropGroupItem;
                        selectedAgentItem.Agent.GroupName = dropGroupItem.Group.GroupName;

                        AddLast(selectedAgentItem.Agent);
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
