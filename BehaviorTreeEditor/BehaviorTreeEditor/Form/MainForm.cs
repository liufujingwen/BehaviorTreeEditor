using BehaviorTreeEditor.Properties;
using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class MainForm : Form
    {
        private static MainForm ms_Instance;

        public static MainForm Instance
        {
            get { return ms_Instance; }
        }

        public Brush HighLight = new SolidBrush(Color.FromArgb(255, 0, 120, 215));

        //工作区配置数据
        public WorkSpaceData WorkSpaceData;

        //节点类数据（节点模板）
        public NodeClasses NodeClasses;

        //节点类上一次保存的时候的内容，用于检测节点类dirty
        public string NodeClassesStringContent;

        //行为树数据
        public TreeData TreeData;

        //行为树数据上一次保存的时候的内容，用于检测行为树dirty
        public string BehaviorTreeDataStringContent;

        //当前选中的Agent
        public AgentDesigner SelectedAgent;

        private ContentUserControl m_ContentUserControl;
        private NodePropertyUserControl m_NodePropertyUserControl;

        //搜索字符串
        public string SearchStr;

        //当前显示的Agent
        private List<AgentDesigner> FilterAgentList = new List<AgentDesigner>();

        public TreeViewManager TreeViewManager;

        public MainForm()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            this.Width = (int)(width * 0.9f);
            this.Height = (int)(height * 0.8f);
            this.Location = new System.Drawing.Point((int)(width / 2f - this.Width / 2f),
                (int)(height / 2f - this.Height / 2f));

            m_ContentUserControl = new ContentUserControl();
            m_ContentUserControl.Dock = DockStyle.Fill;
            splitContainer3.Panel1.Controls.Clear();
            splitContainer3.Panel1.Controls.Add(m_ContentUserControl);

            m_NodePropertyUserControl = new NodePropertyUserControl();
            m_NodePropertyUserControl.Dock = DockStyle.Fill;
            splitContainer3.Panel2.Controls.Add(m_NodePropertyUserControl);

            Exec(OperationType.LoadWorkSpace);

            CreateTreeViewManager();
        }

        #region TreeView

        public void CreateTreeViewManager()
        {
            TreeViewManager = new TreeViewManager(this, treeView1, TreeData.Groups, TreeData.Agents);
            TreeViewManager.BindAgents();
        }

        /// <summary>
        /// 获取绑定指定数据的TreeNode
        /// </summary>
        /// <param name="obj">Agent</param>
        /// <returns></returns>
        public TreeNode GetTreeNode(AgentDesigner agent)
        {
            if (agent == null)
                return null;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode treeNode = treeView1.Nodes[i];
                if (treeNode == null)
                    continue;
                if (treeNode.Tag == agent)
                    return treeNode;
            }

            return null;
        }

        /// <summary>
        /// 获取TreeNode索引
        /// </summary>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public int GetTreeNodeIndex(TreeNode treeNode)
        {
            if (treeNode == null)
                return -1;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode temp = treeView1.Nodes[i];
                if (temp == null)
                    continue;
                if (temp == treeNode)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 通过索引获取TreeNode
        /// </summary>
        /// <returns></returns>
        public TreeNode GetTreeNodeByIndex(int index)
        {
            if (index >= 0 && index < treeView1.Nodes.Count)
                return treeView1.Nodes[index];
            return null;
        }

        /// <summary>
        /// 通过索引选中Agent
        /// </summary>
        /// <param name="index"></param>
        public void SetSelectedAgent(int index)
        {
            if (index == -1)
                return;

            if (index >= 0 && index < treeView1.Nodes.Count)
            {
                TreeNode treeNode = treeView1.Nodes[index];
                treeView1.SelectedNode = treeNode;
            }
        }

        /// <summary>
        /// 设置选中的agent
        /// </summary>
        /// <param name="agent"></param>
        public void SetSelectedAgent(AgentDesigner agent)
        {
            SelectedAgent = agent;
            if (m_ContentUserControl != null)
                m_ContentUserControl.SetSelectedAgent(SelectedAgent);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag is AgentItem)
            {
                AgentItem agentItem = treeView1.SelectedNode.Tag as AgentItem;
                SetSelectedAgent(agentItem.Agent);
            }
            else
            {
                SetSelectedAgent(null);
            }
        }

        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Exec(OperationType.CopyAgent);
                        break;
                    case Keys.V:
                        Exec(OperationType.PasteAgent);
                        break;
                    case Keys.Up:
                        Exec(OperationType.SwapAgent, true);
                        break;
                    case Keys.Down:
                        Exec(OperationType.SwapAgent, false);
                        break;
                    case Keys.S:
                        Exec(OperationType.Save);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F12:
                        Exec(OperationType.EditAgent);
                        break;
                    case Keys.N:
                        Exec(OperationType.AddAgent);
                        break;
                    case Keys.Delete:
                        Exec(OperationType.DeleteAgentOrGroup);
                        break;
                }
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // 0 添加分组
                    // 1 编辑分组
                    // 2 删除分组
                    // 3 分割线
                    // 4 复制Agent
                    // 5 粘贴Agent
                    // 6 新建Agent
                    // 7 编辑Agent
                    // 8 删除Agent

                    for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                        contextMenuStrip1.Items[i].Visible = true;

                    if (treeView1.SelectedNode != null)
                    {
                        if (treeView1.SelectedNode.Tag is GroupItem)
                        {
                            //隐藏 复制Agent
                            contextMenuStrip1.Items[4].Visible = false;
                            //隐藏 编辑Agent
                            contextMenuStrip1.Items[7].Visible = false;
                            //隐藏 删除Agent
                            contextMenuStrip1.Items[8].Visible = false;
                        }
                        else if (treeView1.SelectedNode.Tag is AgentItem)
                        {
                            //隐藏 添加分组
                            contextMenuStrip1.Items[1].Visible = false;
                            //隐藏 删除分组
                            contextMenuStrip1.Items[2].Visible = false;
                        }
                    }
                    else
                    {
                        contextMenuStrip1.Items[4].Visible = false;
                        contextMenuStrip1.Items[7].Visible = false;
                        contextMenuStrip1.Items[8].Visible = false;
                    }

                    contextMenuStrip1.Show(treeView1, e.Location);
                }
            }
            else if (e.Clicks == 2)
            {
                if (treeView1.SelectedNode != null)
                {
                    Exec(OperationType.EditAgent);
                }
                else
                {
                    Exec(OperationType.AddAgent);
                }
            }
        }

        /// <summary>
        /// 添加Agent
        /// </summary>
        private void AddAgent()
        {
            AgentDesigner agent = new AgentDesigner();
            string agentID = "NewAgent_" + DateTime.Now.Ticks;
            do
            {
                agentID = "NewAgent_" + DateTime.Now.Ticks;
            } while (TreeData.ExistAgent(agentID));

            agent.AgentID = agentID;

            //创建开始节点
            NodeDesigner startNode = null;
            NodeClass nodeClass = NodeClasses.FindNode("Sequence");
            if (nodeClass != null)
            {
                Rect rect = new Rect(EditorUtility.Center.x, EditorUtility.Center.y, EditorUtility.NodeWidth,
                    EditorUtility.NodeHeight);
                startNode = new NodeDesigner(nodeClass.Label, nodeClass.ClassType, rect);
                startNode.ID = agent.GenNodeID();
                startNode.StartNode = true;
                startNode.NodeType = nodeClass.NodeType;
                startNode.Describe = nodeClass.Describe;

                //创建字段
                for (int i = 0; i < nodeClass.Fields.Count; i++)
                {
                    NodeField nodeField = nodeClass.Fields[i];
                    FieldDesigner field = EditorUtility.CreateFieldByNodeField(nodeField);
                    if (field == null)
                        continue;
                    startNode.Fields.Add(field);
                }

                agent.AddNode(startNode);
            }

            //创建空操作节点
            NodeClass noopClass = NodeClasses.FindNode("Noop");
            if (startNode != null && noopClass != null)
            {
                Rect rect = new Rect(EditorUtility.Center.x + 250, EditorUtility.Center.y, EditorUtility.NodeWidth,
                    EditorUtility.NodeHeight);
                NodeDesigner noopNode = new NodeDesigner(noopClass.Label, noopClass.ClassType, rect);
                noopNode.ID = agent.GenNodeID();
                noopNode.NodeType = noopClass.NodeType;
                noopNode.Describe = noopClass.Describe;
                agent.AddNode(noopNode);

                startNode.AddChildNode(noopNode);
            }

            TreeViewManager.AddAgent(agent);
        }

        /// <summary>
        /// 编辑Agent
        /// </summary>
        private void EditAgent()
        {
            if (SelectedAgent == null)
                return;

            EditAgentForm editAgentForm = new EditAgentForm(SelectedAgent);
            editAgentForm.ShowDialog();
        }

        public class AgentListContent
        {
            private List<AgentDesigner> m_DataList = new List<AgentDesigner>();

            public List<AgentDesigner> DataList
            {
                get { return m_DataList; }
            }
        }

        /// <summary>
        /// 复制Agent
        /// </summary>
        private void CopyAgent()
        {
            if (treeView1.SelectedNode != null)
            {
                if (!(treeView1.SelectedNode.Tag is AgentItem))
                {
                    ShowMessage("只能复制Agent");
                    return;
                }

                AgentListContent content = new AgentListContent();
                content.DataList.Add((treeView1.SelectedNode.Tag as AgentItem).Agent);

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个枚举！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择一个进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择一个进行复制！！！", "警告");
            }
        }

        /// <summary>
        /// 粘贴行为树
        /// </summary>
        private void PasteAgent()
        {
            try
            {
                AgentListContent content = XmlUtility.StringToObject<AgentListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    AgentDesigner agent = content.DataList[i];
                    string agentID = "NewAgent_" + DateTime.Now.Ticks;
                    do
                    {
                        agentID = "NewAgent_" + DateTime.Now.Ticks;
                    } while (TreeData.ExistAgent(agentID));

                    agent.AgentID = agentID;
                    TreeViewManager.AddAgent(agent);
                }

                ShowInfo("您粘贴了" + content.DataList.Count + "棵行为树！！！");
            }
            catch (Exception ex)
            {
                ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        /// <summary>
        /// 刷新所有数据
        /// </summary>
        private void RefreshAgents()
        {
        }

        //交换Agent位置
        private void SwapAgent(bool up)
        {
            if (!string.IsNullOrEmpty(SearchStr))
            {
                ShowInfo("搜索模式不可以交换行为树,请把搜索框内容删除后再进行交换");
                ShowMessage("搜索模式不可以交换行为树,请把搜索框内容删除后再进行交换", "警告");
                return;
            }

            if (treeView1.SelectedNode == null)
            {
                ShowInfo("请选择一条记录进行交换");
                ShowMessage("请选择一条记录进行交换", "警告");
                return;
            }

            int selectIdx = GetTreeNodeIndex(treeView1.SelectedNode);
            if (up)
            {
                //第一个不能往上交换
                if (selectIdx == 0)
                    return;

                int preIdx = selectIdx - 1;

                //交换数据
                AgentDesigner preAgent = TreeData.Agents[preIdx];
                AgentDesigner selectedAgent = TreeData.Agents[selectIdx];
                TreeData.Agents[preIdx] = selectedAgent;
                TreeData.Agents[selectIdx] = preAgent;

                TreeNode preTreeNode = treeView1.Nodes[preIdx];
                TreeNode selectedTreeNode = treeView1.SelectedNode;

                treeView1.Nodes.RemoveAt(selectIdx);
                treeView1.Nodes.RemoveAt(preIdx);

                treeView1.Nodes.Insert(preIdx, selectedTreeNode);
                treeView1.Nodes.Insert(selectIdx, preTreeNode);

                if (treeView1.SelectedNode != selectedTreeNode)
                    treeView1.SelectedNode = selectedTreeNode;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == treeView1.Nodes.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                //交换数据
                AgentDesigner nextAgent = TreeData.Agents[nextIdx];
                AgentDesigner selectedAgent = TreeData.Agents[selectIdx];
                TreeData.Agents[nextIdx] = selectedAgent;
                TreeData.Agents[selectIdx] = nextAgent;

                TreeNode nextTreeNode = treeView1.Nodes[nextIdx];
                TreeNode selectedTreeNode = treeView1.SelectedNode;

                treeView1.Nodes.RemoveAt(nextIdx);
                treeView1.Nodes.RemoveAt(selectIdx);

                treeView1.Nodes.Insert(selectIdx, nextTreeNode);
                treeView1.Nodes.Insert(nextIdx, selectedTreeNode);

                if (treeView1.SelectedNode != selectedTreeNode)
                    treeView1.SelectedNode = selectedTreeNode;
            }

            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
        }

        //刷新Agent
        private void UpdateAgent(AgentDesigner agent)
        {
            if (agent == null)
                return;

            TreeViewManager.UpdateAgent(agent);
        }

        //重置数据
        private void Reset()
        {
            EditorUtility.NodeClassDic.Clear();
            treeView1.Nodes.Clear();
            NodeClasses = new NodeClasses();
            NodeClasses.ResetEnums();
            NodeClasses.ResetNodes();
            TreeData = new TreeData();
            SetSelectedAgent(null);
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        private void AddGroup()
        {
            AddGroup addGroup = new AddGroup();
            addGroup.ShowDialog();
        }

        //编辑分组
        private void EditGroup()
        {
            if (treeView1.SelectedNode == null)
                return;
            if (!(treeView1.SelectedNode.Tag is GroupItem))
                return;

            GroupItem groupItem = treeView1.SelectedNode.Tag as GroupItem;
            EditGroupForm editGroup = new EditGroupForm(groupItem.Group);
            editGroup.ShowDialog();
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        private void DeleteGroup()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (!(treeView1.SelectedNode.Tag is GroupItem))
                return;

            GroupItem groupItem = treeView1.SelectedNode.Tag as GroupItem;
            Group group = groupItem.Group;

            if (MessageBox.Show(string.Format("确定删除分组[{0}]吗?", group.GroupName), "提示",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TreeViewManager.DeleteGroup(group);
                ShowInfo("删除分组[{0}]", group.GroupName);
            }
        }

        //更新Group
        private void UpdateGroup(string oldName, Group group)
        {
            TreeViewManager.UpdateGroup(oldName, group);
        }

        /// <summary>
        /// 删除Agent
        /// </summary>
        private void DeleteAgent()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (!(treeView1.SelectedNode.Tag is AgentItem))
                return;

            AgentDesigner agent = (treeView1.SelectedNode.Tag as AgentItem).Agent;

            if (MessageBox.Show(string.Format("确定删除行为树{0}吗?", agent.AgentID), "提示",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TreeViewManager.RemoveAgent(agent.AgentID);
            }
        }

        /// <summary>
        /// 删除Agent
        /// </summary>
        private void DeleteAgentOrGroup()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag is AgentItem)
            {
                DeleteAgent();
            }
            else
            {
                DeleteGroup();
            }
        }

        #endregion

        #region ToolStrip or Menu

        private void 新建工作区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.NewWorkSpace);
        }

        private void 打开工作区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.OpenWorkSpace);
        }

        private void 编辑工作区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.EditWorkSpace);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestForm testForm = new TestForm();
            testForm.ShowDialog();
        }

        private void 类视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassForm classForm = new ClassForm();
            classForm.ShowDialog();
        }

        private void 枚举视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumForm enumForm = new EnumForm();
            enumForm.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.DeleteAgent);
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.AddAgent);
        }

        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exec(OperationType.CopyAgent);
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.PasteAgent);
        }

        private void 添加分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.AddGroup);
        }

        private void 编辑分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.EditGroup);
        }

        private void 删除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.DeleteGroup);
        }

        #endregion

        //添加行为树
        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.AddAgent);
        }

        //保存所有数据
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.Save);
        }

        //删除行为树
        private void deleteAgentToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.DeleteAgentOrGroup);
        }

        //刷新
        private void freshToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.Refresh);
        }

        /// <summary>
        /// 添加行为树Item
        /// </summary>
        /// <param name="agent">agent</param>
        /// <returns>true:添加成功</returns>
        public bool AddAgentItem(AgentDesigner agent)
        {
            if (agent == null)
                return false;
            TreeNode treeNode = treeView1.Nodes.Add(agent.AgentID);
            treeNode.Tag = agent;
            treeView1.SelectedNode = treeNode;
            return true;
        }

        public bool Exec(OperationType opration, params object[] args)
        {
            switch (opration)
            {
                case OperationType.LoadWorkSpace:
                    //加载工作区
                    LoadWorkSpace();
                    break;
                case OperationType.NewWorkSpace:
                    //新建工作区
                    NewWorkSpace();
                    break;
                case OperationType.OpenWorkSpace:
                    //打开工作区
                    OpenWorkSpace();
                    break;
                case OperationType.EditWorkSpace:
                    //编辑工作区
                    EditWorkSpace();
                    break;
                case OperationType.Save:
                    //保存
                    Save();
                    break;
                case OperationType.EditAgent:
                    //编辑Agent
                    EditAgent();
                    break;
                case OperationType.AddAgent:
                    //添加行为树
                    AddAgent();
                    break;
                case OperationType.CopyAgent:
                    //复制Agent
                    CopyAgent();
                    break;
                case OperationType.PasteAgent:
                    PasteAgent();
                    break;
                case OperationType.Refresh:
                    //刷新所有Agent
                    RefreshAgents();
                    break;
                case OperationType.DeleteAgent:
                    //删除选中的Agent
                    DeleteAgent();
                    break;
                case OperationType.DeleteAgentOrGroup:
                    //删除选中的Agent或者分组
                    DeleteAgentOrGroup();
                    break;
                case OperationType.SwapAgent:
                    //交换Agent位置
                    SwapAgent((bool)args[0]);
                    break;
                case OperationType.UpdateAgent:
                    UpdateAgent((AgentDesigner)args[0]);
                    break;
                case OperationType.Reset:
                    Reset();
                    break;
                case OperationType.AddGroup:
                    //添加分组
                    AddGroup();
                    break;
                case OperationType.EditGroup:
                    //编辑分组
                    EditGroup();
                    break;
                case OperationType.UpdateGroup:
                    UpdateGroup((string)args[0], (Group)args[1]);
                    break;
                case OperationType.DeleteGroup:
                    //删除分组
                    DeleteGroup();
                    break;
            }

            return true;
        }

        //加载工作区
        public void LoadWorkSpace()
        {
            this.Text = Settings.Default.EditorTitle;
            if (string.IsNullOrEmpty(Settings.Default.WorkDirectory) ||
                string.IsNullOrEmpty(Settings.Default.WorkSpaceName))
                return;
            WorkSpaceData = XmlUtility.Read<WorkSpaceData>(GetWorkSpacePath());
            if (WorkSpaceData != null)
                this.Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);

            //读取行为树类信息
            NodeClasses = XmlUtility.Read<NodeClasses>(GetNodeClassPath());
            if (NodeClasses == null)
            {
                NodeClasses = new NodeClasses();
                NodeClasses.ResetEnums();
                NodeClasses.ResetNodes();
                XmlUtility.Save(MainForm.Instance.GetNodeClassPath(), MainForm.Instance.NodeClasses);
            }

            NodeClassesStringContent = XmlUtility.ObjectToString(NodeClasses);

            //读取行为树数据
            LoadBehaviorTreeData();
        }

        //新建工作区
        public void NewWorkSpace()
        {
            NewWorkSpaceForm newWorkSpaceForm = new NewWorkSpaceForm();
            newWorkSpaceForm.ShowDialog();
        }

        //打开工作区
        public void OpenWorkSpace()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = string.Format("工作区配置|*{0}", Settings.Default.WorkSpaceSetupSuffix);
            openFileDialog.Title = "打开工作区";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string workSpaceFile = openFileDialog.FileName.Trim();
                if (File.Exists(workSpaceFile))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(workSpaceFile);
                    string workDirectory = dirInfo.Parent.FullName;
                    WorkSpaceData = XmlUtility.Read<WorkSpaceData>(workSpaceFile);
                    if (WorkSpaceData != null)
                    {
                        Settings.Default.WorkDirectory = workDirectory;
                        Settings.Default.WorkSpaceName = WorkSpaceData.WorkSpaceName;
                        Settings.Default.Save();
                        Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);
                        ShowInfo("打开工作区,时间：" + DateTime.Now);

                        //读取行为树类信息
                        NodeClasses = XmlUtility.Read<NodeClasses>(GetNodeClassPath());
                        if (NodeClasses == null)
                        {
                            NodeClasses = new NodeClasses();
                            NodeClasses.ResetNodes();
                            XmlUtility.Save(GetNodeClassPath(), NodeClasses);
                        }

                        NodeClassesStringContent = XmlUtility.ObjectToString(NodeClasses);

                        //读取行为树数据
                        LoadBehaviorTreeData();

                        CreateTreeViewManager();
                    }
                }
            }
        }

        //编辑工作区
        public void EditWorkSpace()
        {
            EditWorkSpaceForm editWorkSpaceForm = new EditWorkSpaceForm();
            editWorkSpaceForm.ShowDialog();
        }

        //保存
        public void Save()
        {
            //节点类移除未定义的枚举字段
            NodeClasses.RemoveUnDefineEnumField();

            //移除未定义的节点
            TreeData.RemoveUnDefineNode();

            //修正数据，和模板的保持一致
            TreeData.AjustData();

            //检验枚举
            VerifyInfo verifyEnum = NodeClasses.VerifyEnum();
            if (verifyEnum.HasError)
            {
                ShowMessage(verifyEnum.Msg);
                ShowInfo(verifyEnum.Msg);
                return;
            }

            //检验节点类
            VerifyInfo verifyNodeClass = NodeClasses.VerifyNodeClass();
            if (verifyNodeClass.HasError)
            {
                ShowMessage(verifyNodeClass.Msg);
                ShowInfo(verifyNodeClass.Msg);
                return;
            }

            //校验行为树
            VerifyInfo verifyBehaviorTree = TreeData.VerifyBehaviorTree();
            if (verifyBehaviorTree.HasError)
            {
                ShowMessage(verifyBehaviorTree.Msg);
                ShowInfo(verifyBehaviorTree.Msg);
                return;
            }

            if (XmlUtility.Save(GetNodeClassPath(), NodeClasses))
            {
                NodeClassesStringContent = XmlUtility.ObjectToString(NodeClasses);
            }


            if (XmlUtility.Save(GetBehaviorTreeDataPath(), TreeData))
            {
                BehaviorTreeDataStringContent = XmlUtility.ObjectToString(TreeData);
            }

            //序列化成二进制
            BehaviorTreeData.TreeData treeData = EditorUtility.CreateTreeData(TreeData);
            if (treeData != null)
            {
                string savePath = GetNodeDataSavePath();
                if (File.Exists(savePath))
                    File.Delete(savePath);
                BehaviorTreeData.Serializer.SerializeToFile(treeData, savePath);
            }

            ShowInfo("保存成功 时间:" + DateTime.Now);
        }

        /// <summary>
        /// 获取数据保存路径
        /// </summary>
        /// <returns></returns>
        public string GetDataSavePath()
        {
            return Path.Combine(Settings.Default.NodeDataSavePath,
                Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
        }

        /// <summary>
        /// 获取工作区配置路径
        /// </summary>
        /// <returns></returns>
        public string GetWorkSpacePath()
        {
            return Path.Combine(Settings.Default.WorkDirectory,
                Settings.Default.WorkSpaceName + Settings.Default.WorkSpaceSetupSuffix);
        }

        /// <summary>
        /// 获取行为树数据保存路径
        /// </summary>
        /// <returns></returns>
        public string GetBehaviorTreeDataPath()
        {
            return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.BehaviorTreeDataFile);
        }

        /// <summary>
        /// 获取行为树二进制数据导出路径
        /// </summary>
        /// <returns></returns>
        public string GetNodeDataSavePath()
        {
            return Path.Combine(Settings.Default.NodeDataSavePath, Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
        }

        /// <summary>
        /// 获取行为树类信息路径
        /// </summary>
        /// <returns></returns>
        public string GetNodeClassPath()
        {
            return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.NodeClassFile);
        }

        /// <summary>
        /// 在状态栏显示信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void ShowInfo(String info, params object[] args)
        {
            string content = string.Format(info, args);
            toolStripStatusLabel1.Text = content;
        }

        /// <summary>
        /// 弹出消息框
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="title">标题</param>
        public void ShowMessage(string msg, string title = "提示")
        {
            MessageBox.Show(msg, title);
        }

        private void MainForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        Exec(OperationType.Save);
                        break;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool nodeClassDirty = false;
                string tempNodeClassesStringContent = XmlUtility.ObjectToString(NodeClasses);
                if (tempNodeClassesStringContent != NodeClassesStringContent)
                {
                    nodeClassDirty = true;
                }

                bool behaviorTreeDirty = false;
                string tempBehaviorTreeDataStringContent = XmlUtility.ObjectToString(TreeData);
                if (tempBehaviorTreeDataStringContent != BehaviorTreeDataStringContent)
                {
                    behaviorTreeDirty = true;
                }

                if (nodeClassDirty || behaviorTreeDirty)
                {
                    DialogResult result = MessageBox.Show(Settings.Default.SaveWarnning, Settings.Default.EditorTitle,
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                        Exec(OperationType.Save);
                }
            }

            Settings.Default.Save();
        }

        public void LoadBehaviorTreeData()
        {
            //读取行为树数据
            TreeData = XmlUtility.Read<TreeData>(GetBehaviorTreeDataPath());
            if (TreeData == null)
            {
                TreeData = new TreeData();
                XmlUtility.Save(GetBehaviorTreeDataPath(), TreeData);
            }

            BehaviorTreeDataStringContent = XmlUtility.ObjectToString(TreeData);

            if (TreeData.Agents.Count > 0)
            {
                for (int i = 0; i < TreeData.Agents.Count; i++)
                {
                    AgentDesigner agent = TreeData.Agents[i];
                    if (agent != null)
                    {
                        if (agent.Nodes.Count > 0)
                        {
                            for (int ii = 0; ii < agent.Nodes.Count; ii++)
                            {
                                NodeDesigner node = agent.Nodes[ii];
                                if (node.Transitions.Count > 0)
                                {
                                    for (int iii = 0; iii < node.Transitions.Count; iii++)
                                    {
                                        Transition transition = node.Transitions[iii];
                                        NodeDesigner fromNode = agent.FindNodeByID(transition.FromNodeID);
                                        NodeDesigner toNode = agent.FindNodeByID(transition.ToNodeID);
                                        transition.Set(toNode, fromNode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(HighLight, Rectangle.Inflate(e.Node.Bounds, 2, 0));
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, Rectangle.Inflate(e.Bounds, 2, 0));
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void playToolStripButton_Click(object sender, EventArgs e)
        {
            DebugManager.Instance.Debug(SelectedAgent);
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            DebugManager.Instance.Stop();
        }
    }
}