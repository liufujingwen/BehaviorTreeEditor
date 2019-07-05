﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugIfElseNode : DebugNode
    {
        int CurrentRunningIndex = -1;
        private DebugNode ConditionNode = null;

        public override void OnEnter()
        {
            if (Childs.Count != 3)
                Status = DebugNodeStatus.Error;

            CurrentRunningIndex = 0;
            ConditionNode = Childs[0];
        }

        public override void OnRunning(float deltatime)
        {
            if (CurrentRunningIndex == 0)
            {
                ConditionNode.Update(deltatime);

                if (ConditionNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                    return;
                }
            }

            if (ConditionNode.Status == DebugNodeStatus.Success)
            {
                CurrentRunningIndex = 1;
            }
            else if (ConditionNode.Status == DebugNodeStatus.Failed)
            {
                CurrentRunningIndex = 2;
            }

            DebugNode debugNode = Childs[CurrentRunningIndex];

            if (debugNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            Status = debugNode.Status;
        }
    }
}
