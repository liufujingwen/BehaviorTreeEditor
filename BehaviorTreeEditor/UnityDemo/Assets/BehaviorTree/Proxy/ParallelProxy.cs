using BehaviorTreeData;

namespace R7BehaviorTree
{
    [CompositeNode("Parallel")]
    public class ParallelProxy : CSharpNodeProxy
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

        private CompositeNode m_CompositeNode;
        public SUCCESS_POLICY m_SuccessPolicy;
        public FAILURE_POLICY m_FailurePolicy;

        public override void OnAwake()
        {
            EnumField successPolicy = NodeData["SuccessType"] as EnumField;
            EnumField failedPolicy = NodeData["FailType"] as EnumField;

            if (successPolicy == null || failedPolicy == null)
            {
                Node.Status = ENodeStatus.Error;
            }

            m_SuccessPolicy = (SUCCESS_POLICY)successPolicy.Value;
            m_FailurePolicy = (FAILURE_POLICY)failedPolicy.Value;

            m_CompositeNode = Node as CompositeNode;
        }

        public override void OnUpdate(float deltatime)
        {
            int failCount = 0;
            int successCount = 0;

            for (int i = 0; i < m_CompositeNode.Childs.Count; i++)
            {
                BaseNode childNode = m_CompositeNode.Childs[i];
                childNode.Run(deltatime);
                ENodeStatus childNodeStatus = childNode.Status;

                if (childNodeStatus == ENodeStatus.Failed)
                {
                    failCount++;

                    if (m_FailurePolicy == FAILURE_POLICY.FAIL_ON_ONE)
                    {
                        Node.Status = ENodeStatus.Failed;
                        break;
                    }
                    else if (m_FailurePolicy == FAILURE_POLICY.FAIL_ON_ALL && failCount == m_CompositeNode.Childs.Count)
                    {
                        Node.Status = ENodeStatus.Failed;
                    }
                }
                else if (childNode.Status == ENodeStatus.Succeed)
                {
                    successCount++;

                    if (m_SuccessPolicy == SUCCESS_POLICY.SUCCEED_ON_ONE)
                    {
                        Node.Status = ENodeStatus.Succeed;
                    }
                    else if (m_SuccessPolicy == SUCCESS_POLICY.SUCCEED_ON_ALL && successCount == m_CompositeNode.Childs.Count)
                    {
                        Node.Status = ENodeStatus.Succeed;
                    }
                }
                else if (childNode.Status == ENodeStatus.Error)
                {
                    Node.Status = ENodeStatus.Error;
                }
            }
        }
    }
}