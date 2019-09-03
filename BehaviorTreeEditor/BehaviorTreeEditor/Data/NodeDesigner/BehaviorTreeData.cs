using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class TreeData
    {
        private List<Group> m_Groups = new List<Group>();
        private List<AgentDesigner> m_Agents = new List<AgentDesigner>();

        public List<Group> Groups
        {
            get { return m_Groups; }
            set { m_Groups = value; }
        }

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

        /// <summary>
        /// 检验AgentID
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyAgentID()
        {
            //校验ID是否为空
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (string.IsNullOrEmpty(agent.AgentID))
                {
                    return new VerifyInfo("行为树空的AgentID");
                }
            }

            //检验AgentID是否相同
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent_i = m_Agents[i];
                if (agent_i != null)
                {
                    for (int ii = i + 1; ii < m_Agents.Count; ii++)
                    {
                        AgentDesigner agent_ii = m_Agents[ii];
                        if (agent_i.AgentID == agent_ii.AgentID)
                            return new VerifyInfo(string.Format("行为树存在相同AgentID:{0}", agent_i.AgentID));
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 校验行为树数据
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyBehaviorTree()
        {
            VerifyInfo verifyAgentID = VerifyAgentID();
            if (verifyAgentID.HasError)
                return verifyAgentID;

            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent != null)
                {
                    VerifyInfo verifyAgent = agent.VerifyAgent();
                    if (verifyAgent.HasError)
                        return verifyAgent;
                }
            }
            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 移除未定义的节点
        /// </summary>
        public void RemoveUnDefineNode()
        {
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent != null)
                    agent.RemoveUnDefineNode();
            }
        }


        /// <summary>
        /// 修正数据(和模板保持一致)
        /// </summary>
        public void AjustData()
        {
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent != null)
                    agent.AjustData();
            }
        }

        /// <summary>
        /// 修改节点名字
        /// </summary>
        /// <param name="old">旧的classType</param>
        /// <param name="newType">新的classType</param>
        public void UpdateClassType(string old, string newType)
        {
            for (int i = 0; i < m_Agents.Count; i++)
            {
                AgentDesigner agent = m_Agents[i];
                if (agent != null)
                    agent.UpdateClassType(old, newType);
            }
        }
    }
}
