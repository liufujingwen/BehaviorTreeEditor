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
            DebugNode debugNode = Childs[RunningNodeIndex];

            //子节点跳过Transition状态
            if (debugNode.Status == DebugNodeStatus.None)
            {
                debugNode.TransitionElapsedTime = DebugManager.TransitionTime;
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

            if (CurFrames >= Frames)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
