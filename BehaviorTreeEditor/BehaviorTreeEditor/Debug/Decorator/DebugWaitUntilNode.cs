using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugWaitUntilNode : DebugNode
    {
        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed)
            {
                Status = DebugNodeStatus.None;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Success)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
