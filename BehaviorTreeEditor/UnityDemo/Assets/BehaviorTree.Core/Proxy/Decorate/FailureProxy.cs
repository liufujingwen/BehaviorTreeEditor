namespace R7BehaviorTree
{
    /// <summary>
    /// 将子节点结果以失败返回
    /// </summary>
    [DecorateNode("Failure")]
    public class FailureProxy : BaseNodeProxy
    {
        private CompositeNode m_CompositeNode;
        public override void OnStart()
        {
            m_CompositeNode = Node as CompositeNode;
        }

        public override void OnUpdate(float deltatime)
        {
            BaseNode childNode = m_CompositeNode.Childs[0];
            childNode.Run(deltatime);
            ENodeStatus childNodeStatus = childNode.Status;

            if (childNodeStatus == ENodeStatus.Error)
            {
                m_CompositeNode.Status = ENodeStatus.Error;
                return;
            }

            if (childNodeStatus > ENodeStatus.Running)
                m_CompositeNode.Status = ENodeStatus.Failed;
        }
    }
}