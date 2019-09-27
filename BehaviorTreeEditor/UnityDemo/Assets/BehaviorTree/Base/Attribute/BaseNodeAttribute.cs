using System;

namespace R7BehaviorTree
{
    public abstract class BaseNodeAttribute : Attribute
    {
        public BaseNodeAttribute(string classType)
        {
            ClassType = classType;
        }

        public string ClassType { get; set; }
    }
}
