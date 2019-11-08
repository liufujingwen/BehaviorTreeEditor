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
    public partial class GlobalVariableForm : Form
    {
        private VariableDesigner m_EditGlobalVariable;
        private VariableDesigner m_GlobalVariable;
        private string m_OldContent = null;

        public GlobalVariableForm(VariableDesigner globalVariable)
        {
            m_GlobalVariable = globalVariable;
            m_OldContent = XmlUtility.ObjectToString(globalVariable);
            m_EditGlobalVariable = XmlUtility.StringToObject<VariableDesigner>(m_OldContent);
            InitializeComponent();
        }

        private void GlobalVariableForm_Load(object sender, EventArgs e)
        {
            Exec("Refresh");
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
                    BindVariable();
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


        private void BindVariable()
        {
            listView1.Items.Clear();
            for (int i = 0; i < m_EditGlobalVariable.VariableFields.Count; i++)
            {
                VariableFieldDesigner variableField = m_EditGlobalVariable.VariableFields[i];
                ListViewItem listViewItem = listView1.Items.Add((i + 1).ToString());
                listViewItem.Tag = variableField;
                listViewItem.SubItems.Add(variableField.VariableFieldName);
                listViewItem.SubItems.Add(variableField.VariableFieldType.ToString());
                listViewItem.SubItems.Add(variableField.Value == null ? string.Empty : variableField.Value.ToString());
                listViewItem.SubItems.Add(variableField.Describe);
            }
        }

        public class VariableItemListContent
        {
            private List<VariableFieldDesigner> m_DataList = new List<VariableFieldDesigner>();
            public List<VariableFieldDesigner> DataList { get { return m_DataList; } }
        }

        private void Copy()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                VariableItemListContent content = new VariableItemListContent();
                foreach (ListViewItem lvItem in listView1.SelectedItems)
                {
                    if (lvItem.Tag is VariableFieldDesigner)
                        content.DataList.Add((VariableFieldDesigner)lvItem.Tag);
                }

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个变量选项！！！");
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
                VariableItemListContent content = XmlUtility.StringToObject<VariableItemListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    VariableFieldDesigner variableField = content.DataList[i];
                    string variableStr = variableField.VariableFieldName;
                    do
                    {
                        variableStr += "_New";
                    }
                    while (m_EditGlobalVariable.ExistVariableStr(variableStr));

                    variableField.VariableFieldName = variableStr;
                    m_EditGlobalVariable.Add(variableField);
                }

                Exec("Refresh");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个变量选项！！！");
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

                VariableFieldDesigner preVariable = m_EditGlobalVariable.VariableFields[preIdx];
                VariableFieldDesigner selectedVariable = m_EditGlobalVariable.VariableFields[selectIdx];

                m_EditGlobalVariable.VariableFields[preIdx] = selectedVariable;
                m_EditGlobalVariable.VariableFields[selectIdx] = preVariable;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView1.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                VariableFieldDesigner preVariable = m_EditGlobalVariable.VariableFields[nextIdx];
                VariableFieldDesigner selectedVariable = m_EditGlobalVariable.VariableFields[selectIdx];

                m_EditGlobalVariable.VariableFields[nextIdx] = selectedVariable;
                m_EditGlobalVariable.VariableFields[selectIdx] = preVariable;

                selectIdx = nextIdx;
            }

            Exec("Refresh");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView1.Items[selectIdx].Selected = true;
        }

        private void Edit()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[selectIdx];
                VariableFieldDesigner variableItem = selectedItem.Tag as VariableFieldDesigner;

                InputValueDialogForm testForm = new InputValueDialogForm("编辑", variableItem);
                if (testForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateVariableFieldItem(variableItem);
                }
            }
        }

        private void Delete()
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行删除");
                return;
            }

            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[selectIdx];
                VariableFieldDesigner variableItem = selectedItem.Tag as VariableFieldDesigner;

                if (MessageBox.Show(string.Format("确定删除变量选项{0}吗?", variableItem.VariableFieldName), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listView1.Items.RemoveAt(selectIdx);
                    m_EditGlobalVariable.Remove(variableItem.VariableFieldName);
                    MainForm.Instance.ShowInfo(string.Format("删除变量选项{0} 时间:{1}", variableItem.VariableFieldName, DateTime.Now));
                    if (listView1.Items.Count > selectIdx)
                        listView1.Items[selectIdx].Selected = true;
                }
            }
            else
            {
                if (MessageBox.Show("确定批量删除选中的变量选项吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    List<ListViewItem> removeItems = new List<ListViewItem>();

                    for (int i = 0; i < listView1.SelectedIndices.Count; i++)
                    {
                        int index = listView1.SelectedIndices[i];
                        ListViewItem listViewItem = listView1.Items[index];
                        removeItems.Add(listViewItem);
                    }

                    while (removeItems.Count > 0)
                    {
                        ListViewItem listViewItem = removeItems[0];
                        removeItems.RemoveAt(0);
                        VariableFieldDesigner variableField = listViewItem.Tag as VariableFieldDesigner;
                        listView1.Items.Remove(listViewItem);
                        m_EditGlobalVariable.Remove(variableField);
                    }
                }
            }

            //更新序号
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Text = (i + 1).ToString();
            }
        }

        private void New()
        {
            VariableFieldDesigner field = new VariableFieldDesigner();
            InputValueDialogForm form = new InputValueDialogForm("添加变量", field);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (m_EditGlobalVariable.Add(field))
                {
                    AddVariableFieldItem(field);
                }
            }
        }

        public void AddVariableFieldItem(VariableFieldDesigner variableItem)
        {
            if (variableItem == null)
                return;

            ListViewItem listViewItem = listView1.Items.Add((listView1.Items.Count + 1).ToString());
            listViewItem.Tag = variableItem;
            listViewItem.SubItems.Add(variableItem.VariableFieldName);
            listViewItem.SubItems.Add(variableItem.VariableFieldType.ToString());
            listViewItem.SubItems.Add(variableItem.Value == null ? string.Empty : variableItem.Value.ToString());
            listViewItem.SubItems.Add(variableItem.Describe);
            listViewItem.Selected = true;
        }

        public void UpdateVariableFieldItem(VariableFieldDesigner variableItem)
        {
            if (variableItem == null)
                return;

            ListViewItem listViewItem = GetListViewItem(variableItem);

            listViewItem.Tag = variableItem;
            listViewItem.SubItems.Add(variableItem.VariableFieldType.ToString());
            listViewItem.SubItems.Add(variableItem.Value == null ? string.Empty : variableItem.Value.ToString());
            listViewItem.SubItems.Add(variableItem.Describe);

            listViewItem.Selected = true;
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

        private void GlobalVariableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string newContent = XmlUtility.ObjectToString(m_EditGlobalVariable);
            if (newContent != m_OldContent)
            {
                m_GlobalVariable.VariableFields = m_EditGlobalVariable.VariableFields;
            }
        }
    }
}