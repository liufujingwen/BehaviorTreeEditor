using BehaviorTreeData;

namespace BehaviorTree
{
    public abstract class BaseNode : IBaseNode
    {
        protected NodeData NodeData { get; private set; }
        protected BaseContext Context { get; private set; }
        protected INodeProxy NodeProxy { get; private set; }
        public ENodeStatus NodeStatus { get; set; }
        public string NodeClass { get { return NodeData.ClassType; } }

        public BaseNode(NodeData data, BaseContext context)
        {
            NodeData = data;
            Context = context;
            NodeProxy = BehaviorTreeManager.Instance.CreateProxy(data);
            NodeProxy.SetData(NodeData);
            NodeProxy.SetContext(Context);
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