namespace R7BehaviorTree
{
    /// <summary>
    /// 取反节点
    /// </summary>
    [DecorateNode("Not")]
    public class NotProxy : CSharpNodeProxy
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
                m_CompositeNode.Status = ENodeStatus.Failed;
                return;
            }

            if (childNodeStatus == ENodeStatus.Failed)
            {
                m_CompositeNode.Status = ENodeStatus.Succeed;
                return;
            }
        }
    }
}
