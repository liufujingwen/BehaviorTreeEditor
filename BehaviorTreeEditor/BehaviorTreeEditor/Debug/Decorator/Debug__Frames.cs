using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Frames : DebugNode
    {
        int Frames = -1;
        int CurFrames = -1;

        public override void OnEnter()
        {
            IntFieldDesigner intFieldDesigner = Node["Frames"].Field as IntFieldDesigner;
            Frames = intFieldDesigner.Value;
            CurFrames = 0;
        }

        public override void OnRunning(float deltatime)
        {
            CurFrames++;
            DebugNode runningNode = Childs[RunningNodeIndex];

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
                return;
            }

            if (CurFrames >= Frames)
            {
                Status = DebugNodeStatus.Success;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed || runningNode.Status == DebugNodeStatus.Success)
            {
                runningNode.Status = DebugNodeStatus.Transition;
            }
        }
    }
}
