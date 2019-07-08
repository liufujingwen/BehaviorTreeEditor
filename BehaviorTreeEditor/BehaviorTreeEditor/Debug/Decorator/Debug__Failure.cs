using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Failure : DebugNode
    {
        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
            }
            else if (runningNode.Status == DebugNodeStatus.Failed)
            {
                Status = DebugNodeStatus.Failed;
            }
            else if (runningNode.Status == DebugNodeStatus.Success)
            {
                Status = DebugNodeStatus.Failed;
            }
        }
    }
}
