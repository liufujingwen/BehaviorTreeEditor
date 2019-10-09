using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNodeProxy
    {
        public NodeData NodeData { get; private set; }
        public IContext Context { get; private set; }
        public BaseNode Node { get; private set; }

        public virtual void SetData(NodeData data)
        {
            NodeData = data;
        }

        public virtual void SetContext(IContext context)
        {
            Context = context;
        }

        public void SetNode(BaseNode node)
        {
            Node = node;
        }

        public virtual void OnAwake()
        {
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
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