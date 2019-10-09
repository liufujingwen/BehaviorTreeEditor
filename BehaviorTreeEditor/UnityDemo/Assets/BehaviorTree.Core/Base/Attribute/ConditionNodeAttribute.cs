namespace R7BehaviorTree
{
    public class ConditionNodeAttribute : BaseNodeAttribute
    {
        public ConditionNodeAttribute(string classType) : base(classType, ENodeType.Condition)
        {
        }
    }
}