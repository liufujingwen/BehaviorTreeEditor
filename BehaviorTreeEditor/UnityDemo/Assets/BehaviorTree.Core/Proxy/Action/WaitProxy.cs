namespace R7BehaviorTree
{
    [ActionNode("Wait")]
    public class WaitProxy : BaseNodeProxy
    {
        private int m_WaitTime;
        private float m_Time;

        public override void OnAwake()
        {
            m_WaitTime = NodeData["Millisecond"];
        }

        public override void OnStart()
        {
            m_Time = 0;
        }

        public override void OnUpdate(float deltatime)
        {
            m_Time += deltatime;

            if (m_Time >= m_WaitTime / 1000f)
            {
                Node.Status = ENodeStatus.Succeed;
            }
        }
    }
}
