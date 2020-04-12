namespace BehaviorTreeEditor
{
    public class Debug__Log : DebugNode
    {
        public override void OnEnter()
        {
            Status = DebugNodeStatus.Success;
        }
    }
}
