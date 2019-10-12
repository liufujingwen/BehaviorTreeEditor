using System;
using System.Collections.Generic;

namespace R7BehaviorTree
{
    /// <summary>
    /// 先打乱所有的子节点,然后从第一个节点开始执行，只要成功一个就算成功
    /// </summary>
    [CompositeNode("RandomSelector")]
    public class RandomSelectorProxy : BaseNodeProxy
    {
        private List<BaseNode> m_Children = new List<BaseNode>();
        private CompositeNode m_CompositeNode;
        private Random m_Random = new Random();

        public override void OnStart()
        {
            m_CompositeNode = Node as CompositeNode;

            m_CompositeNode.RunningNodeIndex = 0;
            m_Children.Clear();
            for (int i = 0; i < m_CompositeNode.Childs.Count; i++)
            {
                m_Children.Add(m_CompositeNode.Childs[i]);
            }

            int count = m_CompositeNode.Childs.Count;
            for (int index = 0; index < count; index++)
            {
                int randIndex = m_Random.Next(index, count);
                BaseNode childNode = m_Children[randIndex];
                m_Children[randIndex] = m_Children[index];
                m_Children[index] = childNode;
            }
        }

        public override void OnUpdate(float deltatime)
        {
            for (int i = m_CompositeNode.RunningNodeIndex; i < m_CompositeNode.Childs.Count;)
            {
                BaseNode childNode = m_Children[i];
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
                    m_CompositeNode.RunningNodeIndex++;
                    i++;
                    if (m_CompositeNode.RunningNodeIndex >= m_Children.Count)
                    {
                        m_CompositeNode.Status = ENodeStatus.Failed;
                        return;
                    }
                }
            }
        }
    }
}