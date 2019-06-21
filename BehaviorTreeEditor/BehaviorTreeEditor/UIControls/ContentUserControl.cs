using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BehaviorTreeEditor.Properties;
using System.Drawing.Drawing2D;

namespace BehaviorTreeEditor.UIControls
{
    public partial class ContentUserControl : UserControl
    {
        private const int GridMinorSize = 12;
        private const int GridMajorSize = 120;
        private Graphics m_Graphics = null;
        private BufferedGraphicsContext m_BufferedGraphicsContext = null;
        private BufferedGraphics m_BufferedGraphics = null;

        //偏移
        private Vector2 m_Offset = Vector2.zero;
        //原来视窗大小，没有经过缩放的大小
        private Rect m_ViewSize;
        //缩放后的视窗大小
        private Rect m_ScaledViewSize;
        //滑动起始位置
        private Vector2 m_ScrollPosition;

        BaseNodeDesigner m_Node1;
        BaseNodeDesigner m_Node2;

        //当前缩放
        private float m_ZoomScale = 1f;

        //帧时间
        private float m_Deltaltime;
        //当前渲染时间
        private DateTime m_DrawTime;
        //PFS
        private int m_Fps = 0;

        //记录上一次鼠标位置(坐标系为Local)
        private Vector2 m_MouseLocalPoint;
        //记录上一次鼠标位置(坐标系为World)
        private Vector2 m_MouseWorldPoint;
        //鼠标移动偏移量
        private Vector2 m_Deltal;
        //鼠标是否按下
        private bool m_MouseDown = false;
        //是否按下鼠标滚轮
        private bool m_MouseMiddle = false;

        //是否按下左Ctrl键
        private bool m_LControlKeyDown = false;


        private SelectionMode m_SelectionMode;
        private Vector2 m_SelectionStartPosition;

        private List<BaseNodeDesigner> m_Nodes = new List<BaseNodeDesigner>();
        private List<BaseNodeDesigner> m_SelectionNodes = new List<BaseNodeDesigner>();

        public enum SelectionMode
        {
            None,
            Pick,
            Rect,
        }

        public ContentUserControl()
        {
            InitializeComponent();

        }

        public static Vector2 Center
        {
            get
            {
                return new Vector2(5000, 5000);
            }
        }

        private void ContentUserControl_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.MouseWheel += ContentUserControl_MouseWheel;

            //UpdateOffset(Center);

            m_Node1 = new BaseNodeDesigner("并行节点", new Rect(100 + m_Offset.x, 100 + m_Offset.y, 150, 100));
            m_Node2 = new BaseNodeDesigner("顺序节点", new Rect(400 + m_Offset.x, 100 + m_Offset.y, 150, 100));

            m_Nodes.Add(m_Node1);
            m_Nodes.Add(m_Node2);

            UpdateRect();
            CenterView();

            m_DrawTime = DateTime.Now;
            m_Graphics = this.CreateGraphics();
            timer1.Start();
        }

        //定时器控制刷新频率
        private void timer1_Tick(object sender, EventArgs e)
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
            var a = new Vector2(this.Width, this.Height);
            m_ViewSize = new Rect(0, 0, this.Width, this.Height);
            float scale = 1f / m_ZoomScale;
            m_ScaledViewSize = new Rect(m_ViewSize.x * scale, m_ViewSize.y * scale, m_ViewSize.width * scale, m_ViewSize.height * scale);
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
            m_Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
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
        protected void UpdateOffset(Vector2 position)
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
            BezierLink.Draw(m_Graphics, m_Node1, m_Node2, Color.Blue, 2, m_Offset);

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                BaseNodeDesigner node = m_Nodes[i];
                if (node == null)
                    continue;
                EditorUtility.Draw(node, m_Graphics, m_Offset, false);
            }

            for (int i = 0; i < m_SelectionNodes.Count; i++)
            {
                BaseNodeDesigner node = m_SelectionNodes[i];
                if (node == null)
                    continue;
                EditorUtility.Draw(node, m_Graphics, m_Offset, true);
            }

            DrawSelectionRect();
            AutoPanNodes(3.0f);
        }

        #region Winform Event

        // 鼠标滚轮事件
        private void ContentUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            m_ZoomScale += e.Delta * 0.0003f;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale, 0.5f, 2.0f);
            UpdateRect();
            Vector2 offset = (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f;
            UpdateOffset(m_ScrollPosition - (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f + offset);
        }

        //鼠标按下事件
        private void ContentUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            //处理选择节点
            if (e.Button == MouseButtons.Left)
            {
                //标记鼠标按下
                m_MouseDown = true;

                m_SelectionStartPosition = m_MouseWorldPoint;
                BaseNodeDesigner node = MouseOverNode();
                //Transition transition = MouseOverTransition();

                if (node != null)
                {
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

                m_SelectionMode = SelectionMode.Pick;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                m_MouseMiddle = true;
            }
        }

        //鼠标弹起事件
        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            //标记鼠标弹起
            m_MouseDown = false;
            m_MouseMiddle = false;
            m_SelectionMode = SelectionMode.None;
        }

        //鼠标移动事件
        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 currentMousePoint = (Vector2)e.Location;
            m_Deltal = (currentMousePoint - m_MouseLocalPoint) / m_ZoomScale;
            m_MouseLocalPoint = currentMousePoint;
            m_MouseWorldPoint = LocalToWorldPoint(m_MouseLocalPoint);
            //按下鼠标左键且
            if (MouseButtons == MouseButtons.Left && m_MouseDown)
            {
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
                            BaseNodeDesigner node = m_SelectionNodes[i];
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
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    m_LControlKeyDown = true;
                    break;
            }
        }

        //释放按键
        private void ContentUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    m_LControlKeyDown = false;
                    break;
            }

            Console.WriteLine("KeyUp：" + e.KeyCode);
        }

        #endregion

        //当前鼠标悬停节点
        private BaseNodeDesigner MouseOverNode()
        {
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                BaseNodeDesigner node = m_Nodes[i];
                if (node.Rect.Contains(m_MouseWorldPoint))
                {
                    return node;
                }
            }
            return null;
        }

        //private Transition MouseOverTransition()
        //{
        //    for (int i = 0; i < mCurrentSkillData.nodeList.Count; i++)
        //    {
        //        BaseNodeDesigner node = (BaseNodeDesigner)mCurrentSkillData.nodeList[i];
        //        if (node is BaseCompositeNodeDesigner)
        //        {
        //            BaseCompositeNodeDesigner compositeNodeDesigner = node as BaseCompositeNodeDesigner;
        //            for (int j = 0; j < compositeNodeDesigner.transitionList.Count; j++)
        //            {
        //                Transition transition = compositeNodeDesigner.transitionList[j];
        //                if (transition != null)
        //                {
        //                    Vector3 start = transition.fromNode.position.center;
        //                    Vector3 end = transition.toNode.position.center;
        //                    Vector3 cross = Vector3.Cross((start - end).normalized, Vector3.forward);
        //                    if (HandleUtility.DistanceToLine(start, end) < 3f)
        //                    {
        //                        return transition;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}


        //绘制框选矩形
        private void DrawSelectionRect()
        {
            if (m_SelectionMode != SelectionMode.Rect)
                return;

            Vector2 tmpStartLocalPoint = WorldToLocalPoint(m_SelectionStartPosition) / m_ZoomScale;
            Vector2 tmpEndLocalPoint = WorldToLocalPoint(m_MouseWorldPoint) / m_ZoomScale;
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

            Vector2 delta = Vector2.zero;

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

            if (delta != Vector2.zero)
            {
                delta /= m_ZoomScale;
                for (int i = 0; i < m_SelectionNodes.Count; i++)
                {
                    BaseNodeDesigner node = m_SelectionNodes[i];
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
        private Rect FromToRect(Vector2 start, Vector2 end)
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
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                BaseNodeDesigner node = m_Nodes[i];
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
            Vector2 center = Vector2.zero;
            if (m_Nodes.Count > 0)
            {
                for (int i = 0; i < m_Nodes.Count; i++)
                {
                    BaseNodeDesigner node = m_Nodes[i];
                    center += new Vector2(node.Rect.x - m_ScaledViewSize.width * 0.5f, node.Rect.center.y - m_ScaledViewSize.height * 0.5f);
                }
                center /= m_Nodes.Count;
            }
            else
            {
                center = Center;
            }
            UpdateOffset(center);
        }

        public Vector2 LocalToWorldPoint(Vector2 point)
        {
            return point / m_ZoomScale + m_Offset;
        }

        public Vector2 WorldToLocalPoint(Vector2 point)
        {
            return (point - m_Offset) * m_ZoomScale;
        }

        private void DragView()
        {
            if (!m_MouseMiddle)
                return;

            UpdateOffset(m_ScrollPosition - m_Deltal);

        }
    }
}