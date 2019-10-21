using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class NodeTemplateForm : Form
    {
        private NodeTemplate m_Nodes;
        private NodeTreeViewManager m_NodeTreeViewManager;

        public NodeTemplateForm()
        {
            m_Nodes = MainForm.Instance.NodeTemplate;
            InitializeComponent();
        }

        private void 添加类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NodeType nodeType = NodeType.Composite;

            if (treeView1.SelectedNode != null)
            {
                TreeNode treeNode = treeView1.SelectedNode;
                while (treeNode.Parent != null)
                    treeNode = treeNode.Parent;

                if (treeNode.Tag is NodeTypeItem)
                {
                    NodeTypeItem nodeTypeItem = treeNode.Tag as NodeTypeItem;
                    nodeType = nodeTypeItem.NodeType;
                }
            }

            AddDefineForm addDefineForm = new AddDefineForm(nodeType, this);
            addDefineForm.ShowDialog();
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            m_NodeTreeViewManager = new NodeTreeViewManager(treeView1, m_Nodes);
            m_NodeTreeViewManager.BindNodeTemplate();
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible = false;
            }

            if (treeView1.SelectedNode.Tag is NodeTypeItem || treeView1.SelectedNode.Tag is CategoryItem)
            {
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[3].Visible = true;
            }
            else if (treeView1.SelectedNode.Tag is NodeItem)
            {
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = true;
            }

            if (e.Clicks == 2)
            {
                if (treeView1.SelectedNode == null)
                    return;

                if (treeView1.SelectedNode.Tag == null)
                    return;

                if (!(treeView1.SelectedNode.Tag is NodeItem))
                    return;

                NodeItem nodeItem = treeView1.SelectedNode.Tag as NodeItem;
                NodeDefine nodeDefine = nodeItem.NodeDefine;


                EditNodeDefineForm editClassForm = new EditNodeDefineForm(this, nodeDefine, delegate ()
                {
                    nodeItem = m_NodeTreeViewManager.BindNodeDefine(nodeDefine);
                    treeView1.SelectedNode = nodeItem.TreeNode;
                });
                editClassForm.ShowDialog();

            }
        }

        public void AddNodeDefine(NodeDefine nodeDefine)
        {
            m_NodeTreeViewManager.BindNodeDefine(nodeDefine);
            MainForm.Instance.ShowInfo("成功添加:" + nodeDefine.ClassType + ",时间：" + DateTime.Now);
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否重置所有类信息吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                MainForm.Instance.Exec(OperationType.Reset);
                m_Nodes = MainForm.Instance.NodeTemplate;
                m_NodeTreeViewManager = new NodeTreeViewManager(treeView1, m_Nodes);
                m_NodeTreeViewManager.BindNodeTemplate();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag == null)
                return;

            if (!(treeView1.SelectedNode.Tag is NodeItem))
                return;

            NodeItem nodeItem = treeView1.SelectedNode.Tag as NodeItem;
            NodeDefine nodeDefine = nodeItem.NodeDefine;

            if (MessageBox.Show(string.Format("是否删除节点{0}？", nodeDefine.ClassType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_Nodes.Remove(nodeDefine);
                m_NodeTreeViewManager.RemoveNodeDefine(nodeDefine);
            }
        }

        private void treeView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        MainForm.Instance.Exec(OperationType.Save);
                        break;
                    case Keys.C:
                        CopyClass();
                        break;
                    case Keys.V:
                        PasteClass();
                        break;
                }
            }
        }

        private void 编辑类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag == null)
                return;

            if (!(treeView1.SelectedNode.Tag is NodeItem))
                return;

            NodeItem nodeItem = treeView1.SelectedNode.Tag as NodeItem;
            NodeDefine nodeDefine = nodeItem.NodeDefine;

            EditNodeDefineForm editClassForm = new EditNodeDefineForm(this, nodeDefine, delegate ()
             {
                 nodeItem = m_NodeTreeViewManager.BindNodeDefine(nodeDefine);
                 treeView1.SelectedNode = nodeItem.TreeNode;
             });
            editClassForm.ShowDialog();
        }

        public class NodeDefineListContent
        {
            private List<NodeDefine> m_DataList = new List<NodeDefine>();
            public List<NodeDefine> DataList { get { return m_DataList; } }
        }

        private void CopyClass()
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag == null)
                return;

            if (!(treeView1.SelectedNode.Tag is NodeItem))
                return;

            NodeItem nodeItem = treeView1.SelectedNode.Tag as NodeItem;
            NodeDefine nodeDefine = nodeItem.NodeDefine;

            NodeDefineListContent content = new NodeDefineListContent();
            content.DataList.Add(nodeDefine);

            if (content.DataList.Count > 0)
                Clipboard.SetText(XmlUtility.ObjectToString(content));

            MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个节点类！！！");
        }

        private void PasteClass()
        {
            try
            {
                NodeDefineListContent content = XmlUtility.StringToObject<NodeDefineListContent>(Clipboard.GetText());

                NodeDefine nodeDefine = null;
                for (int i = 0; i < content.DataList.Count; i++)
                {
                    nodeDefine = content.DataList[i];
                    string classType = nodeDefine.ClassType;
                    do
                    {
                        classType += "_New";
                    }
                    while (m_Nodes.ExistClassType(classType));

                    nodeDefine.ClassType = classType;
                    m_Nodes.AddClass(nodeDefine);
                }

                m_NodeTreeViewManager.BindNodeDefine(nodeDefine);

                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个节点类！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }
    }
}