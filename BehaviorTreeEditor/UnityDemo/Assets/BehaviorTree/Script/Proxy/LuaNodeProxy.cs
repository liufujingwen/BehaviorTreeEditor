using BehaviorTreeData;

namespace BehaviorTree
{
    public class LuaNodeProxy : BaseNodeProxy
    {
        public override void SetData(NodeData data)
        {
            base.SetData(data);
        }

        public override void SetContext(BaseContext context)
        {
            base.SetContext(context);
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate(float deltatime)
        {
        }

        public override void OnReset()
        {
        }

        public override void OnDestroy()
        {
        }
    }
}