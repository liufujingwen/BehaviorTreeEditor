using BehaviorTreeData;
using System.Collections.Generic;

namespace R7BehaviorTree
{
    public class CompositeNode : BaseNode
    {
        protected List<BaseNode> Childs { get; set; } = new List<BaseNode>();

        public CompositeNode(NodeData data, BaseContext context) : base(data, context)
        {
        }

        public void AddChild(BaseNode childNode)
        {
            if (childNode == null || Childs.Contains(childNode))
                return;

            Childs.Add(childNode);
        }

        public BaseNode GetChild(int id)
        {
            for (int i = 0; i < Childs.Count; i++)
            {
                BaseNode baseNode = Childs[i];
                if (baseNode == null)
                    continue;
                if (baseNode.ID == id)
                    return baseNode;
            }
            return null;
        }

        public BaseNode GetChildByIndex(int index)
        {
            for (int i = 0; i < Childs.Count; i++)
            {

            }
            return null;
        }

        public BaseNode this[int index]
        {
            get { return Childs[index]; }
        }

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