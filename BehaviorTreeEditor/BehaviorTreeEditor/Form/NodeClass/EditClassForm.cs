using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class EditClassForm : Form
    {
        private NodeClass m_NodeClass;
        private NodeClass m_EditNodeClass;
        private ClassForm m_ClassForm = null;
        private string m_Content;

        public EditClassForm(ClassForm classForm, NodeClass nodeClass)
        {
            m_ClassForm = classForm;
            m_NodeClass = nodeClass;
            m_Content = XmlUtility.ObjectToString(m_NodeClass);
            m_EditNodeClass = XmlUtility.StringToObject<NodeClass>(m_Content);
            InitializeComponent();
        }

        private void EditClassForm_Load(object sender, EventArgs e)
        {
            classTypeTB.Text = m_EditNodeClass.ClassType;

            nodeTypeCBB.Items.Clear();
            string[] enumNames = Enum.GetNames(typeof(NodeType));
            for (int i = 2; i < enumNames.Length; i++)
            {
                nodeTypeCBB.Items.Add(enumNames[i]);
            }
            nodeTypeCBB.SelectedIndex = (int)m_EditNodeClass.NodeType - 2;

            describeTB.Text = m_EditNodeClass.Describe;

            ShowFieldDataInPage(m_EditNodeClass);
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(classTypeTB.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("请填写类名", "提示");
                return;
            }

            m_EditNodeClass.ClassType = classTypeTB.Text.Trim();
            m_EditNodeClass.NodeType = (NodeType)(nodeTypeCBB.SelectedIndex + 2);
            m_EditNodeClass.Describe = describeTB.Text.Trim();

            //校验节点类是否合法
            VerifyInfo verifyNodeClass = m_NodeClass.VerifyNodeClass();
            if (verifyNodeClass.HasError)
            {
                MainForm.Instance.ShowMessage(verifyNodeClass.Msg);
                return;
            }

            string content = XmlUtility.ObjectToString(m_EditNodeClass);

            if (m_Content != content)
            {
                m_NodeClass.UpdateNodeClass(m_EditNodeClass);
                MainForm.Instance.ShowInfo("修改成功 时间:" + DateTime.Now);
            }

            this.Close();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("CopyField");
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("PasteField");
        }

        private void 选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("EditField");
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("NewField");
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("DeleteField");
        }

        private void listViewFields_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Exec("CopyField");
                        break;
                    case Keys.V:
                        Exec("PasteField");
                        break;
                    case Keys.Up:
                        Exec("SwapField", true);
                        break;
                    case Keys.Down:
                        Exec("SwapField", false);
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
                        Exec("EditField");
                        break;
                    case Keys.N:
                        Exec("NewField");
                        break;
                    case Keys.Delete:
                        Exec("DeleteField");
                        break;
                }
            }
        }

        private void listViewFields_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Tag = listViewFields;
                    contextMenuStrip1.Show(listViewFields, e.Location);
                }
            }
            else if (e.Clicks == 2)
            {
                if (listViewFields.SelectedItems.Count == 1)
                {
                    InputValueDialogForm dlg = new InputValueDialogForm("编辑字段", listViewFields.SelectedItems[0].Tag);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Exec("Refresh");
                    }
                }
                else
                {
                    Exec("NewField");
                }
                Exec("Refresh");
            }
        }

        private bool Exec(string cmd, params object[] agrs)
        {
            if (string.IsNullOrEmpty(cmd))
                return false;

            switch (cmd)
            {
                case "Refresh":
                    ShowFieldDataInPage(m_EditNodeClass);
                    break;
                case "NewField":
                    NewField();
                    break;
                case "EditField":
                    EditField();
                    break;
                case "DeleteField":
                    DeleteField();
                    break;
                case "SwapField":
                    SwapField((bool)agrs[0]);
                    break;
                case "CopyField":
                    CopyField();
                    break;
                case "PasteField":
                    PasteField();
                    break;
            }

            return true;
        }

        private void NewField()
        {
            NodeField field = new NodeField();
            InputValueDialogForm form = new InputValueDialogForm("添加字段", field);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (m_EditNodeClass.AddField(field))
                {
                    Exec("Refresh");
                }
            }
        }

        //刷新ListView
        private void ShowFieldDataInPage(NodeClass m_NodeClass)
        {
            if (m_NodeClass == null)
                return;

            listViewFields.Items.Clear();
            foreach (NodeField field in m_NodeClass.Fields)
            {
                if (field.DefaultValue == null)
                    continue;
                ListViewItem listViewItem = listViewFields.Items.Add(field.FieldName);
                listViewItem.Tag = field;
                listViewItem.SubItems.Add(EditorUtility.GetFieldTypeName(field.FieldType));
                listViewItem.SubItems.Add(field.DefaultValue != null ? field.DefaultValue.ToString() : string.Empty);
                listViewItem.SubItems.Add(field.Describe);
            }
        }

        //编辑字段
        private void EditField()
        {
            CollectionEditor.EditValue(this, m_EditNodeClass, "Fields");
        }

        public class FieldListContent
        {
            private List<NodeField> m_DataList = new List<NodeField>();
            public List<NodeField> DataList { get { return m_DataList; } }
        }

        //复制字段
        private void CopyField()
        {
            if (listViewFields.SelectedItems.Count > 0)
            {
                FieldListContent content = new FieldListContent();
                foreach (ListViewItem lvItem in listViewFields.SelectedItems)
                {
                    if (lvItem.Tag is NodeField)
                        content.DataList.Add((NodeField)lvItem.Tag);
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

        //粘贴字段
        private void PasteField()
        {
            try
            {
                FieldListContent content = (FieldListContent)XmlUtility.StringToObject<FieldListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    NodeField field = content.DataList[i];
                    string fieldName = field.FieldName;
                    do
                    {
                        fieldName += "_New";
                    }
                    while (m_EditNodeClass.ExistFieldName(fieldName));

                    field.FieldName = fieldName;
                    m_EditNodeClass.AddField(field);
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

        //删除字段
        private void DeleteField()
        {
            if (listViewFields.SelectedIndices.Count == 1)
            {
                int selectIdx = listViewFields.SelectedIndices[0];
                m_EditNodeClass.Fields.RemoveAt(selectIdx);
                MainForm.Instance.ShowInfo("删除成功");
                Exec("Refresh");
                if (listViewFields.Items.Count > selectIdx)
                    listViewFields.Items[selectIdx].Selected = true;
            }
            else if (listViewFields.SelectedIndices.Count == 0)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行删除");
            }
            else
            {
                MainForm.Instance.ShowInfo("无法一次删除多个记录");
            }
        }

        //交换
        private void SwapField(bool up)
        {
            if (listViewFields.SelectedIndices.Count > 1)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行交换");
                MainForm.Instance.ShowMessage("请选择一条记录进行交换", "警告");
                return;
            }

            int selectIdx = listViewFields.SelectedIndices[0];
            if (up)
            {
                //第一个不能往上交换
                if (selectIdx == 0)
                    return;

                int preIdx = selectIdx - 1;

                NodeField preField = m_EditNodeClass.Fields[preIdx];
                NodeField selectedField = m_EditNodeClass.Fields[selectIdx];

                m_EditNodeClass.Fields[preIdx] = selectedField;
                m_EditNodeClass.Fields[selectIdx] = preField;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listViewFields.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                NodeField preField = m_EditNodeClass.Fields[nextIdx];
                NodeField selectedField = m_EditNodeClass.Fields[selectIdx];

                m_EditNodeClass.Fields[nextIdx] = selectedField;
                m_EditNodeClass.Fields[selectIdx] = preField;

                selectIdx = nextIdx;
            }

            Exec("Refresh");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listViewFields.Items[selectIdx].Selected = true;
        }
    }
}
