using BehaviorTreeData;
using System.Collections.Generic;

namespace R7BehaviorTree
{
    public class CompositeNode : BaseNode
    {
        protected List<BaseNode> Childs { get; set; } = new List<BaseNode>();

        internal void AddChild(BaseNode childNode)
        {
            if (childNode == null || Childs.Contains(childNode))
                return;

            Childs.Add(childNode);
        }

        internal BaseNode GetChild(int id)
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

        internal BaseNode this[int index]
        {
            get { return Childs[index]; }
        }

        internal override void SetContext(BaseContext context)
        {
            base.SetContext(context);

            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i]?.SetContext(context);
            }
        }

        internal override void CreateProxy()
        {
            base.CreateProxy();

            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i]?.CreateProxy();
            }
        }

        internal override void Run(float deltatime)
        {
            base.Run(deltatime);
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].Run(deltatime);
            }
        }

        internal override void SetActive(bool active)
        {
            base.SetActive(active);
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].SetActive(active);
            }
        }

        internal override void Reset()
        {
            if (NodeStatus <= ENodeStatus.Ready)
                return;
            base.Reset();
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].Reset();
            }
        }

        internal override void Destroy()
        {
            if (NodeStatus <= ENodeStatus.Ready)
                return;
            base.Destroy();
            for (int i = 0; i < Childs.Count; i++)
            {
                Childs[i].Destroy();
            }
        }
    }
}