using BehaviorTreeData;

namespace R7BehaviorTree
{
    public interface INodeProxy
    {
        void OnAwake();
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}