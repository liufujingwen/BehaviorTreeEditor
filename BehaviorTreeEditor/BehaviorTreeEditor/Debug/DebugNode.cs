using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugNode
    {
        public DebugNodeStatus Status = DebugNodeStatus.None;
        public NodeDesigner Node;
        public List<DebugNode> Childs = new List<DebugNode>();
        public float TransitionElapsedTime = 0;
        private bool EnterState = false; 

        public void DoTransition()
        {
        }

        public void Update(float deltatime)
        {
            if (Status == DebugNodeStatus.Error)
                return;

            if (Status == DebugNodeStatus.Success)
                return;

            if (Status == DebugNodeStatus.None)
            {
                TransitionElapsedTime = 0;
                Status = DebugNodeStatus.Transition;
            }
            else if (Status == DebugNodeStatus.Transition)
            {
                TransitionElapsedTime += deltatime;
                if (TransitionElapsedTime >= DebugManager.TransitionTime)
                {
                    Enter();
                }
            }
            else if (Status != DebugNodeStatus.Error && Status == DebugNodeStatus.Running)
            {
                Running(deltatime);
            }
        }

        private void Enter()
        {
            OnEnter();
            if (Status == DebugNodeStatus.Error)
                return;
            Status = DebugNodeStatus.Running;
        }

        public virtual void OnEnter()
        {
        }

        private void Running(float deltatime)
        {
            if (Status == DebugNodeStatus.Error)
                return;
            OnRunning(deltatime);
        }

        public virtual void OnRunning(float deltatime)
        {
        }
    }
}
