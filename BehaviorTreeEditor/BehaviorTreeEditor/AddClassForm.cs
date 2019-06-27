using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class AddClassForm : Form
    {
        private NodeClass m_NodeClass = new NodeClass();
        private ClassForm m_ClassForm = null;

        public AddClassForm(ClassForm classForm)
        {
            m_ClassForm = classForm;
            InitializeComponent();
        }

        private void AddClassForm_Load(object sender, EventArgs e)
        {
            nodeTypeCBB.Items.Clear();
            string[] enumNames = Enum.GetNames(typeof(NodeType));
            for (int i = 0; i < enumNames.Length; i++)
            {
                nodeTypeCBB.Items.Add(enumNames[i]);
            }
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(classTypeTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("请填写类名", "提示");
                return;
            }

            m_NodeClass.ClassType = classTypeTB.Text.Trim();
            m_NodeClass.NodeType = (NodeType)nodeTypeCBB.SelectedIndex;
            m_NodeClass.Describe = describeTB.Text.Trim();

            if (!MainForm.Instance.NodeClasses.AddClass(m_NodeClass))
                return;

            m_ClassForm.AddClass(m_NodeClass);

            this.Close();
        }
    }
}