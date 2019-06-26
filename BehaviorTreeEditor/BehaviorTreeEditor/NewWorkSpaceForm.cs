using BehaviorTreeEditor.Properties;
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

            MainForm.Instance.WorkSpaceData = new WorkSpaceData();
            MainForm.Instance.WorkSpaceData.WorkSpaceName = workSpaceNameTB.Text.Trim();
            MainForm.Instance.WorkSpaceData.Describe = describeTB.Text.Trim();

            Settings.Default.WorkSpaceName = workSpaceNameTB.Text.Trim();
            Settings.Default.WorkDirectory = workSpaceDirectoryTB.Text.Trim();
            Settings.Default.NodeDataSavePath = MainForm.Instance.GetDataSavePath();

            Settings.Default.Save();
            XmlUtility.Save<WorkSpaceData>(MainForm.Instance.GetWorkSpacePath(), MainForm.Instance.WorkSpaceData);

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
