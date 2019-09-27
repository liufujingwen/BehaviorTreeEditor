namespace R7BehaviorTree
{
    public class DecorateNodeAttribute : BaseNodeAttribute
    {
        public DecorateNodeAttribute(string classType) : base(classType, ENodeType.Decorator)
        {
        }
    }
}
