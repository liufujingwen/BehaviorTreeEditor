using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    /// <summary>
    /// 先打乱所有的子节点,然后从第一个节点开始执行，只要成功一个就算成功
    /// </summary>
    public class Debug__RandomSelector : DebugNode
    {
        private List<DebugNode> Children = new List<DebugNode>();

        public override void OnEnter()
        {
            RunningNodeIndex = 0;
            Children.Clear();
            Children.AddRange(Childs.ToArray());

            Random random = new Random();
            int count = Childs.Count;
            for (int index = 0; index < count; index++)
            {
                int randIndex = random.Next(index, count);
                DebugNode childNode = Children[randIndex];
                Children[randIndex] = Children[index];
                Children[index] = childNode;
            }
        }

        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Children[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed)
            {
                RunningNodeIndex++;
                if (RunningNodeIndex >= Childs.Count)
                {
                    Status = DebugNodeStatus.Failed;
                    return;
                }
            }
            else if (runningNode.Status == DebugNodeStatus.Success)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}