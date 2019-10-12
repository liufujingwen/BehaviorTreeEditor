
namespace R7BehaviorTree
{
    public interface INodeProxy
    {
        void OnAwake();
        void OnEnable();
        void OnDisable();
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}

