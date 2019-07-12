using System;
using System.Collections.Generic;

namespace BehaviorTreeViewer
{
    public class AgentDesigner
    {
        private string m_AgentID;
        private string m_GroupName;
        private string m_Describe;
        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();
        private List<NodeDesigner> m_Nodes = new List<NodeDesigner>();
        

        public string AgentID
        {
            get { return m_AgentID; }
            set { m_AgentID = value; }
        }

        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        public List<FieldDesigner> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public List<NodeDesigner> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }

        public int GenNodeID()
        {
            int id = 0;
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (id <= node.ID)
                    id = node.ID;
            }
            return ++id;
        }

        /// <summary>
        /// 通过ID查找节点
        /// </summary>
        /// <param name="ID">节点ID</param>
        /// <returns></returns>
        public NodeDesigner FindNodeByID(int ID)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.ID == ID)
                    return node;
            }
            return null;
        }
    }
}