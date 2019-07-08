using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Time : DebugNode
    {
        int Duration = -1;
        float CurTime = -1;

        public override void OnEnter()
        {
            IntFieldDesigner intFieldDesigner = Node["Duration"].Field as IntFieldDesigner;
            Duration = intFieldDesigner.Value;
            CurTime = 0;
        }

        public override void OnRunning(float deltatime)
        {
            CurTime += deltatime;
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

            if (CurTime >= Duration / 1000f)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
