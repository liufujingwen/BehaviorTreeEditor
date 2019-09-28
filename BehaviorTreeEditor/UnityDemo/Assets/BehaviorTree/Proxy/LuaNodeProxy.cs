#if LUA

using BehaviorTreeData;
using System;
using XLua;

namespace R7BehaviorTree
{
    public class LuaNodeProxy : BaseNodeProxy
    {
        public static LuaEnv LuaEnv { get; set; }
        static Func<string, LuaNodeProxy, ILuaBehaviorNode> NewFunc = null;
        private ILuaBehaviorNode m_LuaBehaviorNode;

        public LuaNodeProxy()
        {
            if (NewFunc == null)
                NewFunc = LuaEnv.Global.GetInPath<Func<string, LuaNodeProxy, ILuaBehaviorNode>>("LuaUIManager.New");
        }

        //构建lua行为树实例
        internal void CreateLuaObj()
        {
            m_LuaBehaviorNode = NewFunc?.Invoke(NodeData.ClassType, this);
        }

        public override void OnAwake()
        {
            m_LuaBehaviorNode?.OnAwake();
        }

        public override void OnStart()
        {
            m_LuaBehaviorNode?.OnStart();
        }

        public override void OnUpdate(float deltatime)
        {
            if (Node.ProxyData.NeedUpdate)
                m_LuaBehaviorNode?.OnUpdate(deltatime);
        }

        public override void OnReset()
        {
            m_LuaBehaviorNode?.OnStart();
        }

        public override void OnDestroy()
        {
            m_LuaBehaviorNode.OnDestroy();
        }
    }
}

#endif