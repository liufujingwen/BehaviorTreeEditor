using BehaviorTreeData;

namespace BehaviorTree
{
    public class CompositeNode : BaseNode
    {
        public CompositeNode(NodeData data, BaseContext context) : base(data, context)
        {
        }

        public override void OnStart()
        {
            NodeProxy?.OnStart();
        }

        public override void OnUpdate(float deltatime)
        {
            NodeProxy?.OnUpdate(deltatime);
        }

        public override void OnReset()
        {
            NodeProxy?.OnReset();
        }

        public override void OnDestroy()
        {
            NodeProxy?.OnDestroy();
        }
    }
}