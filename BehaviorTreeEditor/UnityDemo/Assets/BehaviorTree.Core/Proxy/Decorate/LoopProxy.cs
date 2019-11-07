using BTData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 循环节点
    /// 循环执行子节点指定的次数
    /// 次数：如果次数配置为-1，则视为无限循环，总是返回运行。否则循环执行子节点指定的次数然后返回成功，在指定次数之前则返回运行。
    /// </summary>
    [DecorateNode("Loop")]
    public class LoopProxy : BaseNodeProxy
    {
        private CompositeNode m_CompositeNode;
        private int m_LoopTimes = 0;
        private int CurTimes = 0;

        public override void OnAwake()
        {
            IntField loopTimesField = Node.NodeData["LoopTimes"] as IntField;
            if (loopTimesField == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            m_LoopTimes = loopTimesField;
        }

        public override void OnStart()
        {
            m_CompositeNode = Node as CompositeNode;
            CurTimes = 0;
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

            if (childNodeStatus == ENodeStatus.Failed || childNodeStatus == ENodeStatus.Succeed)
            {
                CurTimes++;

                if (m_LoopTimes != -1 && CurTimes >= m_LoopTimes)
                {
                    m_CompositeNode.Status = ENodeStatus.Succeed;
                    return;
                }

                //重置子节点状态
                childNode.Reset();
            }
        }
    }
}