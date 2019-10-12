using System;
using System.Collections.Generic;

namespace R7BehaviorTree
{
    public sealed class LuaProxyManager : Singleton<LuaProxyManager>, IProxyManager
    {
        private Dictionary<string, ProxyData> m_ProxyDic = new Dictionary<string, ProxyData>();

        public void Initalize()
        {
            BehaviorTreeManager.Instance.AddProxyManager(this);
        }

        public ProxyData GetProxyData(string classType)
        {
            ProxyData proxyData = null;
            m_ProxyDic.TryGetValue(classType, out proxyData);
            return proxyData;
        }

        public BaseNodeProxy CreateProxy()
        {
            return new LuaNodeProxy();
        }

        /// <summary>
        /// 注册Proxy
        /// </summary>
        /// <param name="classType">节点类</param>
        /// <param name="nodeType">节点类型</param>
        /// <param name="needUpdate">是否执行proxy的update(优化性能)</param>
        public void Register(string classType, ENodeType nodeType,bool needUpdate)
        {
            if (string.IsNullOrEmpty(classType))
            {
                string msg = "LuaProxyManager.Register() \n classType is null.";
                BehaviorTreeManager.Instance.LogError(msg);
                throw new Exception(msg);
            }

            if (m_ProxyDic.ContainsKey(classType))
            {
                string msg = $"LuaProxyManager.Register() \n m_ProxyDic already Contain key {classType}.";
                BehaviorTreeManager.Instance.LogError(msg);
                throw new Exception(msg);
            }

            ProxyData proxyData = new ProxyData();
            proxyData.ClassType = classType;
            proxyData.NodeType = nodeType;
            proxyData.NeedUpdate = needUpdate;

            m_ProxyDic.Add(classType, proxyData);
        }

        public EProxyType GetProxyType()
        {
            return EProxyType.Lua;
        }
    }
}