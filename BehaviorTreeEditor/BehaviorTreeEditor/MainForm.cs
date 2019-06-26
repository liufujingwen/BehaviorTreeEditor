using BehaviorTreeEditor.Properties;
using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ContentUserControl m_ContentUserControl;

        public MainForm()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Exec(OperationType.LoadWorkSpace);
            m_ContentUserControl = new ContentUserControl();
            m_ContentUserControl.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(m_ContentUserControl);
        }

        private void MainForm_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("KeyUp：");
        }

        private void 新建工作区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.NewWorkSpace);
        }

        private void 打开工作区ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec(OperationType.OpenWorkSpace);
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
            if (string.IsNullOrEmpty(Settings.Default.WorkDirectory) || string.IsNullOrEmpty(Settings.Default.WorkSpaceName))
                return;
            WorkSpaceData = XmlUtility.Read<WorkSpaceData>(GetWorkSpacePath());
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
                    }
                }
            }
        }

        //保存
        public void Save()
        {

        }

        /// <summary>
        /// 获取数据保存路径
        /// </summary>
        /// <returns></returns>
        public string GetDataSavePath()
        {
            return Path.Combine(Settings.Default.WorkDirectory, Settings.Default.WorkSpaceName + Settings.Default.NodeDataFileSuffix);
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
        public void ShowMessage(string msg, string title)
        {
            MessageBox.Show(msg, title);
        }
    }
}
