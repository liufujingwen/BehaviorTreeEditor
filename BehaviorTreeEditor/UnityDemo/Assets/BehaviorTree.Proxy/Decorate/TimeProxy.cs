using BehaviorTreeData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 时间节点
    /// 在指定的时间内，持续调用其子节点
    /// </summary>
    [DecorateNode("Time")]
    public class TimeProxy : CSharpNodeProxy
    {
        private CompositeNode m_CompositeNode;
        int Duration = -1;
        float CurTime = -1;

        public override void OnAwake()
        {
            IntField durationField = Node.NodeData["Duration"] as IntField;
            if (durationField == null || durationField.Value <= 0)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            Duration = durationField;
        }

        public override void OnStart()
        {
            m_CompositeNode = Node as CompositeNode;
            CurTime = 0;
        }

        public override void OnUpdate(float deltatime)
        {
            CurTime += deltatime;
            BaseNode childNode = m_CompositeNode.Childs[0];
            childNode.Run(deltatime);
            ENodeStatus childNodeStatus = childNode.Status;

            if (childNodeStatus == ENodeStatus.Error)
            {
                m_CompositeNode.Status = ENodeStatus.Error;
                return;
            }

            if (CurTime >= Duration / 1000f)
            {
                m_CompositeNode.Status = ENodeStatus.Succeed;
                return;
            }

            if (childNodeStatus == ENodeStatus.Failed || childNodeStatus == ENodeStatus.Succeed)
            {
                childNode.Reset();
            }
        }
    }
}