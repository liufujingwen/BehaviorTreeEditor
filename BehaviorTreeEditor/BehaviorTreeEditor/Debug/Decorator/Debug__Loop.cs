using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Loop : DebugNode
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
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (LoopTimes == -1)
            {
                runningNode.Update(deltatime);

                if (runningNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                }
                else if (runningNode.Status == DebugNodeStatus.Success || runningNode.Status == DebugNodeStatus.Failed)
                {
                    runningNode.Status = DebugNodeStatus.None;
                }
            }
            else
            {
                CurTimes++;
                //子节点跳过Transition状态
                if (runningNode.Status == DebugNodeStatus.None)
                {
                    runningNode.Status = DebugNodeStatus.Transition;
                    runningNode.TransitionElapsedTime = DebugManager.TransitionTime;
                }

                runningNode.Update(deltatime);

                if (runningNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                }
                else if (runningNode.Status == DebugNodeStatus.Success || runningNode.Status == DebugNodeStatus.Failed)
                {
                }
            }

            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }
            else if (runningNode.Status == DebugNodeStatus.Failed || runningNode.Status == DebugNodeStatus.Success)
            {
                runningNode.Status = DebugNodeStatus.Transition;
            }

            if (CurTimes >= LoopTimes)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
