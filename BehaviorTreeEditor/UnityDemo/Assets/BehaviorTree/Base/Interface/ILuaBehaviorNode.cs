using System.Collections;

namespace R7BehaviorTree
{
    public interface ILuaBehaviorNode
    {
        void OnAwake();
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}
