using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNodeProxy : INodeProxy
    {
        public NodeData NodeData { get; private set; }
        public BaseContext Context { get; private set; }
        public BaseNode Node { get; private set; }

        internal virtual void SetData(NodeData data)
        {
            NodeData = data;
        }

        internal virtual void SetContext(BaseContext context)
        {
            Context = context;
        }

        internal void SetNode(BaseNode node)
        {
            Node = node;
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void OnUpdate(float deltatime)
        {
        }

        public virtual void OnReset()
        {
        }

        public virtual void OnDestroy()
        {
        }
    }
}