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
    public partial class EditAgentForm : Form
    {
        private AgentDesigner m_Agent;

        public EditAgentForm(AgentDesigner agent)
        {
            m_Agent = agent;
            InitializeComponent();
        }

        private void EditAgentForm_Load(object sender, EventArgs e)
        {
            BindAgent();
        }

        private void BindAgent()
        {
            textBox1.Text = m_Agent.AgentID;
            textBox2.Text = m_Agent.Describe;

            listView1.Items.Clear();
            for (int i = 0; i < m_Agent.Fields.Count; i++)
            {
                FieldDesigner field = m_Agent.Fields[i];
                ListViewItem listViewItem = listView1.Items.Add(field.FieldName);
                listViewItem.Tag = field;
                listViewItem.SubItems.Add(field.FieldType.ToString());
                string content = string.Empty;
                if (field.Field != null)
                {
                    content += field.Field;
                }
                listViewItem.SubItems.Add(content);
                listViewItem.SubItems.Add(field.Describe);
            }
        }

        private ListViewItem GetListViewItem(object obj)
        {
            if (obj == null)
                return null;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem listViewItem = listView1.Items[i];
                if (listViewItem.Tag == obj)
                    return listViewItem;
            }

            return null;
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Copy");
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Paste");
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("New");
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Edit");
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Delete");
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Tag = listView1;
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
            else if (e.Clicks == 2)
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    Exec("Edit");
                }
                else
                {
                    Exec("New");
                }
            }
        }

        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Exec("Copy");
                        break;
                    case Keys.V:
                        Exec("Paste");
                        break;
                    case Keys.Up:
                        Exec("Swap", true);
                        break;
                    case Keys.Down:
                        Exec("Swap", false);
                        break;
                    case Keys.S:
                        Exec("Save", false);
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
                        Exec("Edit");
                        break;
                    case Keys.N:
                        Exec("New");
                        break;
                    case Keys.Delete:
                        Exec("Delete");
                        break;
                }
            }
        }

        public bool Exec(string cmd, params object[] args)
        {
            if (string.IsNullOrEmpty(cmd))
                return false;

            switch (cmd)
            {
                case "Refresh":
                    BindAgent();
                    break;
                case "Save":
                    break;
                case "Copy":
                    Copy();
                    break;
                case "Paste":
                    Paste();
                    break;
                case "Swap":
                    Swap((bool)args[0]);
                    break;
                case "Edit":
                    Edit();
                    break;
                case "Delete":
                    Delete();
                    break;
                case "New":
                    New();
                    break;
            }

            return true;
        }

        private void New()
        {
            FieldDesigner field = new FieldDesigner();
            InputValueDialogForm form = new InputValueDialogForm("添加字段", field);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (m_Agent.AddField(field))
                {
                    Exec("Refresh");
                    ListViewItem listViewItem = GetListViewItem(field);
                    if (listViewItem != null)
                        listViewItem.Selected = true;
                }
            }
        }

        private void Delete()
        {
        }

        private void Edit()
        {
        }

        private void Copy()
        {
        }

        private void Paste()
        {
        }

        private void Swap(bool up)
        {
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {

        }
    }
}
