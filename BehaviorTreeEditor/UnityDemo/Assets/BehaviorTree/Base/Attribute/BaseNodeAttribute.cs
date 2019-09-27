using System;

namespace R7BehaviorTree
{
    public abstract class BaseNodeAttribute : Attribute
    {
        public BaseNodeAttribute(string classType, ENodeType nodeType)
        {
            ClassType = classType;
            NodeType = nodeType;
        }

        public string ClassType { get; set; }
        public ENodeType NodeType { get; set; } = ENodeType.Action;
    }
}