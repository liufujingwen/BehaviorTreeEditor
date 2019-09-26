namespace BehaviorTree
{
    public interface IBaseNode
    {
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}