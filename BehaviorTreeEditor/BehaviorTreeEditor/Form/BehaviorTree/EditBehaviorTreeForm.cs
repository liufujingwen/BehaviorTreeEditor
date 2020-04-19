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
    public partial class EditBehaviorTreeForm : Form
    {
        private BehaviorTreeDesigner m_BehaviorTree;
        private BehaviorTreeDesigner m_EditBehaviorTree;
        private string m_BehaviorTreeContent;

        public EditBehaviorTreeForm(BehaviorTreeDesigner behaviorTree)
        {
            m_BehaviorTree = behaviorTree;
            m_BehaviorTreeContent = XmlUtility.ObjectToString(m_BehaviorTree);
            m_EditBehaviorTree = XmlUtility.StringToObject<BehaviorTreeDesigner>(m_BehaviorTreeContent);
            InitializeComponent();
        }

        private void EditBehaviorTreeForm_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            textBox1.Text = m_EditBehaviorTree.ID;
            textBox2.Text = m_EditBehaviorTree.Describe;
            textBox3.Text = m_EditBehaviorTree.Name;

            listView1.Items.Clear();
            listView2.Items.Clear();

            BindField();
            BindBehaviorTreeVar();
        }

        private ListViewItem GetListViewItem(ListView listView, object obj)
        {
            if (obj == null)
                return null;

            for (int i = 0; i < listView.Items.Count; i++)
            {
                ListViewItem listViewItem = listView.Items[i];
                if (listViewItem.Tag == obj)
                    return listViewItem;
            }

            return null;
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                Exec("Copy_Field");
            else if (tabControl1.SelectedIndex == 1)
                Exec("Copy_BehaviorTreeVar");
            else if (tabControl1.SelectedIndex == 2)
                Exec("Copy_ContextVar");
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                Exec("Paste_Field");
            else if (tabControl1.SelectedIndex == 1)
                Exec("Paste_BehaviorTreeVar");
            else if (tabControl1.SelectedIndex == 2)
                Exec("Paste_ContextVar");
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                Exec("New_Field");
            else if (tabControl1.SelectedIndex == 1)
                Exec("New_BehaviorTreeVar");
            else if (tabControl1.SelectedIndex == 2)
                Exec("New_ContextVar");
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                Exec("Edit_Field");
            else if (tabControl1.SelectedIndex == 1)
                Exec("Edit_BehaviorTreeVar");
            else if (tabControl1.SelectedIndex == 2)
                Exec("Edit_ContextVar");
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                Exec("Delete_Field");
            else if (tabControl1.SelectedIndex == 1)
                Exec("Delete_BehaviorTreeVar");
            else if (tabControl1.SelectedIndex == 2)
                Exec("Delete_ContextVar");
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
                    Exec("Edit_Field");
                }
                else
                {
                    Exec("New_Field");
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
                        Exec("Copy_Field");
                        break;
                    case Keys.V:
                        Exec("Paste_Field");
                        break;
                    case Keys.Up:
                        Exec("Swap_Field", true);
                        break;
                    case Keys.Down:
                        Exec("Swap_Field", false);
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
                        Exec("Edit_Field");
                        break;
                    case Keys.N:
                        Exec("New_Field");
                        break;
                    case Keys.Delete:
                        Exec("Delete_Field");
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
                    Bind();
                    break;
                case "Refresh_Field":
                    BindField();
                    break;
                case "Refresh_BehaviorTreeVar":
                    BindBehaviorTreeVar();
                    break;

                case "Save":
                    break;

                case "Copy_Field":
                    CopyField();
                    break;
                case "Copy_BehaviorTreeVar":
                    CopyBehaviorTreeVar();
                    break;
                case "Copy_ContextVar":
                    CopyBehaviorTreeVar();
                    break;

                case "Paste_Field":
                    PasteField();
                    break;
                case "Paste_BehaviorTreeVar":
                    PasteBehaviorTreeVar();
                    break;

                case "Swap_Field":
                    SwapField((bool)args[0]);
                    break;
                case "Swap_BehaviorTreeVar":
                    SwapBehaviorTreeVar((bool)args[0]);
                    break;

                case "Edit_Field":
                    EditField();
                    break;
                case "Edit_BehaviorTreeVar":
                    EditBehaviorTreeVar();
                    break;

                case "Delete_Field":
                    DeleteField();
                    break;
                case "Delete_BehaviorTreeVar":
                    DeleteBehaviorTreeVar();
                    break;

                case "New_Field":
                    NewField();
                    break;
                case "New_BehaviorTreeVar":
                    NewBehaviorTreeVar();
                    break;
            }

            return true;
        }

        #region 字段

        private void BindField()
        {
            listView1.Items.Clear();
            for (int i = 0; i < m_EditBehaviorTree.Fields.Count; i++)
            {
                FieldDesigner field = m_EditBehaviorTree.Fields[i];
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

        private void NewField()
        {
            FieldDesigner field = new FieldDesigner();
            InputValueDialogForm form = new InputValueDialogForm("添加字段", field);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (m_EditBehaviorTree.AddField(field))
                {
                    Exec("Refresh_Field");
                    ListViewItem listViewItem = GetListViewItem(listView1, field);
                    if (listViewItem != null)
                        listViewItem.Selected = true;
                }
            }
        }

        private void DeleteField()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                m_EditBehaviorTree.Fields.RemoveAt(selectIdx);
                MainForm.Instance.ShowInfo("删除成功");
                Exec("Refresh_Field");
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

        private void EditField()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                FieldDesigner field = listView1.SelectedItems[0].Tag as FieldDesigner;
                InputValueDialogForm dlg = new InputValueDialogForm("编辑字段", listView1.SelectedItems[0].Tag);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Exec("Refresh_Field");
                    ListViewItem listViewItem = GetListViewItem(listView1, field);
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

        private void CopyField()
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

        private void PasteField()
        {
            try
            {
                FieldListContent content = XmlUtility.StringToObject<FieldListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    FieldDesigner field = content.DataList[i];
                    string fieldName = field.FieldName;
                    while (m_EditBehaviorTree.ExistFieldName(fieldName))
                    {
                        fieldName += "_New";
                    }

                    field.FieldName = fieldName;
                    m_EditBehaviorTree.AddField(field);
                }
                Exec("Refresh_Field");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个字段！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        private void SwapField(bool up)
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

                FieldDesigner preField = m_EditBehaviorTree.Fields[preIdx];
                FieldDesigner selectedField = m_EditBehaviorTree.Fields[selectIdx];

                m_EditBehaviorTree.Fields[preIdx] = selectedField;
                m_EditBehaviorTree.Fields[selectIdx] = preField;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView1.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                FieldDesigner preField = m_EditBehaviorTree.Fields[nextIdx];
                FieldDesigner selectedField = m_EditBehaviorTree.Fields[selectIdx];

                m_EditBehaviorTree.Fields[nextIdx] = selectedField;
                m_EditBehaviorTree.Fields[selectIdx] = preField;

                selectIdx = nextIdx;
            }

            Exec("Refresh_Field");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView1.Items[selectIdx].Selected = true;
        }

        #endregion

        #region 行为树变量

        private void BindBehaviorTreeVar()
        {
            listView2.Items.Clear();
            for (int i = 0; i < m_EditBehaviorTree.BehaviorTreeVariableFields.Count; i++)
            {
                VariableFieldDesigner field = m_EditBehaviorTree.BehaviorTreeVariableFields[i];
                ListViewItem listViewItem = listView2.Items.Add(field.VariableFieldName);
                listViewItem.Tag = field;
                listViewItem.SubItems.Add(EditorUtility.GetFieldTypeName(field.VariableFieldType));
                string content = string.Empty;
                if (field.Value != null)
                {
                    content += field.Value;
                }
                listViewItem.SubItems.Add(content);
                listViewItem.SubItems.Add(field.Describe);
            }
        }

        public class VariableFieldListContent
        {
            private List<VariableFieldDesigner> m_DataList = new List<VariableFieldDesigner>();
            public List<VariableFieldDesigner> DataList { get { return m_DataList; } }
        }


        private void listView2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Tag = listView2;
                    contextMenuStrip1.Show(listView2, e.Location);
                }
            }
            else if (e.Clicks == 2)
            {
                if (listView2.SelectedItems.Count == 1)
                {
                    Exec("Edit_BehaviorTreeVar");
                }
                else
                {
                    Exec("New_BehaviorTreeVar");
                }
            }
        }

        private void listView2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Exec("Copy_BehaviorTreeVar");
                        break;
                    case Keys.V:
                        Exec("Paste_BehaviorTreeVar");
                        break;
                    case Keys.Up:
                        Exec("Swap_BehaviorTreeVar", true);
                        break;
                    case Keys.Down:
                        Exec("Swap_BehaviorTreeVar", false);
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
                        Exec("Edit_BehaviorTreeVar");
                        break;
                    case Keys.N:
                        Exec("New_BehaviorTreeVar");
                        break;
                    case Keys.Delete:
                        Exec("Delete_BehaviorTreeVar");
                        break;
                }
            }
        }

        private void NewBehaviorTreeVar()
        {
            VariableFieldDesigner field = new VariableFieldDesigner();
            InputValueDialogForm form = new InputValueDialogForm("添加行为树变量", field);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (m_EditBehaviorTree.AddBehaviorTreeVar(field))
                {
                    Exec("Refresh_BehaviorTreeVar");
                    ListViewItem listViewItem = GetListViewItem(listView2, field);
                    if (listViewItem != null)
                        listViewItem.Selected = true;
                }
            }
        }

        private void EditBehaviorTreeVar()
        {
            if (listView2.SelectedItems.Count == 1)
            {
                VariableFieldDesigner field = listView2.SelectedItems[0].Tag as VariableFieldDesigner;
                InputValueDialogForm dlg = new InputValueDialogForm("编辑行为树变量", listView2.SelectedItems[0].Tag);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Exec("Refresh_BehaviorTreeVar");
                    ListViewItem listViewItem = GetListViewItem(listView2, field);
                    if (listViewItem != null)
                        listViewItem.Selected = true;
                }
            }
        }

        private void CopyBehaviorTreeVar()
        {
            if (listView2.SelectedItems.Count > 0)
            {
                VariableFieldListContent content = new VariableFieldListContent();
                foreach (ListViewItem lvItem in listView2.SelectedItems)
                {
                    if (lvItem.Tag is VariableFieldDesigner)
                        content.DataList.Add((VariableFieldDesigner)lvItem.Tag);
                }

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个变量！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择至少一个进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择至少一个进行复制！！！", "警告");
            }
        }

        private void PasteBehaviorTreeVar()
        {
            try
            {
                VariableFieldListContent content = XmlUtility.StringToObject<VariableFieldListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    VariableFieldDesigner field = content.DataList[i];
                    string fieldName = field.VariableFieldName;
                    while (m_EditBehaviorTree.ExistBehaviorTreeVar(fieldName))
                    {
                        fieldName += "_New";
                    }

                    field.VariableFieldName = fieldName;
                    m_EditBehaviorTree.AddBehaviorTreeVar(field);
                }
                Exec("Refresh_BehaviorTreeVar");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个变量！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        private void SwapBehaviorTreeVar(bool up)
        {
            if (listView2.SelectedIndices.Count > 1)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行交换");
                MainForm.Instance.ShowMessage("请选择一条记录进行交换", "警告");
                return;
            }

            int selectIdx = listView2.SelectedIndices[0];
            if (up)
            {
                //第一个不能往上交换
                if (selectIdx == 0)
                    return;

                int preIdx = selectIdx - 1;

                VariableFieldDesigner preField = m_EditBehaviorTree.BehaviorTreeVariableFields[preIdx];
                VariableFieldDesigner selectedField = m_EditBehaviorTree.BehaviorTreeVariableFields[selectIdx];

                m_EditBehaviorTree.BehaviorTreeVariableFields[preIdx] = selectedField;
                m_EditBehaviorTree.BehaviorTreeVariableFields[selectIdx] = preField;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView2.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                VariableFieldDesigner preField = m_EditBehaviorTree.BehaviorTreeVariableFields[nextIdx];
                VariableFieldDesigner selectedField = m_EditBehaviorTree.BehaviorTreeVariableFields[selectIdx];

                m_EditBehaviorTree.BehaviorTreeVariableFields[nextIdx] = selectedField;
                m_EditBehaviorTree.BehaviorTreeVariableFields[selectIdx] = preField;

                selectIdx = nextIdx;
            }

            Exec("Refresh_BehaviorTreeVar");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView2.Items[selectIdx].Selected = true;
        }

        private void DeleteBehaviorTreeVar()
        {
            if (listView2.SelectedIndices.Count == 1)
            {
                int selectIdx = listView2.SelectedIndices[0];
                m_EditBehaviorTree.Fields.RemoveAt(selectIdx);
                MainForm.Instance.ShowInfo("删除成功");
                Exec("Refresh_BehaviorTreeVar");
                if (listView2.Items.Count > selectIdx)
                    listView2.Items[selectIdx].Selected = true;
            }
            else if (listView2.SelectedIndices.Count == 0)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行删除");
            }
            else
            {
                MainForm.Instance.ShowInfo("无法一次删除多个记录");
            }
        }

        #endregion

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("BehaviorTreeID不能为空");
                MainForm.Instance.ShowInfo("BehaviorTreeID不能为空");
                return;
            }

            string behaviorTreeId = textBox1.Text.Trim();
            string behaviorName = textBox3.Text.Trim();

            if (m_BehaviorTree.ID != behaviorTreeId)
            {
                if (MainForm.Instance.BehaviorTreeData.ExistBehaviorTree(behaviorTreeId))
                {
                    MainForm.Instance.ShowMessage(string.Format("已存在BehaviorTreeID:{0}", behaviorTreeId));
                    MainForm.Instance.ShowInfo(string.Format("已存在BehaviorTreeID:{0}", behaviorTreeId));
                    return;
                }
            }

            m_EditBehaviorTree.ID = behaviorTreeId;
            m_EditBehaviorTree.Describe = textBox2.Text.Trim();
            m_EditBehaviorTree.Name = textBox3.Text.Trim();

            //检验行为树是否合法
            VerifyInfo verifyBehaviorTree = m_EditBehaviorTree.VerifyBehaviorTree();
            if (verifyBehaviorTree.HasError)
            {
                MainForm.Instance.ShowMessage(verifyBehaviorTree.Msg);
                return;
            }

            string editContent = XmlUtility.ObjectToString(m_EditBehaviorTree);
            if (editContent != m_BehaviorTreeContent)
            {
                m_BehaviorTree.UpdateBehaviorTree(m_EditBehaviorTree);
                MainForm.Instance.ShowInfo(string.Format("更新行为树:{0}成功 时间：{1}", m_EditBehaviorTree.ID, DateTime.Now));
            }

            MainForm.Instance.Exec(OperationType.UpdateBehaviorTree, m_BehaviorTree);
            this.Close();
        }


    }
}
