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

        NodeDesigner m_Node1;
        NodeDesigner m_Node2;

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

        private AgentDesigner m_Agent = new AgentDesigner();
        private Transition m_SelectedTransition;
        private List<NodeDesigner> m_SelectionNodes = new List<NodeDesigner>();

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
            //BezierLink.Draw(m_Graphics, m_Node1, m_Node2, Color.Blue, 2, m_Offset);

            DoTransitions();

            for (int i = 0; i < m_Agent.Nodes.Count; i++)
            {
                NodeDesigner node = m_Agent.Nodes[i];
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

            for (int i = 0; i < m_Agent.Nodes.Count; i++)
            {
                NodeDesigner node_i = m_Agent.Nodes[i];
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

            //子节点不能指向父节点
            if (fromNode.ParentNode == toNode)
                return;

            //节点只有拥有一个父节点
            if (toNode.ParentNode != null)
                return;

            //if (fromNode is BaseActionNodeDesigner)
            //{
            //    ShowNotification(FsmContent.actionNodeAddTips);
            //    return;
            //}

            //if (fromNode is BaseDecoratorNodeDesigner)
            //{
            //    BaseDecoratorNodeDesigner decoratorNodeDesigner = fromNode as BaseDecoratorNodeDesigner;
            //    if (decoratorNodeDesigner.transitionList.Count == 1)
            //    {
            //        ShowNotification(FsmContent.decoratorNodeAddTips);
            //        return;
            //    }
            //}

            //if (fromNode is BaseCompositeNodeDesigner)
            //{
            //    BaseCompositeNodeDesigner compositeNodeDesigner = fromNode as BaseCompositeNodeDesigner;
            //    //检测toNode节点是否已经添加了
            //    for (int i = 0; i < compositeNodeDesigner.transitionList.Count; i++)
            //    {
            //        Transition transition = compositeNodeDesigner.transitionList[i];
            //        if (transition != null)
            //        {
            //            if (transition.toNode == toNode)
            //                return;
            //        }
            //    }

            //    //子节点不能指向父节点
            //    if (toNode is BaseCompositeNodeDesigner)
            //    {
            //        BaseCompositeNodeDesigner tempNode = toNode as BaseCompositeNodeDesigner;
            //        if (tempNode.transitionList.Count > 0)
            //        {
            //            for (int i = 0; i < tempNode.transitionList.Count; i++)
            //            {
            //                Transition tansition = tempNode.transitionList[i];
            //                if (tansition != null)
            //                {
            //                    if (tansition.toNode == fromNode)
            //                        return;
            //                }
            //            }
            //        }
            //    }

            //    compositeNodeDesigner.AddNode(toNode);
            //}

            fromNode.AddNode(toNode);
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
                    }
                }

                if (m_SelectionNodes.Count > 0)
                {
                    ShowNodeContextMenu();
                }
                else if (m_SelectedTransition != null)
                {
                    ShowTransitionContextMenu();
                }
                else
                {
                    ShowViewContextMenu();
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
           

            Console.WriteLine("KeyUp：" + e.KeyCode);
        }

        //控件大小改变通知事件
        private void ContentUserControl_Resize(object sender, EventArgs e)
        {
            if (m_ZoomScalerUserControl == null)
                return;
            m_ZoomScalerUserControl.Location = new Point(Width / 2 - m_ZoomScalerUserControl.Width / 2 + 2, Height - m_ZoomScalerUserControl.Height - 2);
            m_ZoomScalerUserControl.SetZoomScale(m_ZoomScale);
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
                m_Agent.Remove(node);
            }
            m_SelectionNodes.Clear();
        }

        //添加节点
        private void addNodeItem_Click(object sender, EventArgs e)
        {
            NodeDesigner node = new NodeDesigner();
            node.ID = m_Agent.GenNodeID();
            node.Name = "测试节点";
            node.ClassType = "Sequence";
            node.Rect = new Rect(m_MouseWorldPoint.x, m_MouseWorldPoint.y, EditorUtility.NodeWidth, EditorUtility.NodeHeight);
            m_Agent.AddNode(node);
        }

        private void 删除连线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_SelectedTransition == null)
                return;

            m_Agent.RemoveTranstion(m_SelectedTransition);
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
            if (m_Agent == null)
                return null;

            for (int i = 0; i < m_Agent.Nodes.Count; i++)
            {
                NodeDesigner node = m_Agent.Nodes[i];
                if (node.Rect.Contains(m_MouseWorldPoint))
                {
                    return node;
                }
            }
            return null;
        }

        private Transition MouseOverTransition()
        {
            if (m_Agent == null)
                return null;

            for (int i = 0; i < m_Agent.Nodes.Count; i++)
            {
                NodeDesigner node = (NodeDesigner)m_Agent.Nodes[i];
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
            if (m_Agent == null)
                return;

            for (int i = 0; i < m_Agent.Nodes.Count; i++)
            {
                NodeDesigner node = m_Agent.Nodes[i];
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
            if (m_Agent != null && m_Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < m_Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = m_Agent.Nodes[i];
                    center += new Vec2(node.Rect.x - m_ScaledViewSize.width * 0.5f, node.Rect.center.y - m_ScaledViewSize.height * 0.5f);
                }
                center /= m_Agent.Nodes.Count;
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

            //隐藏上移、下移、连线
            if (m_SelectionNodes.Count > 1)
            {
                nodeContextMenuStrip.Items[0].Visible = false;
                nodeContextMenuStrip.Items[2].Visible = false;
                nodeContextMenuStrip.Items[3].Visible = false;
            }

            nodeContextMenuStrip.Show(PointToScreen(m_MouseLocalPoint));
            this.Refresh();
        }

        //显示视图菜单上下文
        private void ShowViewContextMenu()
        {
            if (m_SelectionNodes.Count > 0)
                return;

            viewContextMenuStrip.Show(PointToScreen(m_MouseLocalPoint));
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

        public Vec2 LocalToWorldPoint(Vec2 point)
        {
            return point / m_ZoomScale + m_Offset;
        }

        public Vec2 WorldToLocalPoint(Vec2 point)
        {
            return (point - m_Offset) * m_ZoomScale;
        }

        
    }
}