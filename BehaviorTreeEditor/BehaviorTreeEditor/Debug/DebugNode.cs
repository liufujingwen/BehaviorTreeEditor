using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugNode
    {
        public bool CanChangeStatus = false;
        public DebugNodeStatus Status = DebugNodeStatus.None;
        public int RunningNodeIndex = 0;
        public NodeDesigner Node;
        public DebugNode ParentNode;
        public List<DebugNode> Childs = new List<DebugNode>();
        public List<PointF> TransitionPoints = new List<PointF>();
        public float TransitionElapsedTime = 0;
        public float RunningElapsedTime = 0;
        public float RunningAlpha = 0f;
        public float SuccessAlpha = 0f;


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
            RunningElapsedTime = 0;
            RunningNodeIndex = 0;
            RunningAlpha = 0;
            SuccessAlpha = 0;
            OnEnter();
            if (Status == DebugNodeStatus.Error || Status == DebugNodeStatus.Success)
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
            RunningElapsedTime += deltatime;
            OnRunning(deltatime);
        }

        public virtual void OnRunning(float deltatime)
        {
        }
    }
}
