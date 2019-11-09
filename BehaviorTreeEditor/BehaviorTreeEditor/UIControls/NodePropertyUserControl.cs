using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class NodePropertyUserControl : UserControl
    {
        public static NodePropertyUserControl ms_Instance;

        public static NodePropertyUserControl Instance
        {
            get { return ms_Instance; }
        }

        private NodeDesigner m_Node;
        private CompareUserControl m_CompareUserControl;
        private SetVariableUserControl m_SetVariableUserControl;

        public NodePropertyUserControl()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void NodePropertyUserControl_Load(object sender, EventArgs e)
        {
            m_CompareUserControl = new CompareUserControl();
            panel3.Controls.Add(m_CompareUserControl);
            m_CompareUserControl.Visible = false;

            m_SetVariableUserControl = new SetVariableUserControl();
            panel3.Controls.Add(m_SetVariableUserControl);
            m_SetVariableUserControl.Visible = false;

            ImageList il = new ImageList();
            //设置高度
            il.ImageSize = new Size(1, 20);
            //绑定listView控件
            listView1.SmallImageList = il;

            BindNode();
        }

        public void SetSelectedNode(NodeDesigner node)
        {
            m_Node = node;
            BindNode();
        }

        private void BindNode()
        {
            if (m_Node == null)
            {
                classTypeLab.Text = string.Empty;
                nodeIdLab.Text = string.Empty;
                nodeTypeLab.Text = string.Empty;
                describeTB.Text = string.Empty;
                listView1.Items.Clear();
            }
            else
            {
                classTypeLab.Text = m_Node.ClassType;
                nodeIdLab.Text = m_Node.ID.ToString();
                nodeTypeLab.Text = m_Node.NodeType.ToString();
                describeTB.Text = m_Node.Describe;
            }

            if (m_Node != null)
            {
                //比较节点特殊处理
                if (m_Node.ClassType == "Compare")
                {
                    listView1.Visible = false;
                    m_SetVariableUserControl.Visible = false;
                    m_CompareUserControl.Visible = true;
                    m_CompareUserControl.SetNode(m_Node);
                }
                //设置变量节点
                else if (m_Node.ClassType == "SetVariable")
                {
                    listView1.Visible = false;
                    m_SetVariableUserControl.Visible = true;
                    m_CompareUserControl.Visible = false;
                    m_SetVariableUserControl.SetNode(m_Node);
                }
                else
                {
                    listView1.Visible = true;
                    m_SetVariableUserControl.Visible = false;
                    m_CompareUserControl.Visible = false;

                    BindFields();
                }
            }
        }

        private void BindFields()
        {
            listView1.Items.Clear();

            if (m_Node == null)
                return;

            for (int i = 0; i < m_Node.Fields.Count; i++)
            {
                FieldDesigner field = m_Node.Fields[i];
                ListViewItem listViewItem = listView1.Items.Add(field.Title);
                listViewItem.Tag = field;
                listViewItem.SubItems.Add(EditorUtility.GetFieldTypeName(field.FieldType));
                string content = field.Field != null ? field.Field.ToString() : string.Empty;
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
                if (listViewItem == null)
                    continue;
                if (listViewItem.Tag == obj)
                    return listViewItem;
            }

            return null;
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    MainForm.Instance.ShowInfo("双击字段进行编辑");
                }
                else if (listView1.SelectedItems.Count == 1)
                {
                    if (DebugManager.Instance.Debugging)
                    {
                        MainForm.Instance.ShowInfo("调试模式不能编辑");
                        return;
                    }

                    EditField();
                }

            }
        }

        private void EditField()
        {
            if (listView1.SelectedItems.Count != 1)
                return;

            ListViewItem listViewItem = listView1.SelectedItems[0];
            FieldDesigner field = listViewItem.Tag as FieldDesigner;

            if (field == null)
                return;

            if (field.Field == null)
                return;

            InputValueDialogForm dlg = new InputValueDialogForm("编辑", field.Field);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                BindFields();
                ListViewItem selectedItem = GetListViewItem(field);
                if (selectedItem != null)
                    selectedItem.Selected = true;
            }
        }

        private void panel2_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        MainForm.Instance.Exec(OperationType.Save);
                        break;
                }
            }
        }

        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        MainForm.Instance.Exec(OperationType.Save);
                        break;
                }
            }
        }
    }
}
