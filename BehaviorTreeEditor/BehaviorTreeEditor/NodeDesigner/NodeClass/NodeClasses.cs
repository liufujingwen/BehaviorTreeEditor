using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class NodeClasses
    {
        private List<NodeClass> m_Nodes = new List<NodeClass>();

        public List<NodeClass> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }

        /// <summary>
        /// 获取指定类型所有组合节点类
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <returns></returns>
        public List<NodeClass> GetClasses(NodeType nodeType)
        {
            List<NodeClass> nodeList = new List<NodeClass>();
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeClass nodeClass = m_Nodes[i];
                if (nodeClass == null)
                    continue;
                if (nodeClass.NodeType == nodeType)
                    nodeList.Add(nodeClass);
            }
            return nodeList;
        }

        /// <summary>
        /// 添加节点类
        /// </summary>
        /// <param name="nodeClass"></param>
        public bool AddClass(NodeClass nodeClass)
        {
            if (nodeClass == null)
                return false;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeClass tmp = m_Nodes[i];
                if (tmp.ClassType == nodeClass.ClassType)
                {
                    MainForm.Instance.ShowMessage(string.Format("已存在{0},请换一个类名", nodeClass.ClassType), "警告");
                    return false;
                }
            }

            m_Nodes.Add(nodeClass);

            m_Nodes.Sort(delegate (NodeClass a, NodeClass b)
            {
                return a.NodeType.CompareTo(b.NodeType);
            });

            return true;
        }

        /// <summary>
        /// 删除节点类
        /// </summary>
        /// <param name="nodeClass">节点类对象</param>
        /// <returns></returns>
        public bool Remove(NodeClass nodeClass)
        {
            if (nodeClass == null)
                return false;
            return m_Nodes.Remove(nodeClass);
        }

        public void ResetNodes()
        {
            m_Nodes.Clear();
            #region 组合节点
            //并行节点
            NodeClass parallelNode = new NodeClass();
            parallelNode.ClassType = "Parallel";
            parallelNode.NodeType = NodeType.Composite;
            parallelNode.Describe = "Parallel节点在一般意义上是并行的执行其子节点，即“一边做A，一边做B”";
            m_Nodes.Add(parallelNode);

            //顺序节点
            NodeClass sequenceNode = new NodeClass();
            sequenceNode.ClassType = "Sequence";
            sequenceNode.NodeType = NodeType.Composite;
            sequenceNode.Describe = "Sequence节点以给定的顺序依次执行其子节点，直到所有子节点成功返回，该节点也返回成功。只要其中某个子节点失败，那么该节点也失败。";
            m_Nodes.Add(sequenceNode);

            #endregion

            #region 装饰节点

            //空操作节点
            NodeClass notNode = new NodeClass();
            notNode.ClassType = "Not";
            notNode.NodeType = NodeType.Decorator;
            notNode.Describe = "非节点将子节点的返回值取反";
            m_Nodes.Add(notNode);

            #endregion

            #region 条件节点

            //空操作节点
            NodeClass compareNode = new NodeClass();
            compareNode.ClassType = "Compare";
            compareNode.NodeType = NodeType.Condition;
            compareNode.Describe = "Compare节点对左右参数进行比较";
            m_Nodes.Add(compareNode);

            #endregion

            #region 动作节点

            //空操作节点
            NodeClass noopNode = new NodeClass();
            noopNode.ClassType = "Noop";
            noopNode.NodeType = NodeType.Action;
            noopNode.Describe = "空操作（Noop）节点只是作为占位，仅执行一次就返回成功";
            m_Nodes.Add(noopNode);

            #endregion
        }
    }
}
