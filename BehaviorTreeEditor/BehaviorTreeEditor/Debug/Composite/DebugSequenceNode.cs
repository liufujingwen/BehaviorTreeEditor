using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugSequenceNode : DebugNode
    {
        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            switch (runningNode.Status)
            {
                case DebugNodeStatus.Success:
                    RunningNodeIndex++;
                    if (RunningNodeIndex >= Childs.Count)
                        Status = DebugNodeStatus.Success;
                    break;
                case DebugNodeStatus.Failed:
                    Status = DebugNodeStatus.Failed;
                    break;
                case DebugNodeStatus.Error:
                    Status = DebugNodeStatus.Error;
                    break;
            }
        }
    }
}