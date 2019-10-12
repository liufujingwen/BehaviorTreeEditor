using BehaviorTreeData;

namespace R7BehaviorTree
{
    /// <summary>
    /// 输出指定Log,方便调试
    /// </summary>
    [ActionNode("Log")]
    public class LogProxy : BaseNodeProxy
    {
        private string m_Content;

        public override void OnAwake()
        {
            StringField contentField = Node.NodeData["Content"] as StringField;
            if (contentField == null)
            {
                Node.Status = ENodeStatus.Error;
                return;
            }

            m_Content = contentField;
        }

        public override void OnStart()
        {
            BehaviorTreeManager.Instance.Log(m_Content);
        }
    }
}