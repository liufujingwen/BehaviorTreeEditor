using System;
using System.Xml.Serialization;

namespace BehaviorTreeViewer
{
    public class Transition
    {
        public int FromNodeID;
        public int ToNodeID;

        [XmlIgnore]
        public NodeDesigner FromNode;

        [XmlIgnore]
        public NodeDesigner ToNode;
    
        public void Set(NodeDesigner toNode, NodeDesigner fromNode)
        {
            this.ToNode = toNode;
            this.FromNode = fromNode;

            FromNodeID = fromNode.ID;
            ToNodeID = toNode.ID;

            toNode.ParentNode = fromNode;
        }
    }
}
