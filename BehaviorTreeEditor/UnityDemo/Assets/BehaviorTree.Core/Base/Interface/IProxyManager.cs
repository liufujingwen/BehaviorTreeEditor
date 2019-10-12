namespace R7BehaviorTree
{
    public interface IProxyManager
    {
        EProxyType GetProxyType();
        ProxyData GetProxyData(string classType);
        BaseNodeProxy CreateProxy();

    }
}