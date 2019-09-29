using System;
using System.Collections.Generic;
using BehaviorTreeData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 按概率排序节点，依次执行排序后的节点，只要一个成功就为成功，全部失败为失败
    /// </summary>
    [CompositeNode("RateSelector")]
    public class RateSelectorProxy : CSharpNodeProxy
    {
        private List<BaseNode> Children = new List<BaseNode>();
        private Dictionary<BaseNode, int> PriorityIndex = new Dictionary<BaseNode, int>();
        private List<BaseNode> m_RandomList = new List<BaseNode>();
        private List<int> m_PriorityList = null;
        private CompositeNode m_CompositeNode;
        private Random m_Random = new Random();

        public override void OnAwake()
        {
            m_PriorityList = Node.NodeData["Priority"];
        }

        public override void OnStart()
        {
            if (m_PriorityList == null || m_PriorityList.Count == 0)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            m_CompositeNode = Node as CompositeNode;

            Children.Clear();
            for (int i = 0; i < m_CompositeNode.Childs.Count; i++)
                Children.Add(m_CompositeNode.Childs[i]);

            //先计算权重总和
            int prioritySum = 0;
            for (int index = 0; index < m_PriorityList.Count; index++)
            {
                prioritySum += m_PriorityList[index];
            }

            m_RandomList.Clear();

            //遍历所有权重
            for (int index = 0; index < m_PriorityList.Count; index++)
            {
                //从 0 到最大权重随出一个随机数
                int randIndex = m_Random.Next(0, prioritySum);
                //随机数 + 节点权重值 = 本次权重值
                int priority = randIndex + m_PriorityList[index];
                int pos = 0;

                BaseNode childNode = Children[index];

                //记录
                PriorityIndex.Add(childNode, priority);

                //插入排序
                if (m_RandomList.Count == 0)
                {
                    //插入第一个节点
                    m_RandomList.Add(childNode);
                }
                else
                {
                    for (int i = 0; i < m_RandomList.Count; i++)
                    {
                        //最大的一端开始向下遍历，插入到第一个小于自己权重节点的位置
                        pos = i;
                        if (priority > PriorityIndex[m_RandomList[i]])
                            break;
                        pos++;
                    }
                    //插入节点
                    m_RandomList.Insert(pos, childNode);
                }
            }
        }

        public override void OnUpdate(float deltatime)
        {
            for (int i = m_CompositeNode.RunningNodeIndex; i < m_CompositeNode.Childs.Count;)
            {
                BaseNode childNode = m_RandomList[i];
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

                if (childNode.Status == ENodeStatus.Failed)
                {
                    m_CompositeNode.RunningNodeIndex++;
                    i++;
                    //所有运行失败将返回失败
                    if (m_CompositeNode.RunningNodeIndex >= Children.Count)
                        m_CompositeNode.Status = ENodeStatus.Failed;
                }
            }
        }
    }
}