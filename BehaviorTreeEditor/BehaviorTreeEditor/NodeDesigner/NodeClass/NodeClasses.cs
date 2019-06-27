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
        /// 获取指定类型所有组合节点
        /// </summary>
        /// <param name="nodeType">节点类型</param>
        /// <returns></returns>
        public List<NodeClass> GetNodes(NodeType nodeType)
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
    }
}
