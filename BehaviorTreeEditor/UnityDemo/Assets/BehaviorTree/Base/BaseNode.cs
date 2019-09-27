using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNode : IBaseNode
    {
        public int ID { get; private set; }
        protected NodeData NodeData { get; private set; }
        protected BaseContext Context { get; private set; }
        protected INodeProxy NodeProxy { get; private set; }
        public ENodeStatus NodeStatus { get; set; }
        public string NodeClass { get; private set; }
    
        public BaseNode(NodeData data, BaseContext context)
        {
            NodeData = data;
            ID = NodeData.ID;
            NodeClass = data.ClassType;
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