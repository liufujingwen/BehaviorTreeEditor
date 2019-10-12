namespace R7BehaviorTree
{
    /// <summary>
    /// 直到子节点返回成功
    /// 子节点返回失败直接重置继续执行
    /// </summary>
    [DecorateNode("SuccessUntil")]
    public class SuccessUntilProxy : BaseNodeProxy
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

            if (childNodeStatus == ENodeStatus.Succeed)
            {
                m_CompositeNode.Status = ENodeStatus.Succeed;
                return;
            }

            if (childNodeStatus == ENodeStatus.Failed)
            {
                childNode.Reset();
            }
        }
    }
}