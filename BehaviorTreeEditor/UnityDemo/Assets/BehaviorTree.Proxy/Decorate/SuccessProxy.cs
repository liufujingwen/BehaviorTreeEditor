namespace R7BehaviorTree
{
    /// <summary>
    /// 把失败包装成功返回
    /// </summary>
    [DecorateNode("Success")]
    public class SuccessProxy : CSharpNodeProxy
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

            if (childNodeStatus == ENodeStatus.Failed)
                m_CompositeNode.Status = ENodeStatus.Succeed;
        }
    }
}
