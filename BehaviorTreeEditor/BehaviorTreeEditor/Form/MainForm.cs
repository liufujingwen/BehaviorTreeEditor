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

        //节点模板数据
        public NodeTemplate NodeTemplate;

        //节点类上一次保存的时候的内容，用于检测节点类dirty
        public string NodeTemplateStringContent;

        //行为树数据
        public BehaviorTreeDataDesigner BehaviorTreeData;

        //行为树数据上一次保存的时候的内容，用于检测行为树dirty
        public string BehaviorTreeDataStringContent;

        //当前选中的行为树
        public BehaviorTreeDesigner SelectedBehaviorTree;

        private ContentUserControl m_ContentUserControl;
        private NodePropertyUserControl m_NodePropertyUserControl;

        //搜索字符串
        public string SearchStr;

        //当前显示的行为树
        private List<BehaviorTreeDesigner> FilterBehaviorTreeList = new List<BehaviorTreeDesigner>();

        public TreeViewManager TreeViewManager;

        public MainForm()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void NewMainForm_Load(object sender, EventArgs e)
        {
            toolStripMenuItem7.Checked = Settings.Default.ShowContent;
            toolStripMenuItem8.Checked = Settings.Default.ShowDescribe;

            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;

            Width = (int)(width * 0.9f);
            Height = (int)(height * 0.8f);
            Location = new Point((int)(width / 2f - this.Width / 2f), (int)(height / 2f - this.Height / 2f));

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
            if (BehaviorTreeData == null)
                return;

            TreeViewManager = new TreeViewManager(this, treeView1, BehaviorTreeData.Groups, BehaviorTreeData.BehaviorTrees);
            TreeViewManager.BindBehaviorTrees();
        }

        /// <summary>
        /// 获取绑定指定数据的TreeNode
        /// </summary>
        /// <param name="obj">行为树</param>
        /// <returns></returns>
        public TreeNode GetTreeNode(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                return null;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode treeNode = treeView1.Nodes[i];
                if (treeNode == null)
                    continue;
                if (treeNode.Tag == behaviorTree)
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
        /// 通过索引选中行为树
        /// </summary>
        /// <param name="index"></param>
        public void SetSelectedBehaviorTree(int index)
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
        /// 设置选中的行为树
        /// </summary>
        /// <param name="behaviorTree"></param>
        public void SetSelectedBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            SelectedBehaviorTree = behaviorTree;
            if (m_ContentUserControl != null)
                m_ContentUserControl.SetSelectedBehaviorTree(SelectedBehaviorTree);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag is BehaviorTreeItem)
            {
                BehaviorTreeItem behaviorTreeItem = treeView1.SelectedNode.Tag as BehaviorTreeItem;
                SetSelectedBehaviorTree(behaviorTreeItem.BehaviorTree);
            }
            else
            {
                SetSelectedBehaviorTree(null);
            }
        }

        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:

                        if (treeView1.SelectedNode.Tag is BehaviorTreeItem behaviorTreeItem)
                            Exec(OperationType.CopyBehaviorTree);
                        else if (treeView1.SelectedNode.Tag is GroupItem groupItem)
                            Exec(OperationType.CopyGroup);

                        break;
                    case Keys.V:

                        //尝试粘贴行为树
                        try
                        {
                            BehaviorTreeListContent behaviorTreeContent = XmlUtility.StringToObject<BehaviorTreeListContent>(Clipboard.GetText());
                            if (behaviorTreeContent != null)
                            {
                                Exec(OperationType.PasteBehaviorTree);
                                return;
                            }
                        }
                        catch { }

                        //尝试粘贴分组
                        try
                        {
                            BehaviorTreeGroupListContent groupContent = XmlUtility.StringToObject<BehaviorTreeGroupListContent>(Clipboard.GetText());
                            if (groupContent != null)
                            {
                                Exec(OperationType.PasteBehaviorTreeGroup);
                                return;
                            }

                        }
                        catch { }

                        ShowInfo("无法进行粘贴，错误信息：");
                        ShowMessage("无法进行粘贴，错误信息：");

                        break;
                    case Keys.Up:
                        Exec(OperationType.SwapBehaviorTree, true);
                        break;
                    case Keys.Down:
                        Exec(OperationType.SwapBehaviorTree, false);
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
                        Exec(OperationType.EditBehaviorTree);
                        break;
                    case Keys.N:
                        Exec(OperationType.AddBehaviorTree);
                        break;
                    case Keys.Delete:
                        Exec(OperationType.DeleteBehaviorTreeOrGroup);
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
                    // 4 复制行为树
                    // 5 粘贴行为树
                    // 6 新建行为树
                    // 7 编辑行为树
                    // 8 删除行为树

                    for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
                        contextMenuStrip1.Items[i].Visible = true;

                    if (treeView1.SelectedNode != null)
                    {
                        if (treeView1.SelectedNode.Tag is GroupItem)
                        {
                            //隐藏 复制行为树
                            contextMenuStrip1.Items[4].Visible = false;
                            //隐藏 编辑行为树
                            contextMenuStrip1.Items[7].Visible = false;
                            //隐藏 删除行为树
                            contextMenuStrip1.Items[8].Visible = false;
                        }
                        else if (treeView1.SelectedNode.Tag is BehaviorTreeItem)
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
                else if (e.Button == MouseButtons.Left)
                {
                    TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                    treeView1.SelectedNode = node;
                }
            }
            else if (e.Clicks == 2)
            {
                if (treeView1.SelectedNode != null)
                {
                    Exec(OperationType.EditBehaviorTree);
                }
                else
                {
                    Exec(OperationType.AddBehaviorTree);
                }
            }
        }

        /// <summary>
        /// 添加行为树
        /// </summary>
        private void AddBehaviorTree()
        {
            if (WorkSpaceData == null)
            {
                ShowMessage("当前没有工作空间，请新建或者打开工作空间！");
                return;
            }

            if (BehaviorTreeData == null)
                return;

            string group = string.Empty;
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is GroupItem)
                {
                    GroupItem groupItem = treeView1.SelectedNode.Tag as GroupItem;
                    group = groupItem.Group.GroupName;
                }
                else if (treeView1.SelectedNode.Tag is BehaviorTreeItem)
                {
                    BehaviorTreeItem behaviorTreeItem = treeView1.SelectedNode.Tag as BehaviorTreeItem;
                    if (behaviorTreeItem.GroupItem != null)
                        group = behaviorTreeItem.GroupItem.Group.GroupName;
                }
            }

            BehaviorTreeDesigner behaviorTree = new BehaviorTreeDesigner();
            string behaviorTreeID = "New_" + DateTime.Now.Ticks;
            do
            {
                behaviorTreeID = "New_" + DateTime.Now.Ticks;
            } while (BehaviorTreeData.ExistBehaviorTree(behaviorTreeID));

            behaviorTree.ID = behaviorTreeID;

            //创建开始节点
            NodeDesigner startNode = null;
            NodeDefine nodeDefine = NodeTemplate.FindNode("Sequence");
            if (nodeDefine != null)
            {
                Rect rect = new Rect(EditorUtility.Center.x, EditorUtility.Center.y, EditorUtility.NodeWidth,
                    EditorUtility.NodeHeight);
                startNode = new NodeDesigner(nodeDefine.Label, nodeDefine.ClassType, rect);
                startNode.ID = behaviorTree.GenNodeID();
                startNode.StartNode = true;
                startNode.NodeType = nodeDefine.NodeType;

                //创建字段
                for (int i = 0; i < nodeDefine.Fields.Count; i++)
                {
                    NodeField nodeField = nodeDefine.Fields[i];
                    FieldDesigner field = EditorUtility.CreateFieldByNodeField(nodeField);
                    if (field == null)
                        continue;
                    startNode.Fields.Add(field);
                }

                behaviorTree.AddNode(startNode);
            }

            //创建空操作节点
            NodeDefine noopDefine = NodeTemplate.FindNode("Noop");
            if (startNode != null && noopDefine != null)
            {
                Rect rect = new Rect(EditorUtility.Center.x + 250, EditorUtility.Center.y, EditorUtility.NodeWidth,
                    EditorUtility.NodeHeight);
                NodeDesigner noopNode = new NodeDesigner(noopDefine.Label, noopDefine.ClassType, rect);
                noopNode.ID = behaviorTree.GenNodeID();
                noopNode.NodeType = noopDefine.NodeType;
                noopNode.Describe = noopDefine.Describe;
                behaviorTree.AddNode(noopNode);

                startNode.AddChildNode(noopNode);
            }

            TreeViewManager.AddBehaviorTree(behaviorTree);
        }

        /// <summary>
        /// 编辑行为树
        /// </summary>
        private void EditBehaviorTree()
        {
            if (SelectedBehaviorTree == null)
                return;

            EditBehaviorTreeForm editBehaviorTreeForm = new EditBehaviorTreeForm(SelectedBehaviorTree);
            editBehaviorTreeForm.ShowDialog();
        }

        public class BehaviorTreeListContent
        {
            private List<BehaviorTreeDesigner> m_DataList = new List<BehaviorTreeDesigner>();

            public List<BehaviorTreeDesigner> DataList
            {
                get { return m_DataList; }
            }
        }

        public class BehaviorTreeGroupListContent
        {
            private List<BehaviorGroupDesigner> m_DataList = new List<BehaviorGroupDesigner>();

            public List<BehaviorGroupDesigner> DataList
            {
                get { return m_DataList; }
            }
        }

        /// <summary>
        /// 复制行为树
        /// </summary>
        private void CopyBehaviorTree()
        {
            if (treeView1.SelectedNode != null)
            {
                if (!(treeView1.SelectedNode.Tag is BehaviorTreeItem))
                {
                    ShowMessage("只能复制行为树");
                    return;
                }

                BehaviorTreeListContent content = new BehaviorTreeListContent();
                content.DataList.Add((treeView1.SelectedNode.Tag as BehaviorTreeItem).BehaviorTree);

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个行为树！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择一个进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择一个进行复制！！！", "警告");
            }
        }

        /// <summary>
        /// 复制分组
        /// </summary>
        private void CopyBehaviorGroup()
        {
            if (treeView1.SelectedNode != null)
            {
                if (!(treeView1.SelectedNode.Tag is GroupItem groupItem))
                {
                    return;
                }

                BehaviorTreeGroupListContent content = new BehaviorTreeGroupListContent();
                content.DataList.Add(groupItem.Group);

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个分组！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择一个分组进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择一个分组进行复制！！！", "警告");
            }
        }

        /// <summary>
        /// 粘贴行为树分组
        /// </summary>
        private void PasteBehaviorTreeGroup()
        {
            try
            {
                BehaviorTreeGroupListContent content = XmlUtility.StringToObject<BehaviorTreeGroupListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    BehaviorGroupDesigner behaviorGroup = content.DataList[i];

                    string groupName = behaviorGroup.GroupName;
                    do
                    {
                        groupName += "_New";
                    }
                    while (BehaviorTreeData.ExistBehaviorGroup(groupName));

                    behaviorGroup.GroupName = groupName;
                    TreeViewManager.AddGroup(behaviorGroup);
                }

                ShowInfo("您粘贴了" + content.DataList.Count + "个分组！！！");
            }
            catch (Exception ex)
            {
                ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        /// <summary>
        /// 粘贴行为树
        /// </summary>
        private void PasteBehaviorTree()
        {
            try
            {
                BehaviorTreeListContent content = XmlUtility.StringToObject<BehaviorTreeListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    BehaviorTreeDesigner behaviorTree = content.DataList[i];
                    string behaviorTreeID = behaviorTree.ID;
                    do
                    {
                        behaviorTreeID += "_New";
                    } while (BehaviorTreeData.ExistBehaviorTree(behaviorTreeID));

                    behaviorTree.ID = behaviorTreeID;
                    TreeViewManager.AddBehaviorTree(behaviorTree);
                }

                ShowInfo("您粘贴了" + content.DataList.Count + "个行为树！！！");
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
        private void RefreshBehaviorTrees()
        {
            if (WorkSpaceData != null)
                this.Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);
        }

        //交换BehaviorTree位置
        private void SwapBehaviorTree(bool up)
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
                BehaviorTreeDesigner preBehaviorTree = BehaviorTreeData.BehaviorTrees[preIdx];
                BehaviorTreeDesigner selectedBehaviorTree = BehaviorTreeData.BehaviorTrees[selectIdx];
                BehaviorTreeData.BehaviorTrees[preIdx] = selectedBehaviorTree;
                BehaviorTreeData.BehaviorTrees[selectIdx] = preBehaviorTree;

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
                BehaviorTreeDesigner nextBehaviorTree = BehaviorTreeData.BehaviorTrees[nextIdx];
                BehaviorTreeDesigner selectedBehaviorTree = BehaviorTreeData.BehaviorTrees[selectIdx];
                BehaviorTreeData.BehaviorTrees[nextIdx] = selectedBehaviorTree;
                BehaviorTreeData.BehaviorTrees[selectIdx] = nextBehaviorTree;

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

        //刷新行为树
        private void UpdateBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                return;

            TreeViewManager.UpdateBehaviorTreeItem(behaviorTree);
        }

        //重置数据
        private void Reset()
        {
            EditorUtility.NodeDefineDic.Clear();
            treeView1.Nodes.Clear();
            NodeTemplate = new NodeTemplate();
            NodeTemplate.ResetEnums();
            NodeTemplate.ResetNodes();
            BehaviorTreeData = new BehaviorTreeDataDesigner();
            CreateTreeViewManager();
            SetSelectedBehaviorTree(null);
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
            BehaviorGroupDesigner group = groupItem.Group;

            if (MessageBox.Show(string.Format("确定删除分组[{0}]吗?", group.GroupName), "提示",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TreeViewManager.DeleteGroup(group);
                ShowInfo("删除分组[{0}]", group.GroupName);
            }
        }

        //更新Group
        private void UpdateGroup(BehaviorGroupDesigner original, BehaviorGroupDesigner group)
        {
            TreeViewManager.UpdateGroup(original, group);
        }

        /// <summary>
        /// 删除行为树
        /// </summary>
        private void DeleteBehaviorTree()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (!(treeView1.SelectedNode.Tag is BehaviorTreeItem))
                return;

            BehaviorTreeDesigner behaviorTree = (treeView1.SelectedNode.Tag as BehaviorTreeItem).BehaviorTree;

            if (MessageBox.Show(string.Format("确定删除行为树{0}吗?", behaviorTree.ID), "提示",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TreeViewManager.RemoveBehaviorTree(behaviorTree);
            }
        }

        /// <summary>
        /// 删除行为树
        /// </summary>
        private void DeleteBehaviorTreeOrGroup()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag is BehaviorTreeItem)
            {
                DeleteBehaviorTree();
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
            NodeDefine nodeDefine = new NodeDefine();
            InputValueDialogForm testForm = new InputValueDialogForm("编辑", nodeDefine);
            testForm.ShowDialog();
        }

        private void 类视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeTemplateForm classForm = new NodeTemplateForm();
            classForm.ShowDialog();
        }

        private void 枚举视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumForm enumForm = new EnumForm();
            enumForm.ShowDialog();
        }

        private void 全局变量视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalVariableForm globalVariableForm = new GlobalVariableForm(BehaviorTreeData.GlobalVariable);
            globalVariableForm.ShowDialog();
        }

        private void context变量视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextVariableForm contextVariableForm = new ContextVariableForm(BehaviorTreeData.ContextVariable);
            contextVariableForm.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.DeleteBehaviorTree);
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.AddBehaviorTree);
        }

        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Exec(OperationType.CopyBehaviorTree);
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.PasteBehaviorTree);
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
            Exec(OperationType.AddBehaviorTree);
        }

        //保存所有数据
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.Save);
        }

        //删除行为树
        private void deleteBehaviorTreeToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.DeleteBehaviorTreeOrGroup);
        }

        //刷新
        private void freshToolStripButton_Click(object sender, EventArgs e)
        {
            Exec(OperationType.Refresh);
        }

        /// <summary>
        /// 添加行为树Item
        /// </summary>
        /// <param name="behaviorTree">行为树</param>
        /// <returns>true:添加成功</returns>
        public bool AddBehaviorTreeItem(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
                return false;
            TreeNode treeNode = treeView1.Nodes.Add(behaviorTree.ID);
            treeNode.Tag = behaviorTree;
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
                case OperationType.EditBehaviorTree:
                    //编辑行为树
                    EditBehaviorTree();
                    break;
                case OperationType.AddBehaviorTree:
                    //添加行为树
                    AddBehaviorTree();
                    break;
                case OperationType.CopyBehaviorTree:
                    //复制行为树
                    CopyBehaviorTree();
                    break;
                case OperationType.CopyGroup:
                    CopyBehaviorGroup();
                    break;
                case OperationType.PasteBehaviorTree:
                    PasteBehaviorTree();
                    break;
                case OperationType.PasteBehaviorTreeGroup:
                    PasteBehaviorTreeGroup();
                    break;
                case OperationType.Refresh:
                    //刷新所有行为树
                    RefreshBehaviorTrees();
                    break;
                case OperationType.DeleteBehaviorTree:
                    //删除选中的行为树
                    DeleteBehaviorTree();
                    break;
                case OperationType.DeleteBehaviorTreeOrGroup:
                    //删除选中的行为树或者分组
                    DeleteBehaviorTreeOrGroup();
                    break;
                case OperationType.SwapBehaviorTree:
                    //交换行为树位置
                    SwapBehaviorTree((bool)args[0]);
                    break;
                case OperationType.UpdateBehaviorTree:
                    UpdateBehaviorTree((BehaviorTreeDesigner)args[0]);
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
                    UpdateGroup((BehaviorGroupDesigner)args[0], (BehaviorGroupDesigner)args[1]);
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
            //读取行为树类信息
            NodeTemplate = XmlUtility.Read<NodeTemplate>(GetNodeTemplatePath());
            if (NodeTemplate == null)
            {
                NodeTemplate = new NodeTemplate();
                NodeTemplate.ResetEnums();
                NodeTemplate.ResetNodes();
                XmlUtility.Save(MainForm.Instance.GetNodeTemplatePath(), MainForm.Instance.NodeTemplate);
            }

            NodeTemplateStringContent = XmlUtility.ObjectToString(NodeTemplate);

            this.Text = Settings.Default.EditorTitle;
            if (string.IsNullOrEmpty(Settings.Default.WorkDirectory) || string.IsNullOrEmpty(Settings.Default.WorkSpaceName))
                return;

            WorkSpaceData = XmlUtility.Read<WorkSpaceData>(GetWorkSpacePath());

            if (WorkSpaceData == null)
                return;

            if (WorkSpaceData != null)
                this.Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);

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
                        NodeTemplate = XmlUtility.Read<NodeTemplate>(GetNodeTemplatePath());
                        if (NodeTemplate == null)
                        {
                            NodeTemplate = new NodeTemplate();
                            NodeTemplate.ResetNodes();
                            XmlUtility.Save(GetNodeTemplatePath(), NodeTemplate);
                        }

                        NodeTemplateStringContent = XmlUtility.ObjectToString(NodeTemplate);

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
            if (WorkSpaceData == null)
                return;

            EditWorkSpaceForm editWorkSpaceForm = new EditWorkSpaceForm();
            editWorkSpaceForm.ShowDialog();
        }

        //保存
        public void Save()
        {
            if (WorkSpaceData == null)
            {
                ShowMessage("当前没有工作空间，请新建或者打开工作空间！");
                return;
            }

            if (NodeTemplate == null || BehaviorTreeData == null)
                return;

            //移除未定义的枚举字段
            NodeTemplate.RemoveUnDefineEnumField();

            //移除未定义的节点
            BehaviorTreeData.RemoveUnDefineNode();

            //修正数据，和模板的保持一致
            BehaviorTreeData.AjustData();

            //检验枚举
            VerifyInfo verifyEnum = NodeTemplate.VerifyEnum();
            if (verifyEnum.HasError)
            {
                ShowMessage(verifyEnum.Msg);
                ShowInfo(verifyEnum.Msg);
                return;
            }

            //检验节点类
            VerifyInfo verifyNodeTemplate = NodeTemplate.VerifyNodeTemplate();
            if (verifyNodeTemplate.HasError)
            {
                ShowMessage(verifyNodeTemplate.Msg);
                ShowInfo(verifyNodeTemplate.Msg);
                return;
            }

            //校验行为树
            VerifyInfo verifyBehaviorTree = BehaviorTreeData.VerifyBehaviorTree();
            if (verifyBehaviorTree.HasError)
            {
                ShowMessage(verifyBehaviorTree.Msg);
                ShowInfo(verifyBehaviorTree.Msg);
                return;
            }

            if (XmlUtility.Save(GetNodeTemplatePath(), NodeTemplate))
            {
                NodeTemplateStringContent = XmlUtility.ObjectToString(NodeTemplate);
            }


            if (XmlUtility.Save(GetBehaviorTreeDataPath(), BehaviorTreeData))
            {
                BehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);
            }

            //序列化成二进制
            BTData.BehaviorTreeData treeData = EditorUtility.CreateTreeData(BehaviorTreeData);
            if (treeData != null)
            {
                string savePath = GetNodeDataSavePath();
                if (File.Exists(savePath))
                    File.Delete(savePath);
                BTData.Serializer.SerializeToFile(treeData, savePath);
            }

            ShowInfo("保存成功 时间:" + DateTime.Now);
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
            return Path.Combine(Settings.Default.WorkDirectory, WorkSpaceData.WorkSpaceName + Settings.Default.BehaviorTreeDataFileSuffix);
        }

        /// <summary>
        /// 获取行为树二进制数据导出路径
        /// </summary>
        /// <returns></returns>
        public string GetNodeDataSavePath()
        {
            if (string.IsNullOrEmpty(Settings.Default.NodeDataSavePath))
                return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
            else
                return Path.Combine(Settings.Default.NodeDataSavePath, Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
        }

        /// <summary>
        /// 获取行为树类信息路径
        /// </summary>
        /// <returns></returns>
        public string GetNodeTemplatePath()
        {
            return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.NodeTemplateFile);
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
            Settings.Default.Save();

            if (e.CloseReason == CloseReason.UserClosing)
            {
                bool nodeDefineDirty = false;
                if (NodeTemplate != null)
                {
                    string tempNodeTemplateStringContent = XmlUtility.ObjectToString(NodeTemplate);
                    if (tempNodeTemplateStringContent != NodeTemplateStringContent)
                    {
                        nodeDefineDirty = true;
                    }
                }

                bool behaviorTreeDirty = false;
                if (BehaviorTreeData != null)
                {
                    string tempBehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);
                    if (tempBehaviorTreeDataStringContent != BehaviorTreeDataStringContent)
                    {
                        behaviorTreeDirty = true;
                    }
                }

                if (nodeDefineDirty || behaviorTreeDirty)
                {
                    DialogResult result = MessageBox.Show(Settings.Default.SaveWarnning, Settings.Default.EditorTitle,
                        MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                        Exec(OperationType.Save);
                }
            }
        }

        public void LoadBehaviorTreeData()
        {
            //读取行为树数据
            BehaviorTreeData = XmlUtility.Read<BehaviorTreeDataDesigner>(GetBehaviorTreeDataPath());
            if (BehaviorTreeData == null)
            {
                BehaviorTreeData = new BehaviorTreeDataDesigner();
                XmlUtility.Save(GetBehaviorTreeDataPath(), BehaviorTreeData);
            }

            BehaviorTreeDataStringContent = XmlUtility.ObjectToString(BehaviorTreeData);

            if (BehaviorTreeData.BehaviorTrees.Count > 0)
            {
                for (int i = 0; i < BehaviorTreeData.BehaviorTrees.Count; i++)
                {
                    BehaviorTreeDesigner behaviorTree = BehaviorTreeData.BehaviorTrees[i];
                    if (behaviorTree != null)
                    {
                        if (behaviorTree.Nodes.Count > 0)
                        {
                            for (int ii = 0; ii < behaviorTree.Nodes.Count; ii++)
                            {
                                NodeDesigner node = behaviorTree.Nodes[ii];
                                if (node.Transitions.Count > 0)
                                {
                                    for (int iii = 0; iii < node.Transitions.Count; iii++)
                                    {
                                        Transition transition = node.Transitions[iii];
                                        NodeDesigner fromNode = behaviorTree.FindNodeByID(transition.FromNodeID);
                                        NodeDesigner toNode = behaviorTree.FindNodeByID(transition.ToNodeID);
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
                RectangleF rect = new RectangleF(e.Bounds.X, e.Bounds.Y, treeView1.Width, e.Bounds.Height);
                e.Graphics.FillRectangle(HighLight, rect);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, rect);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void playToolStripButton_Click(object sender, EventArgs e)
        {
            DebugManager.Instance.Debug(SelectedBehaviorTree);
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            DebugManager.Instance.Stop();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowContent = !Settings.Default.ShowContent;
            toolStripMenuItem7.Checked = Settings.Default.ShowContent;
            Settings.Default.Save();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowDescribe = !Settings.Default.ShowDescribe;
            toolStripMenuItem8.Checked = Settings.Default.ShowDescribe;
            Settings.Default.Save();
        }
    }
}