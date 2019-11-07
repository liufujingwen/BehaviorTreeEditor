using BTData;
using R7BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeViewer
{
    public class NodeDesigner
    {
        public BaseNode baseNode;

        public NodeData NodeData;
        //节点位置
        public Rect Rect;
        //子节点
        public List<Transition> Transitions = new List<Transition>();

        public int ID
        {
            get { return NodeData.ID; }
        }
    }
}
