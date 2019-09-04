using BehaviorTreeEditor.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class EditWorkSpaceForm : Form
    {
        private string m_OldWorkSpaceDirectory;
        private string m_OldWorkSpaceName;
        private string m_OldDataSaveDirectory;

        public EditWorkSpaceForm()
        {
            InitializeComponent();
        }

        private void EditWorkSpaceForm_Load(object sender, EventArgs e)
        {
            m_OldWorkSpaceDirectory = Settings.Default.WorkDirectory;
            m_OldWorkSpaceName = MainForm.Instance.WorkSpaceData.WorkSpaceName;
            m_OldDataSaveDirectory = string.IsNullOrEmpty(Settings.Default.NodeDataSavePath) ? Settings.Default.WorkDirectory : Settings.Default.NodeDataSavePath;

            workSpaceNameTB.Text = MainForm.Instance.WorkSpaceData.WorkSpaceName;
            workSpaceDirectoryTB.Text = Settings.Default.WorkDirectory;
            describeTB.Text = MainForm.Instance.WorkSpaceData.Describe;
            dataSaveDirectoryTB.Text = Settings.Default.NodeDataSavePath;
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(workSpaceNameTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("请输入工作区名字", "警告");
                return;
            }

            if (string.IsNullOrEmpty(workSpaceDirectoryTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("请选择工作区位置", "警告");
                return;
            }

            if (!Directory.Exists(workSpaceDirectoryTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("工作区目录不存在", "警告");
                return;
            }

            //if (string.IsNullOrEmpty(dataSaveDirectoryTB.Text.Trim()))
            //{
            //    MainForm.Instance.ShowMessage("请选择数据保存位置", "警告");
            //    return;
            //}

            if (!string.IsNullOrEmpty(dataSaveDirectoryTB.Text.Trim()) && !Directory.Exists(dataSaveDirectoryTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("数据保存目录不存在", "警告");
                return;
            }

            if (MainForm.Instance.WorkSpaceData != null)
            {
                MainForm.Instance.WorkSpaceData.WorkSpaceName = workSpaceNameTB.Text.Trim();
                MainForm.Instance.WorkSpaceData.Describe = describeTB.Text.Trim();

                Settings.Default.WorkSpaceName = workSpaceNameTB.Text.Trim();
                Settings.Default.WorkDirectory = workSpaceDirectoryTB.Text.Trim();
                Settings.Default.NodeDataSavePath = dataSaveDirectoryTB.Text.Trim();

                if (m_OldWorkSpaceDirectory != Settings.Default.WorkDirectory || m_OldWorkSpaceName != Settings.Default.WorkSpaceName)
                {
                    //移动工作空间文件
                    string oldWorkSpaceFile = Path.Combine(m_OldWorkSpaceDirectory, m_OldWorkSpaceName + Settings.Default.WorkSpaceSetupSuffix);
                    string newWorkSpaceFile = Path.Combine(Settings.Default.WorkDirectory, Settings.Default.WorkSpaceName + Settings.Default.WorkSpaceSetupSuffix);
                    if (File.Exists(oldWorkSpaceFile))
                        File.Move(oldWorkSpaceFile, newWorkSpaceFile);

                    //移动节点xml文件
                    string oldXmlDataFile = Path.Combine(m_OldWorkSpaceDirectory, m_OldWorkSpaceName + Settings.Default.BehaviorTreeDataFileSuffix);
                    string newXmlDataFile = MainForm.Instance.GetBehaviorTreeDataPath();
                    if (File.Exists(oldXmlDataFile))
                        File.Move(oldXmlDataFile, newXmlDataFile);

                    //移动NodeClass文件
                    string oldNodeClassXmlFile = Path.Combine(m_OldWorkSpaceDirectory, Settings.Default.NodeClassFile);
                    string newNodeClassXmlFile = Path.Combine(Settings.Default.WorkDirectory, Settings.Default.NodeClassFile);
                    if (File.Exists(oldNodeClassXmlFile))
                        File.Move(oldNodeClassXmlFile, newNodeClassXmlFile);
                }

                //移动旧二进制数据
                if (Settings.Default.NodeDataSavePath != m_OldDataSaveDirectory)
                {
                    string oldTreeDataFile = Path.Combine(m_OldDataSaveDirectory, m_OldWorkSpaceName + Settings.Default.NodeDataFileSuffix);
                    string newFile = MainForm.Instance.GetNodeDataSavePath();
                    if (File.Exists(oldTreeDataFile))
                        File.Move(oldTreeDataFile, newFile);
                }

                Settings.Default.Save();
                XmlUtility.Save<WorkSpaceData>(MainForm.Instance.GetWorkSpacePath(), MainForm.Instance.WorkSpaceData);
                MainForm.Instance.Exec(OperationType.Refresh);
                MainForm.Instance.ShowInfo("编辑工作区,时间：" + DateTime.Now);
            }

            this.Close();
        }

        private void selectWorkSpaceDirectoryBTN_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                workSpaceDirectoryTB.Text = folderBrowserDialog1.SelectedPath.Trim();
            }
        }

        private void selectDataSaveDirectoryBTN_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                dataSaveDirectoryTB.Text = folderBrowserDialog2.SelectedPath.Trim();
            }
        }
    }
}
