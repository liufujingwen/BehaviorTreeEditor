using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugRandomSequence : DebugNode
    {
        private List<DebugNode> Children = new List<DebugNode>();
        private int RunningNodeIndex;

        public override void OnEnter()
        {
            Children.Clear();
            Children.AddRange(Childs.ToArray());
            Random random = new Random();
            
            //打乱子节点
            int count = Children.Count;
            for (int index = 0; index < count; index++)
            {
                int randIndex = random.Next(index, count);
                DebugNode node = Children[randIndex];
                Children[randIndex] = Children[index];
                Children[index] = node;
            }
        }

        public override void OnRunning(float deltatime)
        {
            DebugNode node = Children[RunningNodeIndex];
            if (node.Status == DebugNodeStatus.Success)
            {
                RunningNodeIndex++;
                if (RunningNodeIndex >= Children.Count)
                {
                    Status = DebugNodeStatus.Success;
                }
            }
            else if (node.Status == DebugNodeStatus.Failed)
            {
                Status = DebugNodeStatus.Failed;
            }
            else if (node.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
            }
        }
    }
}
