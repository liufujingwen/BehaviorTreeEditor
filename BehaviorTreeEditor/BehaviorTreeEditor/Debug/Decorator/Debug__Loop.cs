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

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed || runningNode.Status == DebugNodeStatus.Success)
                CurTimes++;

            if (LoopTimes != -1 && CurTimes >= LoopTimes)
            {
                Status = DebugNodeStatus.Success;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed || runningNode.Status == DebugNodeStatus.Success)
            {
                runningNode.Status = DebugNodeStatus.None;
            }
        }
    }
}