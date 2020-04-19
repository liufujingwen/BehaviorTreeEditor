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
    public partial class EditGroupForm : Form
    {
        private string m_OldContent;
        BehaviorGroupDesigner m_Group;
        BehaviorGroupDesigner m_EditGroup;

        public EditGroupForm(BehaviorGroupDesigner group)
        {
            m_Group = group;
            m_OldContent = XmlUtility.ObjectToString(m_Group);
            m_EditGroup = XmlUtility.StringToObject<BehaviorGroupDesigner>(m_OldContent);

            InitializeComponent();
        }

        private void EditGroup_Load(object sender, EventArgs e)
        {
            textBox1.Text = m_EditGroup.GroupName;
            textBox2.Text = m_EditGroup.Describe;
        }

        private void editGroupBTN_Click(object sender, EventArgs e)
        {
            string groupName = textBox1.Text.Trim();
            string describe = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(groupName))
            {
                MainForm.Instance.ShowMessage("组名不能为空");
                return;
            }

            if (m_Group.GroupName != m_EditGroup.GroupName && MainForm.Instance.TreeViewManager.ExistGroup(groupName))
            {
                MainForm.Instance.ShowMessage(string.Format("{0}，已存在", groupName));
                return;
            }

            string oldName = m_Group.GroupName;
            m_EditGroup.GroupName = groupName;
            m_EditGroup.Describe = describe;

            string content = XmlUtility.ObjectToString(m_EditGroup);
            if (m_OldContent != content)
            {
                m_Group.GroupName = m_EditGroup.GroupName;
                MainForm.Instance.Exec(OperationType.UpdateGroup, m_Group, m_EditGroup);
                MainForm.Instance.ShowInfo(string.Format("分组{0}改为{1} 时间:{2}", oldName, m_Group.GroupName, DateTime.Now));
            }

            this.Close();
        }
    }
}