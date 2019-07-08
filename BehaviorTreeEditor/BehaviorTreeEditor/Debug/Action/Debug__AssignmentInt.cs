using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__AssignmentInt : DebugNode
    {
        public override void OnEnter()
        {
            Status = DebugNodeStatus.Success;
        }
    }
}
