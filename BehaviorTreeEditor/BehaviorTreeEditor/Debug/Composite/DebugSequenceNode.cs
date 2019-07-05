using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugSequenceNode : DebugNode
    {
        private int m_RunningNodeIndex = 0;

        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[m_RunningNodeIndex];
            runningNode.Update(deltatime);

            switch (runningNode.Status)
            {
                case DebugNodeStatus.Success:
                    m_RunningNodeIndex++;
                    if (m_RunningNodeIndex >= Childs.Count)
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