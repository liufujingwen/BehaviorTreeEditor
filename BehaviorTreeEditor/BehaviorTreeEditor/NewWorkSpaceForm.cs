using BehaviorTreeEditor.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class NewWorkSpaceForm : Form
    {
        public NewWorkSpaceForm()
        {
            InitializeComponent();
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

            if (string.IsNullOrEmpty(dataSaveDirectoryTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("请选择数据保存位置", "警告");
                return;
            }

            if (!Directory.Exists(dataSaveDirectoryTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("数据保存目录", "警告");
                return;
            }

            string workSpaceDirector = workSpaceDirectoryTB.Text.Trim();
            string[] files = Directory.GetFiles(workSpaceDirector, "*" + Settings.Default.WorkSpaceSetupSuffix);
            if (files.Length > 0)
            {
                string existWorkSpaceName = Path.GetFileName(files[0]);
                //去掉后缀
                existWorkSpaceName = existWorkSpaceName.Replace(Settings.Default.WorkSpaceSetupSuffix, "");
                MainForm.Instance.ShowMessage(string.Format("该位置已被{0}使用,请选择别的目录作为新的工作区位置", existWorkSpaceName), "警告");
                return;
            }

            MainForm.Instance.WorkSpaceData = new WorkSpaceData();
            MainForm.Instance.WorkSpaceData.WorkSpaceName = workSpaceNameTB.Text.Trim();
            MainForm.Instance.WorkSpaceData.Describe = describeTB.Text.Trim();

            Settings.Default.WorkSpaceName = workSpaceNameTB.Text.Trim();
            Settings.Default.WorkDirectory = workSpaceDirectoryTB.Text.Trim();
            Settings.Default.NodeDataSavePath = dataSaveDirectoryTB.Text.Trim();

            Settings.Default.Save();
            XmlUtility.Save<WorkSpaceData>(MainForm.Instance.GetWorkSpacePath(), MainForm.Instance.WorkSpaceData);
            MainForm.Instance.ShowInfo("新建工作区成功,时间：" + DateTime.Now);

            //初始化节点类信息
            MainForm.Instance.NodeClasses = new NodeClasses();
            MainForm.Instance.NodeClassDirty = false;
            XmlUtility.Save(MainForm.Instance.GetNodeClassPath(), MainForm.Instance.NodeClasses);

            //初始化行为树数据
            MainForm.Instance.BehaviorTreeData = new BehaviorTreeData();
            MainForm.Instance.BehaviorTreeDirty = false;
            XmlUtility.Save(MainForm.Instance.GetBehaviorTreeDataPath(), MainForm.Instance.BehaviorTreeData);

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
