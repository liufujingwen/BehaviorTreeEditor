using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class AddGroup : Form
    {
        public AddGroup()
        {
            InitializeComponent();
        }

        private void addGroupBTN_Click(object sender, EventArgs e)
        {
            string groupName = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(groupName))
            {
                MainForm.Instance.ShowMessage("组名不能为空");
                return;
            }

            if (MainForm.Instance.TreeViewManager.ExistGroup(groupName))
            {
                MainForm.Instance.ShowMessage(string.Format("该组{0}，已存在", groupName));
                return;
            }

            BehaviorGroup group = new BehaviorGroup();
            group.GroupName = groupName;

            MainForm.Instance.TreeViewManager.AddGroup(group);
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
