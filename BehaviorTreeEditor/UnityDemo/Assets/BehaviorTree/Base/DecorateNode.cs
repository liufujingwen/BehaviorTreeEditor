using BehaviorTreeData;

namespace R7BehaviorTree
{
    public class DecorateNode : CompositeNode
    {
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