using BehaviorTreeData;

namespace BehaviorTree
{
    public interface INodeProxy
    {
        void SetData(NodeData data);
        void SetContext(BaseContext context);
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}