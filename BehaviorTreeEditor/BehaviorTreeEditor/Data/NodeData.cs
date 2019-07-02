using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class NodeData
    {
        //节点唯一标识
        public int ID;
        //名字
        public string Name = string.Empty;
        //节点类名
        public string ClassType = string.Empty;
        //节点类型
        public NodeType NodeType = NodeType.Composite;
        //节点位置
        public Rect Rect;
        //子节点
        public List<NodeDesigner> Childs = null;
    }
}
