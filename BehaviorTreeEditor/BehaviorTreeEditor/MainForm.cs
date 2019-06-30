using BehaviorTreeEditor.Properties;
using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
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

        //工作区配置数据
        public WorkSpaceData WorkSpaceData;
        //节点类数据（节点模板）
        public NodeClasses NodeClasses;
        //节点类数据是否为Dirty
        public bool NodeClassDirty;
        //行为树数据
        public BehaviorTreeData BehaviorTreeData;
        //上一次保存的时候的内容，用于检测行为树dirty
        public string BehaviorTreeDataStringContent;
        //当前选中的Agent
        public AgentDesigner SelectedAgent;

        private ContentUserControl m_ContentUserControl;


        public MainForm()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            Exec(OperationType.LoadWorkSpace);
            m_ContentUserControl = new ContentUserControl();
            m_ContentUserControl.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(m_ContentUserControl);

            BindAgents();
        }

        #region TreeView

        /// <summary>
        /// 绑定Agents
        /// </summary>
        private void BindAgents()
        {
            treeView1.Nodes.Clear();
            for (int i = 0; i < BehaviorTreeData.Agents.Count; i++)
            {
                AgentDesigner agent = BehaviorTreeData.Agents[i];
                TreeNode treeNode = treeView1.Nodes.Add(agent.AgentID);
                treeNode.Tag = agent;
            }
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
            if (SelectedAgent == agent)
                return;

            SelectedAgent = agent;
            m_ContentUserControl.SetSelectedAgent(SelectedAgent);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            AgentDesigner agent = treeView1.SelectedNode.Tag as AgentDesigner;
            SetSelectedAgent(agent);
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
                        Exec(OperationType.DeleteAgent);
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
                    contextMenuStrip1.Tag = treeView1;
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

        public class AgentListContent
        {
            private List<AgentDesigner> m_DataList = new List<AgentDesigner>();
            public List<AgentDesigner> DataList { get { return m_DataList; } }
        }

        /// <summary>
        /// 复制Agent
        /// </summary>
        private void CopyAgent()
        {
            if (treeView1.SelectedNode != null)
            {
                AgentListContent content = new AgentListContent();
                content.DataList.Add((AgentDesigner)treeView1.SelectedNode.Tag);

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
                    }
                    while (BehaviorTreeData.ExistAgent(agentID));
                    agent.AgentID = agentID;
                    BehaviorTreeData.AddAgent(agent);
                    AddAgentItem(agent);
                }
                MainForm.Instance.NodeClassDirty = true;
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "棵行为树！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        /// <summary>
        /// 编辑Agent
        /// </summary>
        private void EditAgent()
        {
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
                AgentDesigner preAgent = BehaviorTreeData.Agents[preIdx];
                AgentDesigner selectedAgent = BehaviorTreeData.Agents[selectIdx];
                BehaviorTreeData.Agents[preIdx] = selectedAgent;
                BehaviorTreeData.Agents[selectIdx] = preAgent;

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
                AgentDesigner nextAgent = BehaviorTreeData.Agents[nextIdx];
                AgentDesigner selectedAgent = BehaviorTreeData.Agents[selectIdx];
                BehaviorTreeData.Agents[nextIdx] = selectedAgent;
                BehaviorTreeData.Agents[selectIdx] = nextAgent;

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

        /// <summary>
        /// 删除Agent
        /// </summary>
        private void DeleteAgent()
        {
            if (treeView1.SelectedNode == null)
                return;

            int index = GetTreeNodeIndex(treeView1.SelectedNode);
            AgentDesigner agentDesigner = treeView1.SelectedNode.Tag as AgentDesigner;

            if (agentDesigner == null)
                return;

            if (MessageBox.Show(string.Format("确定删除行为树{0}吗?", agentDesigner.AgentID), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                BehaviorTreeData.RemoveAgent(agentDesigner);
                treeView1.Nodes.RemoveAt(index);
                TreeNode treeNode = GetTreeNodeByIndex(index);
                if (treeNode != null)
                {
                    SetSelectedAgent(index);
                }
                else
                {
                    index--;
                    if (index >= 0)
                    {
                        SetSelectedAgent(index);
                    }
                }
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
            testForm.Show();
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
            Exec(OperationType.DeleteAgent);
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
                case OperationType.SwapAgent:
                    //交换Agent位置
                    SwapAgent((bool)args[0]);
                    break;
            }

            return true;
        }

        //加载工作区
        public void LoadWorkSpace()
        {
            this.Text = Settings.Default.EditorTitle;
            if (string.IsNullOrEmpty(Settings.Default.WorkDirectory) || string.IsNullOrEmpty(Settings.Default.WorkSpaceName))
                return;
            WorkSpaceData = XmlUtility.Read<WorkSpaceData>(GetWorkSpacePath());
            if (WorkSpaceData != null)
                this.Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);

            //读取行为树类信息
            NodeClasses = XmlUtility.Read<NodeClasses>(GetNodeClassPath());
            if (NodeClasses == null)
            {
                NodeClasses = new NodeClasses();
                NodeClasses.ResetNodes();
                XmlUtility.Save(MainForm.Instance.GetNodeClassPath(), MainForm.Instance.NodeClasses);
            }
            NodeClassDirty = false;

            //读取行为树数据
            BehaviorTreeData = XmlUtility.Read<BehaviorTreeData>(GetBehaviorTreeDataPath());
            if (BehaviorTreeData == null)
            {
                BehaviorTreeData = new BehaviorTreeData();
                XmlUtility.Save(GetBehaviorTreeDataPath(), BehaviorTreeData);
            }
            BehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);
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
                        NodeClassDirty = false;

                        //读取行为树数据
                        BehaviorTreeData = XmlUtility.Read<BehaviorTreeData>(GetBehaviorTreeDataPath());
                        if (BehaviorTreeData == null)
                        {
                            BehaviorTreeData = new BehaviorTreeData();
                            XmlUtility.Save(GetBehaviorTreeDataPath(), BehaviorTreeData);
                        }
                        BehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);
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
            if (NodeClasses != null)
            {
                if (XmlUtility.Save(GetNodeClassPath(), NodeClasses))
                {
                    NodeClassDirty = false;
                }
            }

            if (BehaviorTreeData != null)
            {
                if (XmlUtility.Save(GetBehaviorTreeDataPath(), BehaviorTreeData))
                {
                    BehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);
                }
            }

            ShowInfo("保存成功 时间:" + DateTime.Now);
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
            }
            while (BehaviorTreeData.ExistAgent(agentID));
            agent.AgentID = agentID;
            BehaviorTreeData.AddAgent(agent);
            AddAgentItem(agent);
        }

        /// <summary>
        /// 获取数据保存路径
        /// </summary>
        /// <returns></returns>
        public string GetDataSavePath()
        {
            return Path.Combine(Settings.Default.NodeDataSavePath, Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
        }
        /// <summary>
        /// 获取工作区配置路径
        /// </summary>
        /// <returns></returns>
        public string GetWorkSpacePath()
        {
            return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.WorkSpaceName + Settings.Default.WorkSpaceSetupSuffix);
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
        public void ShowInfo(String info)
        {
            toolStripStatusLabel1.Text = info;
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool behaviorTreeDirty = false;
                string stringContent = XmlUtility.ObjectToString(BehaviorTreeData);
                if (stringContent != BehaviorTreeDataStringContent)
                {
                    behaviorTreeDirty = true;
                }

                if (NodeClassDirty || behaviorTreeDirty)
                {
                    DialogResult result = MessageBox.Show(Settings.Default.SaveWarnning, Settings.Default.EditorTitle, MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                        Exec(OperationType.Save);
                }
            }

            Settings.Default.Save();
            NodeClassDirty = false;
        }
    }
}