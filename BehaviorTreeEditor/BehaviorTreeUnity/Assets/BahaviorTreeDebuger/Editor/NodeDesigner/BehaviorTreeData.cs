using System;
using System.Collections.Generic;

namespace BehaviorTreeViewer
{
    public class TreeData
    {
        private List<AgentDesigner> m_Agents = new List<AgentDesigner>();

        public List<AgentDesigner> Agents
        {
            get { return m_Agents; }
            set { m_Agents = value; }
        }
    }
}
