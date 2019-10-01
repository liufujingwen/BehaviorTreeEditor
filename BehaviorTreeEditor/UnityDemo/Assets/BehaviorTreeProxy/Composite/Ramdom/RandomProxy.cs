using System;

namespace R7BehaviorTree
{
    /// <summary>
    /// 随机选择一个节点，并将随机节点的结果返回
    /// </summary>
    [CompositeNode("Random")]
    public class RandomProxy : CSharpNodeProxy
    {
        private CompositeNode m_CompositeNode;
        private Random m_Random = new Random();

        public override void OnStart()
        {
            m_CompositeNode = Node as CompositeNode;
            m_CompositeNode.RunningNodeIndex = m_Random.Next(0, m_CompositeNode.Childs.Count);
        }

        public override void OnUpdate(float deltatime)
        {
            BaseNode childNode = m_CompositeNode.Childs[m_CompositeNode.RunningNodeIndex];
            childNode.Run(deltatime);
            ENodeStatus childNodeStatus = childNode.Status;

            if (childNodeStatus == ENodeStatus.Error)
            {
                m_CompositeNode.Status = ENodeStatus.Error;
                return;
            }

            if (childNodeStatus > ENodeStatus.Running)
                m_CompositeNode.Status = childNodeStatus;
        }
    }
}