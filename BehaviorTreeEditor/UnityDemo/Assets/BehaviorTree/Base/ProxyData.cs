using System;

namespace R7BehaviorTree
{
    public class ProxyData
    {
        /// <summary>
        /// 对应的节点类
        /// </summary>
        public string ClassType { get; set; }

        /// <summary>
        /// 是否LuaProxy
        /// </summary>
        public bool IsLuaProxy { get; set; }

        /// <summary>
        /// 是否需要执行OnUpdate（lua需要Update才开启）
        /// </summary>
        public bool NeedUpdate { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public ENodeType NodeType { get; set; }

        /// <summary>
        /// C#对应的Proxy
        /// </summary>
        public Type ProxyType { get; set; }
    }
}