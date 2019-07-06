using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    /// <summary>
    /// 与顺序节点类似，创建时需要传入一个节点列表，
    /// 当运行到这个节点时，他的节点会一个接一个的运行。
    /// 如果他的子节点是SUCCESS，那么他会将自身标识成为SUCCESS并且直接返回；
    /// 如果他的子节点状态是RUNNING，那么他会将自身也标识成RUNNING，并且等待节点返回其他结果；
    /// 如果他的子节点状态是FAILED，那么他会运行下一个。
    /// 任何一个节点都没有返回SUCCESS的情况下，他将会将自身标识成为FAILED并且返回
    /// </summary>
    public class DebugSelectorNode : DebugNode
    {
        public override void OnRunning(float deltatime)
        {
            DebugNode runningNode = Childs[RunningNodeIndex];
            runningNode.Update(deltatime);

            if (runningNode.Status == DebugNodeStatus.Error)
            {
                Status = DebugNodeStatus.Error;
                return;
            }

            if (runningNode.Status == DebugNodeStatus.Failed)
            {
                RunningNodeIndex++;
                //所有运行失败将返回失败
                if (RunningNodeIndex >= Childs.Count)
                {
                    Status = DebugNodeStatus.Failed;
                    return;
                }
            }
            else if (runningNode.Status == DebugNodeStatus.Success)
            {
                Status = DebugNodeStatus.Success;
            }
        }
    }
}