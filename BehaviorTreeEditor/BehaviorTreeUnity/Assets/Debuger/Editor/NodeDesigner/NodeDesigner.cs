using BehaviorTree;
using BehaviorTreeData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
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
