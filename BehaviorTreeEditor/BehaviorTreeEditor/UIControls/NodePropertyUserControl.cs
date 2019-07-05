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

        public NodePropertyUserControl()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        private void NodePropertyUserControl_Load(object sender, EventArgs e)
        {
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

            BindFields();
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
