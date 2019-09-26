using BehaviorTreeData;

namespace BehaviorTree
{
    public class BehaviorTreeManager
    {
        private static readonly BehaviorTreeManager ms_Instance = new BehaviorTreeManager();

        public static BehaviorTreeManager Instance
        {
            get { return ms_Instance; }
        }

        public INodeProxy CreateProxy(NodeData data)
        {
            return null;
        }

        public void OnUpdate()
        {
        }


    }
}