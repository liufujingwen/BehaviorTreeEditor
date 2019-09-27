using BehaviorTreeData;

namespace R7BehaviorTree
{
    public interface INodeProxy
    {
        void SetData(NodeData data);
        void SetContext(BaseContext context);
        void OnAwake();
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}