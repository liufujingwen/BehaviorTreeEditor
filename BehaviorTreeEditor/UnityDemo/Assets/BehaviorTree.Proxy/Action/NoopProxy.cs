namespace R7BehaviorTree
{
    [ActionNode("Noop")]
    public class NoopProxy : BaseNodeProxy
    {
        public override void OnStart()
        {
            Node.Status = ENodeStatus.Succeed;
        }
    }
}