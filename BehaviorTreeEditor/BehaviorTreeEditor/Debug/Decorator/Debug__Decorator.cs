using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Decorator : DebugNode
    {
        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (Status != DebugNodeStatus.Transition)
                Status = runningNode.Status;
        }
    }
}