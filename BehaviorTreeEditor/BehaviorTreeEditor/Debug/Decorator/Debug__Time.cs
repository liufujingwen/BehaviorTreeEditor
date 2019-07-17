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
            
            if (CurTime >= Duration / 1000f)
            {
                Status = DebugNodeStatus.Success;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed || runningNode.Status == DebugNodeStatus.Success)
            {
                SetChildState(DebugNodeStatus.Transition);
            }
        }
    }
}
