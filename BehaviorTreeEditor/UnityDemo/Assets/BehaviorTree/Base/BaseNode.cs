using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNode : IBaseNode
    {
        public int ID { get; private set; }
        public NodeData NodeData { get; private set; }
        public BaseContext Context { get; private set; }
        public INodeProxy NodeProxy { get; private set; }
        public ProxyData ProxyData { get; set; }
        public ENodeStatus NodeStatus { get; set; }
        public string ClassType { get; private set; }
        public ENodeType NodeType { get; set; }
       
        public virtual void SetData(NodeData data)
        {
            NodeData = data;
            ID = NodeData.ID;
            ClassType = data.ClassType;
        }

        public virtual void SetContext(BaseContext context)
        {
            Context = context;
        }

        public virtual void CreateProxy()
        {
            NodeProxy = BehaviorTreeManager.Instance.CreateProxy(this);
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