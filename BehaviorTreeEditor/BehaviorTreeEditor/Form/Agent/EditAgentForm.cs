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
        private AgentDesigner m_EditAgent;
        private string m_AgentContent;

        public EditAgentForm(AgentDesigner agent)
        {
            m_Agent = agent;
            m_AgentContent = XmlUtility.ObjectToString(m_Agent);
            m_EditAgent = XmlUtility.StringToObject<AgentDesigner>(m_AgentContent);
            InitializeComponent();
        }

        private void EditAgentForm_Load(object sender, EventArgs e)
        {
            BindAgent();
        }

        private void BindAgent()
        {
            textBox1.Text = m_EditAgent.AgentID;
            textBox2.Text = m_EditAgent.Describe;

            listView1.Items.Clear();
            for (int i = 0; i < m_EditAgent.Fields.Count; i++)
            {
                FieldDesigner field = m_EditAgent.Fields[i];
                ListViewItem listViewItem = listView1.Items.Add(field.FieldName);
                listViewItem.Tag = field;
                listViewItem.SubItems.Add(EditorUtility.GetFieldTypeName(field.FieldType));
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
                if (m_EditAgent.AddField(field))
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
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                m_EditAgent.Fields.RemoveAt(selectIdx);
                MainForm.Instance.ShowInfo("删除成功");
                Exec("Refresh");
                if (listView1.Items.Count > selectIdx)
                    listView1.Items[selectIdx].Selected = true;
            }
            else if (listView1.SelectedIndices.Count == 0)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行删除");
            }
            else
            {
                MainForm.Instance.ShowInfo("无法一次删除多个记录");
            }
        }

        private void Edit()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                FieldDesigner field = listView1.SelectedItems[0].Tag as FieldDesigner;
                InputValueDialogForm dlg = new InputValueDialogForm("编辑字段", listView1.SelectedItems[0].Tag);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Exec("Refresh");
                    ListViewItem listViewItem = GetListViewItem(field);
                    if (listViewItem != null)
                        listViewItem.Selected = true;
                }
            }
        }

        public class FieldListContent
        {
            private List<FieldDesigner> m_DataList = new List<FieldDesigner>();
            public List<FieldDesigner> DataList { get { return m_DataList; } }
        }

        private void Copy()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                FieldListContent content = new FieldListContent();
                foreach (ListViewItem lvItem in listView1.SelectedItems)
                {
                    if (lvItem.Tag is FieldDesigner)
                        content.DataList.Add((FieldDesigner)lvItem.Tag);
                }

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个字段！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择至少一个进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择至少一个进行复制！！！", "警告");
            }
        }

        private void Paste()
        {
            try
            {
                FieldListContent content = XmlUtility.StringToObject<FieldListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    FieldDesigner field = content.DataList[i];
                    string fieldName = field.FieldName;
                    do
                    {
                        fieldName += "_New";
                    }
                    while (m_EditAgent.ExistFieldName(fieldName));

                    field.FieldName = fieldName;
                    m_EditAgent.AddField(field);
                }
                Exec("Refresh");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个字段！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        private void Swap(bool up)
        {
            if (listView1.SelectedIndices.Count > 1)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行交换");
                MainForm.Instance.ShowMessage("请选择一条记录进行交换", "警告");
                return;
            }

            int selectIdx = listView1.SelectedIndices[0];
            if (up)
            {
                //第一个不能往上交换
                if (selectIdx == 0)
                    return;

                int preIdx = selectIdx - 1;

                FieldDesigner preField = m_EditAgent.Fields[preIdx];
                FieldDesigner selectedField = m_EditAgent.Fields[selectIdx];

                m_EditAgent.Fields[preIdx] = selectedField;
                m_EditAgent.Fields[selectIdx] = preField;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView1.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                FieldDesigner preField = m_EditAgent.Fields[nextIdx];
                FieldDesigner selectedField = m_EditAgent.Fields[selectIdx];

                m_EditAgent.Fields[nextIdx] = selectedField;
                m_EditAgent.Fields[selectIdx] = preField;

                selectIdx = nextIdx;
            }

            Exec("Refresh");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView1.Items[selectIdx].Selected = true;
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("AgentID不能为空");
                MainForm.Instance.ShowInfo("AgentID不能为空");
                return;
            }

            m_EditAgent.AgentID = textBox1.Text.Trim();
            m_EditAgent.Describe = textBox2.Text.Trim();

            //检测空字段名
            if (m_EditAgent.ExistEmptyFieldName())
                return;

            //检测重复字段名
            if (m_EditAgent.ExistSameFieldName())
                return;

            string editContent = XmlUtility.ObjectToString(m_EditAgent);
            if (editContent != m_AgentContent)
            {
                m_Agent.UpdateAgent(m_EditAgent);
                MainForm.Instance.ShowInfo(string.Format("更新Agent:{0}成功 时间：{1}", m_EditAgent.AgentID, DateTime.Now));
            }

            this.Close();
        }
    }
}
