namespace R7BehaviorTree
{
    public enum ENodeType
    {
        None,
        Composite,//组合节点
        Decorator,//修饰节点
        Condition,//条件节点
        Action,//叶节点
    }
}