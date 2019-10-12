namespace R7BehaviorTree
{
    public interface IProxyManager
    {
        ProxyData GetProxyData(string classType);
        BaseNodeProxy CreateProxy(BaseNode node);

    }
}