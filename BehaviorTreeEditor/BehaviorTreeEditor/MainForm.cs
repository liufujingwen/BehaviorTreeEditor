using BehaviorTreeEditor.Properties;
using BehaviorTreeEditor.UIControls;
using System;
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

        public WorkSpaceData WorkSpaceData;
        public NodeClasses NodeClasses;
        public bool NodeClassDirty;
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
        }

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
            this.Close();
        }

        private void 类视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClassForm classForm = new ClassForm();
            classForm.ShowDialog();
        }

        public bool Exec(OperationType opration)
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
                        this.Text = string.Format("{0}[{1}]", Settings.Default.EditorTitle, WorkSpaceData.WorkSpaceName);
                        MainForm.Instance.ShowInfo("打开工作区,时间：" + DateTime.Now);

                        //读取行为树类信息
                        NodeClasses = XmlUtility.Read<NodeClasses>(GetNodeClassPath());
                        if (NodeClasses == null)
                        {
                            NodeClasses = new NodeClasses();
                            NodeClasses.ResetNodes();
                            XmlUtility.Save(MainForm.Instance.GetNodeClassPath(), MainForm.Instance.NodeClasses);
                        }
                        NodeClassDirty = false;
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
            if (NodeClassDirty)
            {
                if (NodeClasses != null)
                {
                    if (XmlUtility.Save<NodeClasses>(GetNodeClassPath(), NodeClasses))
                    {
                        NodeClassDirty = false;
                    }
                }
            }

            ShowInfo("保存成功 时间:" + DateTime.Now);
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
                if (NodeClassDirty)
                {
                    DialogResult result = MessageBox.Show(Settings.Default.SaveWarnning, Settings.Default.EditorTitle, MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                        Exec(OperationType.Save);
                }
            }

            Settings.Default.Save();
            NodeClassDirty = false;
        }

        private void 枚举视图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnumForm enumForm = new EnumForm();
            enumForm.ShowDialog();
        }
    }
}
