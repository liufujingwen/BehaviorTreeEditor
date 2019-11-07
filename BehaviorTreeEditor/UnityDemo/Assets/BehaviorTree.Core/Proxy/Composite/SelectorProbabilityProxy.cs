using System;
using System.Collections.Generic;
using BTData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 概率选择
    /// 概率选择节点是根据概率“直接”选择并执行某个子节点，无论其返回成功还是失败，
    /// 概率选择节点也将返回同样的结果。如果该子节点返回失败，概率选择也返回失败，它不会像选择节点那样会继续执行接下来的子节点。
    /// </summary>
    [CompositeNode("SelectorProbability")]
    public class SelectorProbabilityProxy : BaseNodeProxy
    {
        private List<BaseNode> Children = new List<BaseNode>();
        private Dictionary<BaseNode, int> PriorityIndex = new Dictionary<BaseNode, int>();
        private List<BaseNode> m_RandomList = new List<BaseNode>();
        private List<int> m_PriorityList = null;
        private CompositeNode m_CompositeNode;
        private Random m_Random = new Random();
        private BaseNode m_SelectorNode;

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

            m_SelectorNode = m_RandomList[0];
        }

        public override void OnUpdate(float deltatime)
        {
            m_SelectorNode.Run(deltatime);
            ENodeStatus childNodeStatus = m_SelectorNode.Status;

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
                m_CompositeNode.Status = ENodeStatus.Failed;
            }
        }
    }
}