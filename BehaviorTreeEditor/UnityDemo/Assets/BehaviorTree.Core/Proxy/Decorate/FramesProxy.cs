using BTData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 执行n帧后返回succeed，如果子节点返回succeed、failed,则重置子节点继续执行
    /// </summary>
    [DecorateNode("Frames")]
    public class FramesProxy : BaseNodeProxy
    {
        private int m_Frames = -1;
        private int m_CurFrames = -1;
        private CompositeNode m_CompositeNode;

        public override void OnAwake()
        {
            IntField framesField = Node.NodeData["Frames"] as IntField;
            if (framesField == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            m_Frames = framesField;
        }

        public override void OnStart()
        {
            m_CurFrames = 0;
            m_CompositeNode = Node as CompositeNode;
        }

        public override void OnUpdate(float deltatime)
        {
            m_CurFrames++;

            BaseNode childNode = m_CompositeNode.Childs[0];
            childNode.Run(deltatime);
            ENodeStatus childNodeStatus = childNode.Status;

            if (childNodeStatus == ENodeStatus.Error)
            {
                m_CompositeNode.Status = ENodeStatus.Error;
                return;
            }

            if (m_CurFrames >= m_Frames)
            {
                m_CompositeNode.Status = ENodeStatus.Succeed;
                return;
            }

            if (childNodeStatus == ENodeStatus.Failed || childNodeStatus == ENodeStatus.Succeed)
            {
                childNode.Reset();
            }
        }
    }
}