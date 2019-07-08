using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Debug__Parallel : DebugNode
    {
        public enum SUCCESS_POLICY
        {
            SUCCEED_ON_ONE = 1,//当某一个节点返回成功时退出；
            SUCCEED_ON_ALL = 2//当全部节点都返回成功时退出；
        }

        public enum FAILURE_POLICY
        {
            FAIL_ON_ONE = 1,//当某一个节点返回失败时退出；
            FAIL_ON_ALL = 2,//当全部节点都返回失败时退出；
        }

        public SUCCESS_POLICY SuccessPolicy;
        public FAILURE_POLICY FailurePolicy;



        public override void OnRunning(float deltatime)
        {
            int failCount = 0;
            int successCount = 0;

            for (int i = 0; i < Childs.Count; i++)
            {
                DebugNode childNode = Childs[i];
                childNode.Update(deltatime);

                if (childNode.Status == DebugNodeStatus.Failed)
                {
                    failCount++;

                    if (FailurePolicy == FAILURE_POLICY.FAIL_ON_ONE)
                    {
                        Status = DebugNodeStatus.Failed;
                        break;
                    }
                    else if (FailurePolicy == FAILURE_POLICY.FAIL_ON_ALL && failCount == Childs.Count)
                    {
                        Status = DebugNodeStatus.Failed;
                    }
                }
                else if (childNode.Status == DebugNodeStatus.Success)
                {
                    successCount++;

                    if (SuccessPolicy == SUCCESS_POLICY.SUCCEED_ON_ONE)
                    {
                        Status = DebugNodeStatus.Success;
                    }
                    else if (SuccessPolicy == SUCCESS_POLICY.SUCCEED_ON_ALL && successCount == Childs.Count)
                    {
                        Status = DebugNodeStatus.Success;
                    }
                }
                else if (childNode.Status == DebugNodeStatus.Error)
                {
                    Status = DebugNodeStatus.Error;
                }
            }
        }
    }
}
