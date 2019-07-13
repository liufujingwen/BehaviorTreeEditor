using System.Collections.Generic;

namespace BehaviorTreeViewer
{
    public class NodeClasses
    {
        private List<NodeClass> m_Nodes = new List<NodeClass>();
        private List<CustomEnum> m_Enums = new List<CustomEnum>();

        public List<CustomEnum> Enums
        {
            get { return m_Enums; }
            set { m_Enums = value; }
        }

        public List<NodeClass> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }
    }
}