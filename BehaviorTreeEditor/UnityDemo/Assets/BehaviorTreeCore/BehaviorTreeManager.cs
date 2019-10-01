using System;
using BehaviorTreeData;
using System.Collections.Generic;

namespace R7BehaviorTree
{
    public class BehaviorTreeManager
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static BehaviorTreeManager Instance { get; } = new BehaviorTreeManager();

        /// <summary>
        /// Log回调
        /// </summary>
        public Action<ELogType, string> LogHandler;

        /// <summary>
        /// 正在运行的行为树
        /// </summary>
        public List<BehaviorTree> Runnings = new List<BehaviorTree>();

        /// <summary>
        /// 管理行为树编辑器数据
        /// </summary>
        public static readonly Dictionary<int, TreeData> TreeDataDic = new Dictionary<int, TreeData>();

        /// <summary>
        /// C#所有Proxy
        /// </summary>
        public static readonly Dictionary<string, ProxyData> CSharpProxyDic = new Dictionary<string, ProxyData>();

        /// <summary>
        /// Lua所有Proxy
        /// </summary>
        public static readonly Dictionary<string, ProxyData> LuaProxyDic = new Dictionary<string, ProxyData>();

        /// <summary>
        /// 行为树缓存池
        /// </summary>
        public static readonly Dictionary<int, Dictionary<string, Queue<BehaviorTree>>> PoolDic = new Dictionary<int, Dictionary<string, Queue<BehaviorTree>>>();

        /// <summary>
        /// 收集所有C#端的Proxy信息
        /// </summary>
        public void CollectProxyInfos()
        {
            Type[] types = typeof(BaseNodeProxy).Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                object[] objs = type.GetCustomAttributes(typeof(BaseNodeAttribute), true);
                if (objs == null || objs.Length == 0)
                    continue;

                BaseNodeAttribute baseNodeAttribute = objs[0] as BaseNodeAttribute;

                if (baseNodeAttribute == null)
                    continue;

                string classType = baseNodeAttribute.ClassType;

                if (string.IsNullOrEmpty(classType))
                {
                    string msg = "BehaviorTreeManager.CollectProxyInfos() \n classType is null.";
                    LogError(msg);
                    throw new Exception(msg);
                }

                if (CSharpProxyDic.ContainsKey(classType))
                {
                    string msg = $"BehaviorTreeManager.CollectProxyInfos() \n CSharpProxyDic exist key:{classType}.";
                    LogError(msg);
                    throw new Exception(msg);
                }

                ProxyData proxyData = new ProxyData();
                proxyData.ClassType = classType;
                proxyData.NodeType = baseNodeAttribute.NodeType;
                proxyData.ProxyType = type;

                CSharpProxyDic.Add(classType, proxyData);
            }
        }

        /// <summary>
        /// 注册lua proxy
        /// </summary>
        /// <param name="classType">节点类</param>
        /// <param name="nodeType">节点类型</param>
        /// <param name="needUpdate">是否执行proxy的update(优化lua性能)</param>
        public void RegisterLuaProxy(string classType, ENodeType nodeType, bool needUpdate)
        {
            if (string.IsNullOrEmpty(classType))
            {
                string msg = "BehaviorTreeManager.RegisterLuaProxy() \n classType is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            if (LuaProxyDic.ContainsKey(classType))
            {
                string msg = $"BehaviorTreeManager.CollectProxyInfos() \n CSharpProxyDic exist key:{classType}.";
                LogError(msg);
                throw new Exception(msg);
            }

            ProxyData proxyData = new ProxyData();
            proxyData.ClassType = classType;
            proxyData.NodeType = nodeType;
            proxyData.IsLuaProxy = true;
            proxyData.NeedUpdate = needUpdate;

            LuaProxyDic.Add(classType, proxyData);
        }

        /// <summary>
        /// 加载指定类型的行为树
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        public void LoadBehaviorData(int behaviorTreeType, byte[] bytes)
        {
            TreeData treeData = Serializer.DeSerialize<TreeData>(bytes);
            if (treeData == null)
                throw new Exception($"Load {behaviorTreeType} treeData failed.");
            TreeDataDic[behaviorTreeType] = treeData;
        }

        /// <summary>
        /// 根据类型获取AgentData
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        /// <param name="id">AgentId</param>
        /// <returns></returns>
        public AgentData GetAgentData(int behaviorTreeType, string id)
        {
            TreeData treeData = null;

            if (!TreeDataDic.TryGetValue(behaviorTreeType, out treeData))
                throw new Exception($"There is no treeData,BehaviorTreeType:{behaviorTreeType}.");

            if (treeData == null)
                return null;

            for (int i = 0; i < treeData.Agents.Count; i++)
            {
                AgentData agentData = treeData.Agents[i];
                if (agentData != null && agentData.ID == id)
                    return agentData;
            }

            string msg = $"There is no AgentData,BehaviorTreeType:{behaviorTreeType} id:{id}.";
            LogError(msg);
            throw new Exception(msg);
        }

        /// <summary>
        /// 创建行为树
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        /// <param name="agentId">agentId</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public BehaviorTree CreateBehaviorTree(int behaviorTreeType, string agentId, BaseContext context)
        {
            BehaviorTree behaviorTree = Spawn(behaviorTreeType, agentId);

            if (behaviorTree == null)
            {
                string msg = $"BehaviorTreeManager.CreateBehaviorTree() \n Create failed, EBehaviorTreeType:{behaviorTreeType} AgentId:{agentId}.";
                LogError(msg);
                throw new Exception(msg);
            }

            behaviorTree.SetContext(context);
            behaviorTree.CreateProxy();

            return behaviorTree;
        }

        /// <summary>
        /// 激活行为树
        /// </summary>
        /// <param name="behaviorTree"></param>
        public void RunBehaviorTree(BehaviorTree behaviorTree)
        {
            if (behaviorTree == null)
            {
                string msg = "BehaviorTreeManager.ActiveBehaviorTree() \n Run failed, behaviorTree is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            if (Runnings.Contains(behaviorTree))
            {
                string msg = "BehaviorTreeManager.ActiveBehaviorTree() \n behaviorTree already run";
                LogError(msg);
                throw new Exception(msg);
            }

            Runnings.Add(behaviorTree);
            behaviorTree.Run(0);
        }

        #region Pool

        /// <summary>
        /// 从池中产生行为树，如果没有就创建
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        /// <param name="agentId">AgentId</param>
        /// <returns>行为树</returns>
        private BehaviorTree Spawn(int behaviorTreeType, string agentId)
        {
            Queue<BehaviorTree> queue = null;

            do
            {
                Dictionary<string, Queue<BehaviorTree>> typePoolDic = null;
                if (!PoolDic.TryGetValue((int)behaviorTreeType, out typePoolDic))
                    break;

                if (!typePoolDic.TryGetValue(agentId, out queue))
                    break;
            }
            while (false);

            BehaviorTree behaviorTree = null;

            if (queue != null && queue.Count > 0)
            {
                while (behaviorTree == null && queue.Count > 0)
                    behaviorTree = queue.Dequeue();
            }

            if (behaviorTree == null)
            {
                behaviorTree = new BehaviorTree();
                AgentData agentData = GetAgentData(behaviorTreeType, agentId);
                behaviorTree.SetData(agentData);
                behaviorTree.StartNode = CreateNode(agentData.StartNode);
                behaviorTree.BehaviorTreeType = behaviorTreeType;
            }

            return behaviorTree;
        }

        /// <summary>
        /// 回收到缓存池
        /// </summary>
        /// <param name="behaviorTree">回收的行为树</param>
        public void Despawn(BehaviorTree behaviorTree)
        {
            if (behaviorTree == null)
            {
                string msg = $"BehaviorTreeManager.Despawn() \n behaviorTree is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            int behaviorTreeType = behaviorTree.BehaviorTreeType;

            Dictionary<string, Queue<BehaviorTree>> typePoolDic = null;
            if (!PoolDic.TryGetValue(behaviorTreeType, out typePoolDic))
            {
                typePoolDic = new Dictionary<string, Queue<BehaviorTree>>();
                PoolDic.Add(behaviorTreeType, typePoolDic);
            }

            string agentId = behaviorTree.AgentID;
            Queue<BehaviorTree> queue = null;
            if (!typePoolDic.TryGetValue(agentId, out queue))
            {
                queue = new Queue<BehaviorTree>();
                typePoolDic.Add(agentId, queue);
            }

            if (queue.Contains(behaviorTree))
            {
                string msg = $"BehaviorTreeManager.Despawn() \n queue contains behaviorTree,agentId:{agentId}.";
                LogError(msg);
                throw new Exception(msg);
            }

            queue.Enqueue(behaviorTree);
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public void ClearPool()
        {
            PoolDic.Clear();
        }

        /// <summary>
        /// 清除指定类型的缓存数据
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        public void ClearPool(int behaviorTreeType)
        {
            Dictionary<string, Queue<BehaviorTree>> typePoolDic = null;
            if (!PoolDic.TryGetValue(behaviorTreeType, out typePoolDic))
                return;

            if (typePoolDic != null)
                typePoolDic.Clear();
        }

        #endregion

        /// <summary>
        /// 构建节点
        /// </summary>
        /// <param name="nodeData">节点数据</param>
        public BaseNode CreateNode(NodeData nodeData)
        {
            if (nodeData == null)
            {
                string msg = "BehaviorTreeManager.CreateNode() \n nodeData is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            ProxyData proxyData = null;

            //优先取lua的Proxy（如果C#端的proxy有bug,可以用lua的proxy修复）
            LuaProxyDic.TryGetValue(nodeData.ClassType, out proxyData);

            if (proxyData == null)
                CSharpProxyDic.TryGetValue(nodeData.ClassType, out proxyData);

            if (proxyData == null)
            {
                string msg = "BehaviorTreeManager.CreateNode() \n proxyData is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            BaseNode baseNode = null;

            switch (proxyData.NodeType)
            {
                case ENodeType.Action:
                    ActionNode actionNode = new ActionNode();
                    actionNode.NodeType = ENodeType.Action;
                    baseNode = actionNode;
                    break;
                case ENodeType.Composite:
                    CompositeNode compositeNode = new CompositeNode();
                    compositeNode.NodeType = ENodeType.Composite;
                    baseNode = compositeNode;
                    break;
                case ENodeType.Condition:
                    ConditionNode conditionNode = new ConditionNode();
                    conditionNode.NodeType = ENodeType.Condition;
                    baseNode = conditionNode;
                    break;
                case ENodeType.Decorator:
                    DecorateNode decorateNode = new DecorateNode();
                    decorateNode.NodeType = ENodeType.Condition;
                    baseNode = decorateNode;
                    break;
            }

            if (baseNode == null)
            {
                //组合节点必须有子节点
                string msg = $"CreateNode {proxyData.ClassType} Failed";
                LogError(msg);
                throw new Exception(msg);
            }

            baseNode.SetData(nodeData);
            baseNode.SetProxyData(proxyData);

            if (baseNode is CompositeNode)
            {
                if (nodeData.Childs == null || nodeData.Childs.Count == 0)
                {
                    //组合节点必须有子节点
                    string msg = $"{proxyData.NodeType} node must have child nodes";
                    LogError(msg);
                    throw new Exception(msg);
                }

                CompositeNode compositeNode = baseNode as CompositeNode;
                for (int i = 0; i < nodeData.Childs.Count; i++)
                {
                    NodeData childNodeData = nodeData.Childs[i];
                    BaseNode childNode = CreateNode(childNodeData);
                    compositeNode.AddChild(childNode);
                }
            }

            return baseNode;
        }

        public BaseNodeProxy CreateProxy(BaseNode node)
        {
            if (node == null)
            {
                //组合节点必须有子节点
                string msg = "Create nodeProxy failed,node is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            BaseNodeProxy nodeProxy = null;

            if (node.ProxyData.IsLuaProxy)
            {
                LuaNodeProxy luaNodeProxy = new LuaNodeProxy();
                luaNodeProxy.SetNode(node);
                luaNodeProxy.SetData(node.NodeData);
                luaNodeProxy.SetContext(node.Context);
                luaNodeProxy.CreateLuaObj();
                nodeProxy = luaNodeProxy;
            }
            else
            {
                nodeProxy = Activator.CreateInstance(node.ProxyData.ProxyType) as BaseNodeProxy;
                nodeProxy.SetNode(node);
                nodeProxy.SetData(node.NodeData);
                nodeProxy.SetContext(node.Context);
            }

            if (nodeProxy == null)
            {
                //组合节点必须有子节点
                string msg = $"Create nodeProxy failed,ClassType:{node.ProxyData.ClassType}";
                LogError(msg);
                throw new Exception(msg);
            }

            return nodeProxy;
        }

        public void OnUpdate(float deltatime)
        {
            if (Runnings.Count > 0)
            {
                for (int i = 0; i < Runnings.Count; i++)
                {
                    BehaviorTree behaviorTree = Runnings[i];

                    if (behaviorTree == null)
                        continue;
                    if (behaviorTree.Status == ENodeStatus.Error)
                        continue;

                    behaviorTree.Run(deltatime);

                    //if (behaviorTree.Status == ENodeStatus.Failed || behaviorTree.Status == ENodeStatus.Succeed)
                    //{
                    //    Runnings.RemoveAt(i);
                    //    i--;
                    //    behaviorTree.Destroy();
                    //    Despawn(behaviorTree);
                    //}
                }
            }
        }

        #region Log

        internal void Log(string msg, params string[] args)
        {
            if (args != null && args.Length > 0)
                msg = string.Format(msg, args);

            Log(ELogType.Info, msg);
        }

        internal void LogWarnning(string msg, params string[] args)
        {
            if (args != null && args.Length > 0)
                msg = string.Format(msg, args);

            Log(ELogType.Warnning, msg);
        }

        internal void LogError(string msg, params string[] args)
        {
            if (args != null && args.Length > 0)
                msg = string.Format(msg, args);

            Log(ELogType.Error, msg);
        }

        private void Log(ELogType logType, string msg)
        {
            LogHandler?.Invoke(logType, msg);
        }

        #endregion
    }
}