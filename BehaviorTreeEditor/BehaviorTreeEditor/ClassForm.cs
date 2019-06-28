using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class ClassForm : Form
    {
        private NodeClasses m_Nodes;
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
            if (treeView1.SelectedNode == m_CompositeNode)
                nodeType = NodeType.Composite;
            else if (treeView1.SelectedNode == m_DecoratorNode)
                nodeType = NodeType.Decorator;
            else if (treeView1.SelectedNode == m_ConditionNode)
                nodeType = NodeType.Condition;
            else if (treeView1.SelectedNode == m_ActionNode)
                nodeType = NodeType.Action;

            AddClassForm addClassForm = new AddClassForm(nodeType, this);
            addClassForm.ShowDialog();
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            BindNodeTree();
        }

        private void BindNodeTree()
        {
            treeView1.Nodes.Clear();
            m_CompositeNode = treeView1.Nodes.Add("组合节点");
            m_DecoratorNode = treeView1.Nodes.Add("修饰节点");
            m_ConditionNode = treeView1.Nodes.Add("条件节点");
            m_ActionNode = treeView1.Nodes.Add("动作节点");

            //绑定组合节点
            List<NodeClass> compositeList = m_Nodes.GetClasses(NodeType.Composite);
            m_CompositeNode.Nodes.Clear();
            for (int i = 0; i < compositeList.Count; i++)
            {
                NodeClass node = compositeList[i];
                TreeNode treeNode = m_CompositeNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定修饰节点
            List<NodeClass> decoratorList = m_Nodes.GetClasses(NodeType.Decorator);
            m_DecoratorNode.Nodes.Clear();
            for (int i = 0; i < decoratorList.Count; i++)
            {
                NodeClass node = decoratorList[i];
                TreeNode treeNode = m_DecoratorNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定条件节点
            List<NodeClass> conditionList = m_Nodes.GetClasses(NodeType.Condition);
            m_ConditionNode.Nodes.Clear();
            for (int i = 0; i < conditionList.Count; i++)
            {
                NodeClass node = conditionList[i];
                TreeNode treeNode = m_ConditionNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定动作节点
            List<NodeClass> actionList = m_Nodes.GetClasses(NodeType.Action);
            m_ActionNode.Nodes.Clear();
            for (int i = 0; i < actionList.Count; i++)
            {
                NodeClass node = actionList[i];
                TreeNode treeNode = m_ActionNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            treeView1.ExpandAll();
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
            {
                contextMenuStrip1.Items[i].Visible = false;
            }

            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
            {
                contextMenuStrip1.Items[0].Visible = true;
                contextMenuStrip1.Items[3].Visible = true;
            }
            else
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

                NodeClass nodeClass = treeView1.SelectedNode.Tag as NodeClass;

                EditClassForm editClassForm = new EditClassForm(this, nodeClass);
                editClassForm.ShowDialog();
            }
        }

        public void AddClass(NodeClass nodeClass)
        {
            if (nodeClass == null)
                return;

            TreeNode treeNode = null;

            if (nodeClass.NodeType == NodeType.Composite)
                treeNode = m_CompositeNode;
            else if (nodeClass.NodeType == NodeType.Decorator)
                treeNode = m_DecoratorNode;
            else if (nodeClass.NodeType == NodeType.Condition)
                treeNode = m_ConditionNode;
            else if (nodeClass.NodeType == NodeType.Action)
                treeNode = m_ActionNode;

            if (treeNode == null)
                return;

            TreeNode newNode = treeNode.Nodes.Add(nodeClass.ClassType);
            newNode.Tag = nodeClass;

            MainForm.Instance.ShowInfo("成功添加:" + nodeClass.ClassType + ",时间：" + DateTime.Now);
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否重置所有类信息吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_Nodes = new NodeClasses();
                m_Nodes.ResetNodes();
                MainForm.Instance.NodeClasses = m_Nodes;
                MainForm.Instance.NodeClassDirty = true;
                BindNodeTree();
            }
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag == null)
                return;

            TreeNode selectedNode = treeView1.SelectedNode;
            NodeClass nodeClass = selectedNode.Tag as NodeClass;
            if (MessageBox.Show(string.Format("是否删除节点{0}？", nodeClass.ClassType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_Nodes.Remove(nodeClass);
                treeView1.Nodes.Remove(selectedNode);
                MainForm.Instance.NodeClassDirty = true;
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
                }
            }
        }

        private void 编辑类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag == null)
                return;

            NodeClass nodeClass = treeView1.SelectedNode.Tag as NodeClass;

            EditClassForm editClassForm = new EditClassForm(this, nodeClass);
            editClassForm.ShowDialog();
        }
    }
}