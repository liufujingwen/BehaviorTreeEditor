using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            AddClassForm addClassForm = new AddClassForm(this);
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

            if (treeView1.SelectedNode.Tag == null)
            {
                contextMenuStrip1.Items[0].Visible = true;
            }
            else
            {
                contextMenuStrip1.Items[1].Visible = true;
                contextMenuStrip1.Items[2].Visible = true;
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
    }
}