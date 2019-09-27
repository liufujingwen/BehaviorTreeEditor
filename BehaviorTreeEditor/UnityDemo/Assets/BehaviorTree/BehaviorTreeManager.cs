using BehaviorTreeData;
using System.Collections.Generic;
using UnityEngine;

namespace R7BehaviorTree
{
    public class BehaviorTreeManager
    {
        public static BehaviorTreeManager Instance { get; } = new BehaviorTreeManager();
        //正在运行的行为树
        public List<BehaviorTree> ActiveBehaviorTrees = new List<BehaviorTree>();

        //创建行为树
        public void CreateBehaviorTree(AgentData agent)
        {
            if (agent == null)
                return;

            BehaviorTree behaviorTree = new BehaviorTree();
            behaviorTree.SetAgent(agent);
        }

        public INodeProxy CreateProxy(NodeData data)
        {
            return null;
        }

        public void OnUpdate()
        {
            if (ActiveBehaviorTrees.Count > 0)
            {
                for (int i = 0; i < ActiveBehaviorTrees.Count; i++)
                {
                    BehaviorTree behaviorTree = ActiveBehaviorTrees[i];

                    if (behaviorTree == null)
                        continue;
                    if (behaviorTree.Status == ENodeStatus.Failed)
                        continue;
                    if (behaviorTree.Status == ENodeStatus.Succeed)
                        continue;
                    if (behaviorTree.Status == ENodeStatus.Error)
                        continue;

                    behaviorTree.OnUpdate(Time.deltaTime);
                }
            }
        }
    }
}