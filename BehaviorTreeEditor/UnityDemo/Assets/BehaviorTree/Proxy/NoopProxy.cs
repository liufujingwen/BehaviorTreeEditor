namespace R7BehaviorTree
{
    [ActionNode("Noop")]
    public class NoopProxy : CSharpNodeProxy
    {
        public override void OnStart()
        {
        }
    }
}