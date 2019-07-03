using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace BehaviorTreeEditor.UIControls
{
    public partial class ContentUserControl : UserControl
    {
        private static ContentUserControl ms_Instance;
        public static ContentUserControl Instance
        {
            get { return ms_Instance; }
        }

        private ZoomScalerUserControl m_ZoomScalerUserControl;
        private const int GridMinorSize = 12;
        private const int GridMajorSize = 120;
        private Graphics m_Graphics = null;
        private BufferedGraphicsContext m_BufferedGraphicsContext = null;
        private BufferedGraphics m_BufferedGraphics = null;

        //偏移
        private Vec2 m_Offset = Vec2.zero;
        //原来视窗大小，没有经过缩放的大小
        private Rect m_ViewSize;
        //缩放后的视窗大小
        private Rect m_ScaledViewSize;
        //滑动起始位置
        private Vec2 m_ScrollPosition;

        //当前缩放
        private float m_ZoomScale = 1f;

        //帧时间
        private float m_Deltaltime;
        //当前渲染时间
        private DateTime m_DrawTime;
        //PFS
        private int m_Fps = 0;

        //记录上一次鼠标位置(坐标系为Local)
        private Vec2 m_MouseLocalPoint;
        //记录上一次鼠标位置(坐标系为World)
        private Vec2 m_MouseWorldPoint;
        //鼠标移动偏移量
        private Vec2 m_Deltal;
        //鼠标是否按下
        private bool m_MouseDown = false;
        //是否按下鼠标滚轮
        private bool m_MouseMiddle = false;
        //是否按下左Ctrl键
        private bool m_LControlKeyDown = false;

        private SelectionMode m_SelectionMode;
        private Vec2 m_SelectionStartPosition;
        private NodeDesigner m_FromNode;


        private Transition m_SelectedTransition;
        private List<NodeDesigner> m_SelectionNodes = new List<NodeDesigner>();

        private AgentDesigner Agent
        {
            get { return MainForm.Instance.SelectedAgent; }
        }

        public enum SelectionMode
        {
            None,
            Pick,
            Rect,
        }

        public ContentUserControl()
        {
            ms_Instance = this;
            InitializeComponent();
        }

        /// <summary>
        /// 设置选中Agent
        /// </summary>
        /// <param name="agent"></param>
        public void SetSelectedAgent(AgentDesigner agent)
        {
            m_SelectionNodes.Clear();
            m_SelectedTransition = null;

            if (Agent != null)
            {
                if (Agent.Nodes.Count > 0)
                {
                    for (int i = 0; i < Agent.Nodes.Count; i++)
                    {
                        NodeDesigner node = Agent.Nodes[i];
                        if (node.Transitions.Count > 0)
                        {
                            for (int j = 0; j < node.Transitions.Count; j++)
                            {
                                Transition transition = node.Transitions[j];
                                NodeDesigner fromNode = Agent.FindNodeByID(transition.FromNodeID);
                                NodeDesigner toNode = Agent.FindNodeByID(transition.ToNodeID);
                                transition.Set(toNode, fromNode);
                            }
                        }
                    }
                    CenterView();
                }
            }
        }

        public static Vec2 Center
        {
            get
            {
                return new Vec2(5000, 5000);
            }
        }

        private void ContentUserControl_Load(object sender, EventArgs e)
        {
            m_ZoomScalerUserControl = new ZoomScalerUserControl(EditorUtility.ZoomScaleMin, EditorUtility.ZoomScaleMax);
            m_ZoomScalerUserControl.SetVisible(false);
            this.Controls.Add(m_ZoomScalerUserControl);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.MouseWheel += ContentUserControl_MouseWheel;

            UpdateRect();
            CenterView();

            m_DrawTime = DateTime.Now;
            m_Graphics = this.CreateGraphics();
            refreshTimer.Start();
        }

        //定时器控制刷新频率
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void ContentUserControl_Paint(object sender, PaintEventArgs e)
        {
            Begin(sender, e);
            End(sender, e);
        }

        //刷新Rect
        private void UpdateRect()
        {
            //获取视窗大小
            var a = new Vec2(this.Width, this.Height);
            m_ViewSize = new Rect(0, 0, this.Width, this.Height);
            float scale = 1f / m_ZoomScale;
            m_ScaledViewSize = new Rect(m_ViewSize.x * scale, m_ViewSize.y * scale, m_ViewSize.width * scale, m_ViewSize.height * scale);
        }

        private void UpdateMousePoint(object sender, MouseEventArgs e)
        {
            Vec2 currentMousePoint = (Vec2)e.Location;
            m_Deltal = (currentMousePoint - m_MouseLocalPoint) / m_ZoomScale;
            m_MouseLocalPoint = currentMousePoint;
            m_MouseWorldPoint = LocalToWorldPoint(m_MouseLocalPoint);

        }

        private void Begin(object sender, PaintEventArgs e)
        {
            //计算帧时间、fps
            m_Deltaltime = (float)(DateTime.Now - m_DrawTime).TotalSeconds;
            m_Fps = (int)(1 / m_Deltaltime);
            m_DrawTime = DateTime.Now;

            //刷新Rect
            UpdateRect();

            m_BufferedGraphicsContext = BufferedGraphicsManager.Current;
            m_BufferedGraphics = m_BufferedGraphicsContext.Allocate(e.Graphics, e.ClipRectangle);
            m_Graphics = m_BufferedGraphics.Graphics;
            m_Graphics.SmoothingMode = SmoothingMode.HighQuality;
            m_Graphics.Clear(this.BackColor);
            Matrix matrix = m_Graphics.Transform;
            matrix.Scale(m_ZoomScale, m_ZoomScale);
            m_Graphics.Transform = matrix;

            //画格子线
            DrawGrid();
            //绘制节点
            DoNodes();
            //Render
            m_BufferedGraphics.Render();
        }

        private void End(object sender, PaintEventArgs e)
        {

        }

        //更新偏移坐标
        protected void UpdateOffset(Vec2 position)
        {
            m_Offset = m_Offset - (m_ScrollPosition - position);
            m_ScrollPosition = position;
        }

        /// <summary>
        /// 画背景格子
        /// </summary>
        private void DrawGrid()
        {
            EditorUtility.DrawGridLines(m_Graphics, m_ScaledViewSize, GridMinorSize, m_Offset, true);
            EditorUtility.DrawGridLines(m_Graphics, m_ScaledViewSize, GridMajorSize, m_Offset, false);
        }

        private void DoNodes()
        {
            if (Agent == null)
                return;

            DoTransitions();

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                if (node == null)
                    continue;
                EditorUtility.Draw(node, m_Graphics, m_Offset, false);
            }

            for (int i = 0; i < m_SelectionNodes.Count; i++)
            {
                NodeDesigner node = m_SelectionNodes[i];
                if (node == null)
                    continue;
                EditorUtility.Draw(node, m_Graphics, m_Offset, true);
            }

            DrawSelectionRect();
            AutoPanNodes(3.0f);
        }

        //画节点连线
        private void DoTransitions()
        {
            if (m_FromNode != null)
            {
                BezierLink.DrawNodeToPoint(m_Graphics, m_FromNode, m_MouseLocalPoint / m_ZoomScale, m_Offset);
            }

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node_i = Agent.Nodes[i];
                if (node_i == null)
                    continue;

                if (node_i.Transitions == null || node_i.Transitions.Count == 0)
                    continue;

                for (int ii = 0; ii < node_i.Transitions.Count; ii++)
                {
                    Transition node_ii = node_i.Transitions[ii];
                    if (node_ii == null)
                        continue;

                    BezierLink.DrawNodeToNode(m_Graphics, node_ii.FromNode, node_ii.ToNode, false, m_Offset);
                }
            }

            //绘制选中线
            if (m_SelectedTransition != null)
            {
                BezierLink.DrawNodeToNode(m_Graphics, m_SelectedTransition.FromNode, m_SelectedTransition.ToNode, true, m_Offset);
            }
        }

        public void SelectTransition(Transition transition)
        {
            if (m_SelectedTransition == transition)
            {
                return;
            }
            m_SelectedTransition = transition;
        }

        private void AddTransition(NodeDesigner fromNode, NodeDesigner toNode)
        {
            if (fromNode == null || toNode == null)
                return;

            if (toNode.StartNode)
                return;

            //子节点不能指向父节点
            if (fromNode.ParentNode == toNode)
                return;

            //节点只有拥有一个父节点
            if (toNode.ParentNode != null)
                return;

            if (fromNode.NodeType == NodeType.Action)
            {
                MainForm.Instance.ShowMessage("行动节点不能包含子节点");
                return;
            }

            if (fromNode.NodeType == NodeType.Decorator)
            {
                if (fromNode.Transitions.Count == 1)
                {
                    MainForm.Instance.ShowMessage("修饰节点最多只能包含一个子节点");
                    return;
                }
            }

            if (fromNode.NodeType == NodeType.Composite)
            {
                //检测toNode节点是否已经添加了
                for (int i = 0; i < fromNode.Transitions.Count; i++)
                {
                    Transition transition = fromNode.Transitions[i];
                    if (transition != null)
                    {
                        if (transition.ToNode == toNode)
                        {
                            return;
                        }
                    }
                }

                //子节点不能指向父节点
                if (toNode.NodeType == NodeType.Composite)
                {
                    if (toNode.Transitions.Count > 0)
                    {
                        for (int i = 0; i < toNode.Transitions.Count; i++)
                        {
                            Transition tansition = toNode.Transitions[i];
                            if (tansition != null)
                            {
                                if (tansition.ToNode == fromNode)
                                    return;
                            }
                        }
                    }
                }

                fromNode.AddNode(toNode);
            }
        }

        #region Winform Event

        // 鼠标滚轮事件
        private void ContentUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (nodeContextMenuStrip.Visible || viewContextMenuStrip.Visible || transitionContextMenuStrip.Visible)
                return;

            m_ZoomScalerUserControl.SetVisible(true);
            m_ZoomScale += e.Delta * 0.0003f;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale, 0.5f, 2.0f);
            UpdateRect();
            Vec2 offset = (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f;
            UpdateOffset(m_ScrollPosition - (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f + offset);
            m_ZoomScalerUserControl.SetZoomScale(m_ZoomScale);
            this.Refresh();
        }

        //鼠标按下事件
        private void ContentUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            UpdateMousePoint(sender, e);

            //处理选择节点
            if (e.Button == MouseButtons.Left)
            {
                //标记鼠标按下
                m_MouseDown = true;
                m_SelectionStartPosition = m_MouseWorldPoint;
                NodeDesigner node = MouseOverNode();
                Transition transition = MouseOverTransition();

                //选中线，开始节点需要改为选中状态
                if (transition != null && node == null)
                {
                    this.m_SelectionNodes.Clear();
                    NodePropertyUserControl.Instance.SetSelectedNode(null);
                    //this.m_SelectionNodes.Add(transition.FromNode);
                    SelectTransition(transition);
                    return;
                }

                if (node != null)
                {
                    SelectTransition(null);

                    if (m_FromNode != null)
                    {
                        AddTransition(m_FromNode, node);
                        m_FromNode = null;
                        return;
                    }

                    if (m_LControlKeyDown)
                    {
                        //如果按住Control键就变成反选，并且可以多选
                        if (!m_SelectionNodes.Contains(node))
                        {
                            m_SelectionNodes.Add(node);
                        }
                        else
                        {
                            m_SelectionNodes.Remove(node);
                        }
                    }
                    else if (!m_SelectionNodes.Contains(node))
                    {
                        m_SelectionNodes.Clear();
                        m_SelectionNodes.Add(node);
                        NodePropertyUserControl.Instance.SetSelectedNode(node);
                    }

                    return;
                }
                else
                {
                    m_SelectionNodes.Clear();
                }

                m_FromNode = null;
                m_SelectionMode = SelectionMode.Pick;
                SelectTransition(null);
                NodePropertyUserControl.Instance.SetSelectedNode(null);
            }
            //点击鼠标滚轮
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                m_MouseMiddle = true;
            }
            //点击鼠标右键
            else if (e.Button == MouseButtons.Right)
            {
                if (m_LControlKeyDown)
                    return;

                NodeDesigner node = MouseOverNode();
                Transition transition = MouseOverTransition();

                if (node != null)
                {
                    if (!m_SelectionNodes.Contains(node))
                    {
                        m_SelectionNodes.Clear();
                        m_SelectionNodes.Add(node);
                        SelectTransition(null);
                    }
                    else
                    {
                        ShowNodeContextMenu();
                    }
                }
                else
                {
                    if (m_SelectionNodes.Count > 1)
                    {
                        ShowNodeContextMenu();
                    }
                    else if (transition != null)
                    {
                        m_SelectionNodes.Clear();
                        if (m_SelectedTransition == transition)
                        {
                            ShowTransitionContextMenu();
                        }
                        else
                        {
                            SelectTransition(transition);
                        }
                    }
                    else if (m_SelectedTransition != null)
                    {
                        ShowTransitionContextMenu();
                    }
                    else
                    {
                        m_SelectionNodes.Clear();
                        ShowViewContextMenu();
                    }
                }
            }
        }

        //鼠标弹起事件
        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateMousePoint(sender, e);

            //标记鼠标弹起
            m_MouseDown = false;
            m_MouseMiddle = false;
            m_SelectionMode = SelectionMode.None;

            //排序
            if (m_SelectionNodes.Count > 0)
            {
                for (int i = 0; i < m_SelectionNodes.Count; i++)
                {
                    NodeDesigner parentNode = m_SelectionNodes[i].ParentNode;
                    if (parentNode != null)
                        parentNode.Sort();
                }
            }

            this.Refresh();
        }

        //鼠标移动事件
        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateMousePoint(sender, e);

            //按下鼠标左键且没有按Ctrl键
            if (MouseButtons == MouseButtons.Left && m_MouseDown)
            {
                //按下Ctrl键不让拖拽
                if (m_LControlKeyDown)
                {
                    return;
                }

                //框选节点
                if (m_SelectionMode == SelectionMode.Pick || m_SelectionMode == SelectionMode.Rect)
                {
                    m_SelectionMode = SelectionMode.Rect;
                    SelectNodesInRect(FromToRect(m_SelectionStartPosition, m_MouseWorldPoint));
                }
                else
                {
                    //拖拽选中节点
                    if (m_SelectionNodes.Count > 0)
                    {
                        for (int i = 0; i < m_SelectionNodes.Count; i++)
                        {
                            NodeDesigner node = m_SelectionNodes[i];
                            if (node == null)
                                continue;
                            node.Rect += m_Deltal;
                        }
                    }
                }

                this.Refresh();
            }
            //按下鼠标滚轮移动View
            else if (m_MouseMiddle)
            {
                DragView();
                this.Refresh();
            }
        }

        //按键按下事件
        private void ContentUserControl_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown：" + e.KeyCode);
            if (!e.Alt && !e.Shift && e.KeyCode == Keys.ControlKey)
            {
                m_LControlKeyDown = true;
            }
            else
            {
                m_LControlKeyDown = false;
            }

        }

        //释放按键
        private void ContentUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                m_LControlKeyDown = false;
            }
        }

        //控件大小改变通知事件
        private void ContentUserControl_Resize(object sender, EventArgs e)
        {
            if (m_ZoomScalerUserControl == null)
                return;
            m_ZoomScalerUserControl.Location = new Point(Width / 2 - m_ZoomScalerUserControl.Width / 2 + 2, Height - m_ZoomScalerUserControl.Height - 2);
            m_ZoomScalerUserControl.SetZoomScale(m_ZoomScale);
            UpdateRect();
        }

        //视图失焦
        private void ContentUserControl_Leave(object sender, EventArgs e)
        {
            m_LControlKeyDown = false;
        }

        //视图激活
        private void ContentUserControl_Enter(object sender, EventArgs e)
        {
            m_LControlKeyDown = false;
        }

        #endregion

        #region Content Menu Event

        //删除节点
        private void 连线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_FromNode = m_SelectionNodes[0];
        }

        //删除节点
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SelectionNodes.Count == 0)
                return;

            for (int i = 0; i < m_SelectionNodes.Count; i++)
            {
                NodeDesigner node = m_SelectionNodes[i];
                if (node == null)
                    continue;
                Agent.RemoveNode(node);
            }
            m_SelectionNodes.Clear();
        }

        //标记为开始
        private void 标记为开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SelectionNodes.Count != 1)
                return;

            NodeDesigner node = m_SelectionNodes[0];
            if (node == null)
                return;

            if (node.NodeType != NodeType.Composite)
            {
                MainForm.Instance.ShowMessage(string.Format("{0}不能标记为开始节点,请选择组合节点作为开始节点", node.ClassType));
                return;
            }

            Agent.ChangeStartNode(node);
        }


        private void 删除连线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SelectedTransition == null)
                return;

            Agent.RemoveTranstion(m_SelectedTransition);
            SelectTransition(null);
        }

        //居中
        private void centerItem_Click(object sender, EventArgs e)
        {
            CenterView();
        }

        #endregion

        //当前鼠标悬停节点
        private NodeDesigner MouseOverNode()
        {
            if (Agent == null)
                return null;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                if (node.Rect.Contains(m_MouseWorldPoint))
                {
                    return node;
                }
            }
            return null;
        }

        private Transition MouseOverTransition()
        {
            if (Agent == null)
                return null;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = (NodeDesigner)Agent.Nodes[i];
                if (node.Transitions.Count > 0)
                {
                    for (int ii = 0; ii < node.Transitions.Count; ii++)
                    {
                        Transition transition = node.Transitions[ii];
                        if (transition != null)
                        {
                            if (BezierLink.CheckPointAt(transition.FromNode, transition.ToNode, m_MouseLocalPoint / m_ZoomScale, m_Offset))
                            {
                                return transition;
                            }
                        }
                    }
                }
            }

            return null;
        }


        //绘制框选矩形
        private void DrawSelectionRect()
        {
            if (m_SelectionMode != SelectionMode.Rect)
                return;

            Vec2 tmpStartLocalPoint = WorldToLocalPoint(m_SelectionStartPosition) / m_ZoomScale;
            Vec2 tmpEndLocalPoint = WorldToLocalPoint(m_MouseWorldPoint) / m_ZoomScale;
            Rect rect = FromToRect(tmpStartLocalPoint, tmpEndLocalPoint);
            m_Graphics.DrawRectangle(EditorUtility.SelectionModePen, rect);
            m_Graphics.FillRectangle(EditorUtility.SelectionModeBrush, rect);

        }

        private void AutoPanNodes(float speed)
        {
            if (!m_MouseDown)
                return;

            if (m_SelectionMode != SelectionMode.None)
                return;

            if (m_SelectionNodes.Count == 0)
                return;

            Vec2 delta = Vec2.zero;

            if (m_MouseLocalPoint.x > m_ViewSize.width - 50)
            {
                delta.x += speed;
            }

            if ((m_MouseLocalPoint.x < m_ViewSize.x + 50))
            {
                delta.x -= speed;
            }

            if (m_MouseLocalPoint.y > m_ViewSize.height - 50f)
            {
                delta.y += speed;
            }

            if ((m_MouseLocalPoint.y < m_ViewSize.y + 50f))
            {
                delta.y -= speed;
            }

            if (delta != Vec2.zero)
            {
                delta /= m_ZoomScale;
                for (int i = 0; i < m_SelectionNodes.Count; i++)
                {
                    NodeDesigner node = m_SelectionNodes[i];
                    if (node == null)
                        continue;
                    node.Rect += delta;
                }
                UpdateOffset(m_ScrollPosition + delta);
            }
        }

        /// <summary>
        /// 获取矩形范围
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="end">结束点</param>
        /// <returns></returns>
        private Rect FromToRect(Vec2 start, Vec2 end)
        {
            Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
            if (rect.width < 0f)
            {
                rect.x = rect.x + rect.width;
                rect.width = -rect.width;
            }
            if (rect.height < 0f)
            {
                rect.y = rect.y + rect.height;
                rect.height = -rect.height;
            }
            return rect;
        }

        /// <summary>
        /// 选择在指定范围内的节点
        /// </summary>
        /// <param name="r"></param>
        private void SelectNodesInRect(Rect r)
        {
            if (Agent == null)
                return;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                Rect rect = node.Rect;
                if (rect.xMax < r.x || rect.x > r.xMax || rect.yMax < r.y || rect.y > r.yMax)
                {
                    m_SelectionNodes.Remove(node);
                    continue;
                }
                if (!m_SelectionNodes.Contains(node))
                {
                    m_SelectionNodes.Add(node);
                }
            }
        }

        //视图居中
        public void CenterView()
        {
            Vec2 center = Vec2.zero;
            if (Agent != null && Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    center += new Vec2(node.Rect.center.x - m_ScaledViewSize.width * 0.5f, node.Rect.center.y - m_ScaledViewSize.height * 0.5f);
                }
                center /= Agent.Nodes.Count;
            }
            else
            {
                center = Center;
            }
            UpdateOffset(center);
        }

        //按下鼠标滚轮键拖动视图
        private void DragView()
        {
            if (!m_MouseMiddle)
                return;

            UpdateOffset(m_ScrollPosition - m_Deltal);
        }

        /// 显示节点菜单上下文
        private void ShowNodeContextMenu()
        {
            if (m_SelectionNodes.Count == 0)
                return;

            for (int i = 0; i < nodeContextMenuStrip.Items.Count; i++)
            {
                nodeContextMenuStrip.Items[i].Visible = true;
            }

            // 0 连线
            // 1 删除
            // 2 标记为开始
            // 3 复制

            //隐藏上移、下移、连线
            if (m_SelectionNodes.Count > 1)
            {
                nodeContextMenuStrip.Items[0].Visible = false;
                nodeContextMenuStrip.Items[2].Visible = false;
            }

            nodeContextMenuStrip.Show(PointToScreen(m_MouseLocalPoint));
            this.Refresh();
        }

        //显示连线菜单上下文
        private void ShowTransitionContextMenu()
        {
            if (m_SelectedTransition == null)
                return;

            transitionContextMenuStrip.Show(PointToScreen(m_MouseLocalPoint));
            this.Refresh();
        }

        //显示视图菜单上下文
        private void ShowViewContextMenu()
        {
            if (Agent == null)
                return;

            if (m_SelectionNodes.Count > 0)
                return;

            if (MainForm.Instance.NodeClasses == null)
                return;

            NodeClasses nodeClass = MainForm.Instance.NodeClasses;
            List<NodeClass> nodes = nodeClass.Nodes;

            viewContextMenuStrip.Items.Clear();

            ToolStripDropDownItem compositeItem = viewContextMenuStrip.Items.Add("组合节点") as ToolStripDropDownItem;
            ToolStripDropDownItem decoratorItem = viewContextMenuStrip.Items.Add("修饰节点") as ToolStripDropDownItem;
            ToolStripDropDownItem conditionItem = viewContextMenuStrip.Items.Add("条件节点") as ToolStripDropDownItem;
            ToolStripDropDownItem actionItem = viewContextMenuStrip.Items.Add("动作节点") as ToolStripDropDownItem;

            //绑定组合节点
            List<NodeClass> compositeList = nodeClass.GetClasses(NodeType.Composite);
            for (int i = 0; i < compositeList.Count; i++)
            {
                NodeClass node = compositeList[i];
                ToolStripItem nodeItem = compositeItem.DropDownItems.Add(node.ClassType);
                nodeItem.Tag = node;
                nodeItem.Click += new EventHandler(OnClickNodeItem);
            }

            //绑定修饰节点
            List<NodeClass> decoratorList = nodeClass.GetClasses(NodeType.Decorator);
            for (int i = 0; i < decoratorList.Count; i++)
            {
                NodeClass node = decoratorList[i];
                ToolStripItem nodeItem = decoratorItem.DropDownItems.Add(node.ClassType);
                nodeItem.Tag = node;
                nodeItem.Click += new EventHandler(OnClickNodeItem);
            }

            //绑定条件节点
            List<NodeClass> conditionList = nodeClass.GetClasses(NodeType.Condition);
            for (int i = 0; i < conditionList.Count; i++)
            {
                NodeClass node = conditionList[i];
                ToolStripItem nodeItem = conditionItem.DropDownItems.Add(node.ClassType);
                nodeItem.Tag = node;
                nodeItem.Click += new EventHandler(OnClickNodeItem);
            }

            //绑定动作节点
            List<NodeClass> actionList = nodeClass.GetClasses(NodeType.Action);
            for (int i = 0; i < actionList.Count; i++)
            {
                NodeClass node = actionList[i];
                ToolStripItem nodeItem = actionItem.DropDownItems.Add(node.ClassType);
                nodeItem.Tag = node;
                nodeItem.Click += new EventHandler(OnClickNodeItem);
            }

            //添加分割线
            viewContextMenuStrip.Items.Add(new ToolStripSeparator());

            ToolStripItem centerItem = viewContextMenuStrip.Items.Add("居中");
            centerItem.Click += new EventHandler(centerItem_Click);

            viewContextMenuStrip.Show(PointToScreen(m_MouseLocalPoint));
            this.Refresh();
        }

        private void OnClickNodeItem(object sender, EventArgs e)
        {
            ToolStripDropDownItem toolStripDropDownItem = (ToolStripDropDownItem)sender;

            if (toolStripDropDownItem == null)
                return;

            NodeClass nodeClass = (NodeClass)toolStripDropDownItem.Tag;

            if (nodeClass == null)
                return;

            Rect rect = new Rect(m_MouseWorldPoint.x, m_MouseWorldPoint.y, EditorUtility.NodeWidth, EditorUtility.NodeHeight);
            NodeDesigner node = new NodeDesigner(nodeClass.ClassName, nodeClass.ClassType, rect);
            node.ID = Agent.GenNodeID();
            node.ClassName = nodeClass.ClassName;
            node.NodeType = nodeClass.NodeType;
            node.ClassType = nodeClass.ClassType;
            node.Describe = nodeClass.Describe;

            //创建字段
            for (int i = 0; i < nodeClass.Fields.Count; i++)
            {
                NodeField nodeField = nodeClass.Fields[i];
                FieldDesigner field = EditorUtility.CreateFieldByNodeField(nodeField);
                if (field == null)
                    continue;
                node.Fields.Add(field);
            }

            Agent.AddNode(node);
        }

        public Vec2 LocalToWorldPoint(Vec2 point)
        {
            return point / m_ZoomScale + m_Offset;
        }

        public Vec2 WorldToLocalPoint(Vec2 point)
        {
            return (point - m_Offset) * m_ZoomScale;
        }

        private void ContentUserControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        MainForm.Instance.Exec(OperationType.Save);
                        break;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (m_SelectionNodes.Count > 0)
                    {
                        for (int i = m_SelectionNodes.Count - 1; i >= 0; i--)
                        {
                            Agent.RemoveNode(m_SelectionNodes[i]);
                            m_SelectionNodes.RemoveAt(i);
                        }
                    }

                    if (m_SelectedTransition != null)
                    {
                        Agent.RemoveTranstion(m_SelectedTransition);
                        SelectTransition(null);
                    }
                }
            }
        }


    }
}