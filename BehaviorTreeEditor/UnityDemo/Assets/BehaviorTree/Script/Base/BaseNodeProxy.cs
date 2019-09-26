using BehaviorTreeData;

namespace BehaviorTree
{
    public abstract class BaseNodeProxy : INodeProxy
    {
        public NodeData NodeData { get; private set; }
        public BaseContext Context { get; private set; }

        public virtual void SetData(NodeData data)
        {
            NodeData = data;
        }

        public virtual void SetContext(BaseContext context)
        {
            Context = context;
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