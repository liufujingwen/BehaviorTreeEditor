using BehaviorTreeData;

namespace R7BehaviorTree
{
    public interface IBaseNode
    {
        void SetData(NodeData data);
        void SetContext(BaseContext context);
        void CreateProxy();
        void OnStart();
        void OnUpdate(float deltatime);
        void OnReset();
        void OnDestroy();
    }
}