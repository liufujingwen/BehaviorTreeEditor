using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugLoopNode : DebugNode
    {
        private int LoopTimes = 0;
        private int CurTimes = 0;

        public override void OnEnter()
        {
            IntFieldDesigner intFieldDesigner = Node["LoopTimes"].Field as IntFieldDesigner;
            LoopTimes = intFieldDesigner.Value;
            CurTimes = 0;
        }

        public override void OnRunning(float deltatime)
        {
            DebugNode debugNode = Childs[RunningNodeIndex];

            if (LoopTimes == -1)
            {
                debugNode.Update(deltatime);

                if (debugNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                }
                else if (debugNode.Status == DebugNodeStatus.Success || debugNode.Status == DebugNodeStatus.Failed)
                {
                    debugNode.Status = DebugNodeStatus.None;
                }
            }
            else
            {
                CurTimes++;
                //子节点跳过Transition状态
                if (debugNode.Status == DebugNodeStatus.None)
                {
                    debugNode.TransitionElapsedTime = DebugManager.TransitionTime;
                }

                debugNode.Update(deltatime);

                if (debugNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                }
                else if (debugNode.Status == DebugNodeStatus.Success || debugNode.Status == DebugNodeStatus.Failed)
                {
                }
            }

            debugNode.Update(deltatime);

            if (debugNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }
            else if (debugNode.Status == DebugNodeStatus.Failed || debugNode.Status == DebugNodeStatus.Success)
            {
                debugNode.Status = DebugNodeStatus.Transition;
            }

            if (CurTimes >= LoopTimes)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
