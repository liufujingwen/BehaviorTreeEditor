using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class ClassForm : Form
    {
        private NodeClasses m_Nodes;
        private NodeTreeViewManager m_NodeTreeViewManager;
        private TreeNode m_CompositeNode;
        private TreeNode m_DecoratorNode;
        private TreeNode m_ConditionNode;
        private TreeNode m_ActionNode;

        public ClassForm()
        {
            m_Nodes = MainForm.Instance.NodeClasses;
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

            AddClassForm addClassForm = new AddClassForm(nodeType, this);
            addClassForm.ShowDialog();
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            m_NodeTreeViewManager = new NodeTreeViewManager(treeView1, m_Nodes);
            m_NodeTreeViewManager.BindNodeClasses();
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
                NodeClass nodeClass = nodeItem.NodeClass;


                EditClassForm editClassForm = new EditClassForm(this, nodeClass, delegate ()
                {
                    nodeItem = m_NodeTreeViewManager.BindNodeClass(nodeClass);
                    treeView1.SelectedNode = nodeItem.TreeNode;
                });
                editClassForm.ShowDialog();

            }
        }

        public void AddClass(NodeClass nodeClass)
        {
            m_NodeTreeViewManager.BindNodeClass(nodeClass);
            MainForm.Instance.ShowInfo("成功添加:" + nodeClass.ClassType + ",时间：" + DateTime.Now);
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否重置所有类信息吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_NodeTreeViewManager = new NodeTreeViewManager(treeView1, m_Nodes);
                m_NodeTreeViewManager.BindNodeClasses();
                MainForm.Instance.Exec(OperationType.Reset);
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
            NodeClass nodeClass = nodeItem.NodeClass;

            if (MessageBox.Show(string.Format("是否删除节点{0}？", nodeClass.ClassType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_Nodes.Remove(nodeClass);
                m_NodeTreeViewManager.RemoveNodeClass(nodeClass);
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
            NodeClass nodeClass = nodeItem.NodeClass;

            EditClassForm editClassForm = new EditClassForm(this, nodeClass, delegate ()
             {
                 nodeItem = m_NodeTreeViewManager.BindNodeClass(nodeClass);
                 treeView1.SelectedNode = nodeItem.TreeNode;
             });
            editClassForm.ShowDialog();
        }

        public class NodeClassListContent
        {
            private List<NodeClass> m_DataList = new List<NodeClass>();
            public List<NodeClass> DataList { get { return m_DataList; } }
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
            NodeClass nodeClass = nodeItem.NodeClass;

            NodeClassListContent content = new NodeClassListContent();
            content.DataList.Add(nodeClass);

            if (content.DataList.Count > 0)
                Clipboard.SetText(XmlUtility.ObjectToString(content));

            MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个节点类！！！");
        }

        private void PasteClass()
        {
            try
            {
                NodeClassListContent content = XmlUtility.StringToObject<NodeClassListContent>(Clipboard.GetText());

                NodeClass nodeClass = null;
                for (int i = 0; i < content.DataList.Count; i++)
                {
                    nodeClass = content.DataList[i];
                    string classType = nodeClass.ClassType;
                    do
                    {
                        classType += "_New";
                    }
                    while (m_Nodes.ExistClassType(classType));

                    nodeClass.ClassType = classType;
                    m_Nodes.AddClass(nodeClass);
                }

                m_NodeTreeViewManager.BindNodeClass(nodeClass);

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