using BehaviorTreeData;

namespace R7BehaviorTree
{
    public abstract class BaseNode
    {
        public int ID { get; private set; }
        public NodeData NodeData { get; private set; }
        public BaseContext Context { get; private set; }
        public BaseNodeProxy NodeProxy { get; private set; }
        public ProxyData ProxyData { get; private set; }
        public ENodeStatus Status { get; set; }
        public string ClassType { get; private set; }
        public ENodeType NodeType { get; set; }
        public bool Active { get; set; }

        #region 数据相关

        internal virtual void SetData(NodeData data)
        {
            NodeData = data;
            ID = NodeData.ID;
            ClassType = data.ClassType;
        }

        internal virtual void SetContext(BaseContext context)
        {
            Context = context;
        }

        internal void SetProxyData(ProxyData proxyData)
        {
            ProxyData = proxyData;
        }

        internal virtual void CreateProxy()
        {
            NodeProxy = BehaviorTreeManager.Instance.CreateProxy(this);
        }

        #endregion

        #region 外部调用

        /// <summary>
        /// 生命周期驱动函数
        /// </summary>
        /// <param name="deltatime">帧时间</param>
        internal virtual void Run(float deltatime)
        {
            if (Status == ENodeStatus.Error)
            {
                return;
            }

            if (Status == ENodeStatus.None)
            {
                Status = ENodeStatus.Ready;
                OnAwake();
            }

            if (Status == ENodeStatus.Ready)
            {
                Status = ENodeStatus.Running;
                SetActive(true);
                OnStart();
            }

            if (Active && Status == ENodeStatus.Running)
            {
                OnUpdate(deltatime);
            }
        }

        internal virtual void SetActive(bool active)
        {
            if (Status < ENodeStatus.Running)
                return;

            if (Active == active)
                return;

            Active = active;

            if (active)
                OnEnable();
            else
                OnDisable();
        }

        internal virtual void Reset()
        {
            if (Status < ENodeStatus.Running)
                return;

            SetActive(false);
            Status = ENodeStatus.Ready;
            OnReset();
        }

        internal virtual void Destroy()
        {
            if (Status < ENodeStatus.Ready)
                return;

            SetActive(false);
            OnDestroy();
            Status = ENodeStatus.None;
            Context = null;
            NodeProxy = null;
        }

        #endregion

        #region 生命周期函数

        /// <summary>
        /// 数据初始化
        /// </summary>
        internal virtual void OnAwake()
        {
            NodeProxy?.OnAwake();
        }

        /// <summary>
        /// 节点激活
        /// </summary>
        internal virtual void OnEnable()
        {
            NodeProxy?.OnEnable();
        }

        /// <summary>
        /// 节点暂停
        /// </summary>
        internal virtual void OnDisable()
        {
            NodeProxy?.OnDisable();
        }

        /// <summary>
        /// 节点进入时执行OnStart
        /// </summary>
        internal virtual void OnStart()
        {
            NodeProxy?.OnStart();
        }

        /// <summary>
        /// 状态NodeStatus=Running 时每帧执行OnUpdate
        /// </summary>
        internal virtual void OnUpdate(float deltatime)
        {
            NodeProxy?.OnUpdate(deltatime);
        }

        /// <summary>
        /// 节点重置执行OnReset
        /// </summary>
        internal virtual void OnReset()
        {
            NodeProxy?.OnReset();
        }

        /// <summary>
        /// 节点退出执行OnDestroy
        /// </summary>
        internal virtual void OnDestroy()
        {
            NodeProxy?.OnDestroy();
        }

        #endregion
    }
}