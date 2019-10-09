using BehaviorTreeData;
using System;
using System.Collections.Generic;

namespace BehaviorTreeViewer
{
    public class Debugger
    {
        public AgentData AgentData;
        public List<NodeDesigner> Nodes = new List<NodeDesigner>();

        /// <summary>
        /// 通过ID查找节点
        /// </summary>
        /// <param name="ID">节点ID</param>
        /// <returns></returns>
        public NodeDesigner FindNodeByID(int ID)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.ID == ID)
                    return node;
            }
            return null;
        }
    }
}