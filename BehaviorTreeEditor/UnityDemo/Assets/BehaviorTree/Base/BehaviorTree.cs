using BehaviorTreeData;

namespace R7BehaviorTree
{
    public class BehaviorTree
    {
        public string AgentID { get; set; }
        public AgentData AgentData { get; set; }
        public ENodeStatus Status { get; set; } = ENodeStatus.None;
        public BaseNode StartNode { get; set; }
        public BaseContext Context { get; private set; }
        public int BehaviorTreeType { get; set; }

        public void SetData(AgentData agentData)
        {
            AgentData = agentData;
            AgentID = agentData.ID;
        }

        public void SetContext(BaseContext context)
        {
            Context = context;
            StartNode?.SetContext(context);
        }

        public void OnUpdate(float deltatime)
        {
        }
    }
}