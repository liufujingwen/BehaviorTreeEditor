using System;
using BTData;
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
        public readonly Dictionary<int, BehaviorTreeData> TreeDataDic = new Dictionary<int, BehaviorTreeData>();

        /// <summary>
        /// Proxy管理器
        /// </summary>
        private readonly List<IProxyManager> m_ProxyManagers = new List<IProxyManager>();

        /// <summary>
        /// 行为树缓存池
        /// </summary>
        public readonly Dictionary<int, Dictionary<string, Queue<BehaviorTree>>> m_PoolDic = new Dictionary<int, Dictionary<string, Queue<BehaviorTree>>>();

        public void AddProxyManager(IProxyManager proxyManager)
        {
            if (proxyManager == null)
            {
                string msg = "BehaviorTreeManager.AddProxyManager() \n proxyManager is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            EProxyType proxyType = proxyManager.GetProxyType();

            if (m_ProxyManagers.Count == 0)
            {
                m_ProxyManagers.Add(proxyManager);
            }
            else
            {
                //插入排序，EProxyType的枚举值越大，优先级越高
                for (int i = 0; i < m_ProxyManagers.Count; i++)
                {
                    IProxyManager tempProxyManager = m_ProxyManagers[i];
                    EProxyType tempProxyType = tempProxyManager.GetProxyType();
                    if (tempProxyType < proxyType)
                    {
                        m_ProxyManagers.Insert(i, proxyManager);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 加载指定类型的行为树
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        public void LoadBehaviorData(int behaviorTreeType, byte[] bytes)
        {
            BehaviorTreeData treeData = Serializer.DeSerialize<BehaviorTreeData>(bytes);
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
        public BehaviorTreeElement GetAgentData(int behaviorTreeType, string id)
        {
            BehaviorTreeData treeData = null;

            if (!TreeDataDic.TryGetValue(behaviorTreeType, out treeData))
                throw new Exception($"There is no treeData,BehaviorTreeType:{behaviorTreeType}.");

            if (treeData == null)
                return null;

            for (int i = 0; i < treeData.BehaviorTrees.Count; i++)
            {
                BehaviorTreeElement behaviorTreeElement = treeData.BehaviorTrees[i];
                if (behaviorTreeElement != null && behaviorTreeElement.ID == id)
                    return behaviorTreeElement;
            }

            string msg = $"There is no AgentData,BehaviorTreeType:{behaviorTreeType} id:{id}.";
            LogError(msg);
            throw new Exception(msg);
        }

        /// <summary>
        /// 创建行为树
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        /// <param name="behaviorTreeId">行为树ID</param>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public BehaviorTree CreateBehaviorTree(int behaviorTreeType, string behaviorTreeId, IContext context)
        {
            BehaviorTree behaviorTree = Spawn(behaviorTreeType, behaviorTreeId);

            if (behaviorTree == null)
            {
                string msg = $"BehaviorTreeManager.CreateBehaviorTree() \n Create failed, EBehaviorTreeType:{behaviorTreeType} AgentId:{behaviorTreeId}.";
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
        /// <param name="behaviorTreeId">行为树ID</param>
        /// <returns>行为树</returns>
        private BehaviorTree Spawn(int behaviorTreeType, string behaviorTreeId)
        {
            Queue<BehaviorTree> queue = null;

            do
            {
                Dictionary<string, Queue<BehaviorTree>> typePoolDic = null;
                if (!m_PoolDic.TryGetValue((int)behaviorTreeType, out typePoolDic))
                    break;

                if (!typePoolDic.TryGetValue(behaviorTreeId, out queue))
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
                BehaviorTreeElement behaviorTreeElement = GetAgentData(behaviorTreeType, behaviorTreeId);
                behaviorTree.SetData(behaviorTreeElement);
                behaviorTree.StartNode = CreateNode(behaviorTreeElement.StartNode);
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
            if (!m_PoolDic.TryGetValue(behaviorTreeType, out typePoolDic))
            {
                typePoolDic = new Dictionary<string, Queue<BehaviorTree>>();
                m_PoolDic.Add(behaviorTreeType, typePoolDic);
            }

            string behaviorTreeId = behaviorTree.BehaviorTreeID;
            Queue<BehaviorTree> queue = null;
            if (!typePoolDic.TryGetValue(behaviorTreeId, out queue))
            {
                queue = new Queue<BehaviorTree>();
                typePoolDic.Add(behaviorTreeId, queue);
            }

            if (queue.Contains(behaviorTree))
            {
                string msg = $"BehaviorTreeManager.Despawn() \n queue contains behaviorTree,behaviorTreeId:{behaviorTreeId}.";
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
            m_PoolDic.Clear();
        }

        /// <summary>
        /// 清除指定类型的缓存数据
        /// </summary>
        /// <param name="behaviorTreeType">行为树类型</param>
        public void ClearPool(int behaviorTreeType)
        {
            Dictionary<string, Queue<BehaviorTree>> typePoolDic = null;
            if (!m_PoolDic.TryGetValue(behaviorTreeType, out typePoolDic))
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

            for (int i = 0; i < m_ProxyManagers.Count; i++)
            {
                IProxyManager proxyManager = m_ProxyManagers[i];
                proxyData = proxyManager.GetProxyData(nodeData.ClassType);
                if (proxyData != null)
                    break;
            }

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
                string msg = "BehaviorTreeManager.CreateProxy() \n Create nodeProxy failed,node is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            IProxyManager proxyManager = null;
            for (int i = 0; i < m_ProxyManagers.Count; i++)
            {
                IProxyManager tempProxyManager = m_ProxyManagers[i];
                ProxyData proxyData = tempProxyManager.GetProxyData(node.ClassType);
                if (proxyData != null)
                {
                    proxyManager = tempProxyManager;
                    break;
                }
            }

            if (proxyManager == null)
            {
                string msg = $"BehaviorTreeManager.CreateProxy() \n Create nodeProxy failed,proxyManager is null.";
                LogError(msg);
                throw new Exception(msg);
            }

            BaseNodeProxy nodeProxy = proxyManager.CreateProxy();

            if (nodeProxy == null)
            {
                string msg = $"BehaviorTreeManager.CreateProxy() \n Create nodeProxy failed,ClassType:{node.ProxyData.ClassType}";
                LogError(msg);
                throw new Exception(msg);
            }

            nodeProxy.BeginInit();
            nodeProxy.SetNode(node);
            nodeProxy.SetData(node.NodeData);
            nodeProxy.SetContext(node.Context);
            nodeProxy.EndInit();

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