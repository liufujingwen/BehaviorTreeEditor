using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__RateSelector : DebugNode
    {
        private List<int> PriorityList = null;
        private List<DebugNode> Children = new List<DebugNode>();
        private Dictionary<DebugNode, int> PriorityIndex = new Dictionary<DebugNode, int>();
        private List<DebugNode> RandList = new List<DebugNode>();

        public override void OnEnter()
        {
            PriorityIndex.Clear();
            Children.Clear();
            Children.AddRange(Childs.ToArray());
            RandList.Clear();
            PriorityList = new List<int>();
            RepeatIntFieldDesigner repeatIntField = Node["Priority"].Field as RepeatIntFieldDesigner;
            List<int> tempList = repeatIntField.Value;

            if (Childs.Count == 1)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            if (tempList.Count != Childs.Count)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            PriorityList.AddRange(tempList.ToArray());

            RateSortChildren();
        }

        /// <summary>
        /// 概率
        /// </summary>
        public void RateSortChildren()
        {
            if (Children == null || Children.Count <= 0)
                return;

            //先计算权重总和
            int prioritySum = 0;
            for (int index = 0; index < PriorityList.Count; index++)
            {
                prioritySum += PriorityList[index];
            }

            RandList.Clear();

            Random random = new Random();
            //遍历所有权重
            for (int index = 0; index < PriorityList.Count; index++)
            {
                //从 0 到最大权重随出一个随机数
                int randIndex = random.Next(0, prioritySum);
                //随机数 + 节点权重值 = 本次权重值
                int priority = randIndex + PriorityList[index];
                int pos = 0;

                //插入排序
                if (RandList.Count == 0)
                {
                    //插入第一个节点
                    RandList.Add(Children[index]);
                    //记录该节点权重
                    PriorityIndex.Add(Children[index], priority);
                }
                else
                {
                    for (int i = 0; i < RandList.Count; i++)
                    {
                        //最大的一端开始向下遍历，插入到第一个小于自己权重节点的位置
                        pos = i;
                        if (priority > PriorityIndex[RandList[i]])
                            break;
                        pos++;
                    }
                    //插入节点
                    RandList.Insert(pos, Children[index]);
                    //记录
                    PriorityIndex.Add(Children[index], priority);
                }
            }
        }

        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = RandList[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Success)
            {
                Status = DebugNodeStatus.Success;
            }
            else if (runningNode.Status == DebugNodeStatus.Failed)
            {
                Status = DebugNodeStatus.Failed;
                RunningNodeIndex++;
                //所有运行失败将返回失败
                if (RunningNodeIndex >= Children.Count)
                    Status = DebugNodeStatus.Failed;
            }
            else if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
            }
        }
    }
}