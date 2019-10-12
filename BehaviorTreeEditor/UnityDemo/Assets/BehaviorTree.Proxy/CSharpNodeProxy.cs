using System;
using System.Collections.Generic;
using System.Reflection;

namespace R7BehaviorTree
{
    public class CSharpNodeProxy : BaseNodeProxy
    {
        private INodeProxy m_NodeProxy;

        public override void BeginInit()
        {
        }

        public override void EndInit()
        {
            ProxyData proxyData = Node.ProxyData;
            Type type = CSharpProxyManager.Instance.GetType(proxyData.ClassType);
            m_NodeProxy = Activator.CreateInstance(type) as INodeProxy;
        }

        public override void OnAwake()
        {
            m_NodeProxy?.OnAwake();
        }

        public override void OnEnable()
        {
            m_NodeProxy?.OnEnable();
        }

        public override void OnDisable()
        {
            m_NodeProxy?.OnDisable();
        }

        public override void OnStart()
        {
            m_NodeProxy?.OnStart();
        }

        public override void OnUpdate(float deltatime)
        {
            m_NodeProxy?.OnUpdate(deltatime);
        }

        public override void OnReset()
        {
            m_NodeProxy?.OnReset();
        }

        public override void OnDestroy()
        {
            m_NodeProxy?.OnDestroy();
        }
    }
}