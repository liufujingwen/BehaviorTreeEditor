using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Wait : DebugNode
    {
        private int WaitTime;
        private float time;

        public override void OnEnter()
        {
            time = 0;
            IntFieldDesigner intFieldDesigner = Node["Millisecond"].Field as IntFieldDesigner;
            if (intFieldDesigner == null)
            {
                Status = DebugNodeStatus.Error;
                return;
            }
            WaitTime = intFieldDesigner.Value;
        }

        public override void OnRunning(float deltatime)
        {
            time += deltatime;

            if (time >= WaitTime / 1000f)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}
