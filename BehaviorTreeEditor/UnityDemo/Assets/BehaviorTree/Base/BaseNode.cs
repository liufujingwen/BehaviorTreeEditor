using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNode : IBaseNode
    {
        public int ID { get; private set; }
        protected NodeData NodeData { get; private set; }
        protected BaseContext Context { get; private set; }
        protected INodeProxy NodeProxy { get; private set; }
        public ProxyData ProxyData { get; set; }
        public ENodeStatus NodeStatus { get; set; }
        public string NodeClass { get; private set; }
        public ENodeType NodeType { get; set; }
       
        public virtual void SetData(NodeData data)
        {
            NodeData = data;
            ID = NodeData.ID;
            NodeClass = data.ClassType;
        }

        public virtual void SetContext(BaseContext context)
        {
            Context = context;
        }

        public virtual void CreateProxy()
        {
            NodeProxy = BehaviorTreeManager.Instance.CreateProxy(ProxyData);
        }

        public virtual void OnStart()
        {
            NodeProxy?.OnStart();
        }

        public virtual void OnUpdate(float deltatime)
        {
            NodeProxy?.OnUpdate(deltatime);
        }

        public virtual void OnReset()
        {
            NodeProxy?.OnReset();
        }

        public virtual void OnDestroy()
        {
            NodeProxy?.OnDestroy();
        }
    }
}