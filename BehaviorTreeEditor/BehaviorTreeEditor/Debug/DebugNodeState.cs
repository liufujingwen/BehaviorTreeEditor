using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public enum DebugNodeStatus
    {
        None,
        Transition,
        Running,
        Success,
        Failed,
        Error,
    }
}
