using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugRandomNode : DebugNode
    {
        private int RandomIndex = 0;

        public override void OnEnter()
        {
            Random random = new Random();
            RandomIndex = random.Next(0, Childs.Count);
        }

        public override void OnRunning(float deltatime)
        {
            DebugNode childNode = Childs[RandomIndex];
            childNode.Update(deltatime);

            if (childNode.Status == DebugNodeStatus.Success)
                Status = DebugNodeStatus.Success;
            else if(childNode.Status == DebugNodeStatus.Failed)
                Status = DebugNodeStatus.Success;
            else if (childNode.Status == DebugNodeStatus.Error)
                Status = DebugNodeStatus.Error;
        }
    }
}
