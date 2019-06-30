using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class BehaviorTreeData
    {
        private List<AgentDesigner> m_Agents = new List<AgentDesigner>();

        public List<AgentDesigner> Agents
        {
            get { return m_Agents; }
            set { m_Agents = value; }
        }

        /// <summary>
        /// 判断Agent是否存在
        /// </summary>
        /// <param name="agent">agent</param>
        /// <returns>true:存在</returns>
        public bool ExistAgent(AgentDesigner agent)
        {
            if (agent == null)
                throw new Exception("BehaviorTreeData.ExistAgent() error: agent = null");

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner tempAgent = m_Agents[i];
                if (tempAgent == null)
                    continue;
                if (tempAgent == agent)
                    return true;
                if (tempAgent.AgentID == agent.AgentID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断Agent是否存在
        /// </summary>
        /// <param name="agentID">agentID</param>
        /// <returns>true:存在</returns>
        public bool ExistAgent(string agentID)
        {
            if (string.IsNullOrEmpty(agentID))
                throw new Exception("BehaviorTreeData.ExistAgent() error: agentID = null");

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner tempAgent = m_Agents[i];
                if (tempAgent == null)
                    continue;
                if (tempAgent.AgentID == agentID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 添加Agent
        /// </summary>
        /// <param name="agent">agent</param>
        /// <returns>true:添加成功</returns>
        public bool AddAgent(AgentDesigner agent)
        {
            if (ExistAgent(agent))
                return false;

            m_Agents.Add(agent);

            return true;
        }

        /// <summary>
        /// 删除Agent
        /// </summary>
        /// <param name="agent">Agent</param>
        /// <returns>true:删除成功</returns>
        public bool RemoveAgent(AgentDesigner agent)
        {
            if (agent == null)
                return false;

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner temp = m_Agents[i];
                if (temp == null)
                    continue;
                if (temp.AgentID == agent.AgentID)
                {
                    m_Agents.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
