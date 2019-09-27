using BehaviorTreeData;

namespace R7BehaviorTree
{
    public class BehaviorTree
    {
        public string AgentID { get; set; }
        public AgentData AgentData { get; set; }
        public ENodeStatus Status { get; set; } = ENodeStatus.None;

        public void SetAgent(AgentData agent)
        {
            AgentData = agent;
        }

        public void OnUpdate(float deltatime)
        {
        }
    }
}