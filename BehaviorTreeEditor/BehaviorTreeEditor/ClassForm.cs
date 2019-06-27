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
            InitializeComponent();
        }

        private void 添加类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClassForm addClassForm = new AddClassForm(this);
            addClassForm.ShowDialog();
        }

        private void ClassForm_Load(object sender, EventArgs e)
        {
            ResetNodes();
            BindNodeTree();
        }

        private void ResetNodes()
        {
            MainForm.Instance.NodeClasses = new NodeClasses();
            m_Nodes = MainForm.Instance.NodeClasses;

            #region 组合节点
            //并行节点
            NodeClass parallelNode = new NodeClass();
            parallelNode.ClassType = "Parallel";
            parallelNode.NodeType = NodeType.Composite;
            parallelNode.Describe = "Parallel节点在一般意义上是并行的执行其子节点，即“一边做A，一边做B”";
            m_Nodes.Nodes.Add(parallelNode);

            //顺序节点
            NodeClass sequenceNode = new NodeClass();
            sequenceNode.ClassType = "Sequence";
            sequenceNode.NodeType = NodeType.Composite;
            sequenceNode.Describe = "Sequence节点以给定的顺序依次执行其子节点，直到所有子节点成功返回，该节点也返回成功。只要其中某个子节点失败，那么该节点也失败。";
            m_Nodes.Nodes.Add(sequenceNode);

            #endregion

            #region 装饰节点

            //空操作节点
            NodeClass notNode = new NodeClass();
            notNode.ClassType = "Not";
            notNode.NodeType = NodeType.Decorator;
            notNode.Describe = "非节点将子节点的返回值取反";
            m_Nodes.Nodes.Add(notNode);

            #endregion

            #region 条件节点

            //空操作节点
            NodeClass compareNode = new NodeClass();
            compareNode.ClassType = "Compare";
            compareNode.NodeType = NodeType.Condition;
            compareNode.Describe = "Compare节点对左右参数进行比较";
            m_Nodes.Nodes.Add(compareNode);

            #endregion

            #region 动作节点

            //空操作节点
            NodeClass noopNode = new NodeClass();
            noopNode.ClassType = "Noop";
            noopNode.NodeType = NodeType.Action;
            noopNode.Describe = "空操作（Noop）节点只是作为占位，仅执行一次就返回成功";
            m_Nodes.Nodes.Add(noopNode);

            #endregion
        }

        private void BindNodeTree()
        {
            treeView1.Nodes.Clear();
            m_CompositeNode = treeView1.Nodes.Add("组合节点");
            m_DecoratorNode = treeView1.Nodes.Add("修饰节点");
            m_ConditionNode = treeView1.Nodes.Add("条件节点");
            m_ActionNode = treeView1.Nodes.Add("动作节点");

            //绑定组合节点
            List<NodeClass> compositeList = m_Nodes.GetNodes(NodeType.Composite);
            m_CompositeNode.Nodes.Clear();
            for (int i = 0; i < compositeList.Count; i++)
            {
                NodeClass node = compositeList[i];
                TreeNode treeNode = m_CompositeNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定修饰节点
            List<NodeClass> decoratorList = m_Nodes.GetNodes(NodeType.Decorator);
            m_DecoratorNode.Nodes.Clear();
            for (int i = 0; i < decoratorList.Count; i++)
            {
                NodeClass node = decoratorList[i];
                TreeNode treeNode = m_DecoratorNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定条件节点
            List<NodeClass> conditionList = m_Nodes.GetNodes(NodeType.Condition);
            m_ConditionNode.Nodes.Clear();
            for (int i = 0; i < conditionList.Count; i++)
            {
                NodeClass node = conditionList[i];
                TreeNode treeNode = m_ConditionNode.Nodes.Add(node.ClassType);
                treeNode.Tag = node;
            }

            //绑定动作节点
            List<NodeClass> actionList = m_Nodes.GetNodes(NodeType.Action);
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