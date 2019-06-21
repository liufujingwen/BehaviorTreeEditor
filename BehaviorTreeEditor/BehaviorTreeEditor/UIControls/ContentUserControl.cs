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
        private const int MaxViewSize = 50000;
        private const int GridMinorSize = 12;
        private const int GridMajorSize = 120;
        private Graphics m_Graphics = null;
        private BufferedGraphicsContext m_BufferedGraphicsContext = null;
        private BufferedGraphics m_BufferedGraphics = null;
        private Pen m_PenNormal = null;
        private Pen m_PenBold = null;
        private Vector2 m_Offset = Vector2.zero;
        private Rect m_ViewSize;//原来视窗大小，没有经过缩放的大小
        private Rect m_ScaledViewSize; //缩放后的视窗大小
        private Vector2 m_ScrollPosition;

        BaseNodeDesigner m_Node1;
        BaseNodeDesigner m_Node2;

        //当前缩放
        private float m_ZoomScale = 1.0f;

        private BaseNodeDesigner SelectedNode = null;

        private float m_Deltaltime;
        private DateTime m_DrawTime;
        private int m_Fps = 0;

        //记录上一次鼠标位置(坐标系为Local)
        private Vector2 m_MouseLocalPoint;
        //记录上一次鼠标位置(坐标系为World)
        private Vector2 m_MouseWorldPoint;
        //当前事件
        private Event m_CurrentEvent;
        //是否正在拖拽节点
        private bool m_DragingNode = false;
        //鼠标移动偏移量
        private Vector2 m_Deltal;

        private SelectionMode m_SelectionMode;
        private Vector2 m_SelectionStartPosition;

        private List<BaseNodeDesigner> m_Nodes = new List<BaseNodeDesigner>();
        private List<BaseNodeDesigner> m_SelectionNodes = new List<BaseNodeDesigner>();


        public enum Event
        {
            None,
            MouseDown,
            MouseUp,
            MouseDrag,
            PanNode,
        }

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
                return new Vector2(MaxViewSize * 0.5f, MaxViewSize * 0.5f);
            }
        }

        private void ContentUserControl_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.MouseWheel += ContentUserControl_MouseWheel;

            UpdateScrollPosition(new Vector2(0, 0));
            //UpdateScrollPosition(Center);

            m_Node1 = new BaseNodeDesigner("并行节点", new Rect(100 + m_Offset.x, 100 + m_Offset.y, 150, 100));
            m_Node2 = new BaseNodeDesigner("顺序节点", new Rect(400 + m_Offset.x, 100 + m_Offset.y, 150, 100));

            m_Nodes.Add(m_Node1);
            m_Nodes.Add(m_Node2);

            m_DrawTime = DateTime.Now;
            m_Graphics = this.CreateGraphics();
            m_PenNormal = new Pen(Color.Gray, 1f);
            m_PenBold = new Pen(Color.Gray, 2f);
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

            DoNodes();

            m_BufferedGraphics.Render();

            //AutoPanNodes(3.5f);

            UpdateScrollPosition(m_ScrollPosition);
        }

        protected void UpdateScrollPosition(Vector2 position)
        {
            m_Offset = m_Offset + (m_ScrollPosition - position);
            m_ScrollPosition = position;
        }

        private void End(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 画背景格子
        /// </summary>
        private void DrawGrid()
        {
            this.DrawGridLines(m_PenNormal, m_ScaledViewSize, GridMinorSize, m_Offset);
            this.DrawGridLines(m_PenBold, m_ScaledViewSize, GridMajorSize, m_Offset);
        }

        /// <summary>
        /// 画格子线
        /// </summary>
        private void DrawGridLines(Pen pen, Rect rect, int gridSize, Vector2 offset)
        {
            for (float i = rect.x + (offset.x < 0 ? gridSize : 0) + offset.x % gridSize; i < rect.x + rect.width; i = i + gridSize)
            {
                this.DrawLine(pen, new Vector2(i, rect.y), new Vector2(i, rect.y + rect.height));
            }
            for (float j = rect.y + (offset.y < 0 ? gridSize : 0) + offset.y % gridSize; j < rect.y + rect.height; j = j + gridSize)
            {
                this.DrawLine(pen, new Vector2(rect.x, j), new Vector2(rect.x + rect.width, j));
            }
        }

        private void DrawLine(Pen pen, Vector2 p1, Vector2 p2)
        {
            m_Graphics.DrawLine(pen, p1, p2);
        }

        private void DoNodes()
        {
            DrawGrid();
            DrawIcon();
            AutoPanNodes(3.5f);
        }

        private void DrawIcon()
        {
            BezierLink.Draw(m_Graphics, m_Node1, m_Node2, Color.Blue, 2, m_Offset);
            EditorUtility.Draw(m_Node1, m_PenNormal, m_Graphics, m_Offset, m_ZoomScale);
            EditorUtility.Draw(m_Node2, m_PenNormal, m_Graphics, m_Offset, m_ZoomScale);
        }

        private void DoNodeEvents()
        {
            if (MouseButtons != MouseButtons.Left)
                return;

            DragNodes();
        }

        /// <summary>
        /// 鼠标滚轮
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ScrollEventArgs</param>
        private void ContentUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            m_ZoomScale += e.Delta * 0.0003f;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale, 0.5f, 2.0f);
            UpdateRect();
            Vector2 offset = (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f;
            UpdateScrollPosition(m_ScrollPosition - (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f + offset);
        }

        //
        private void ContentUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            m_CurrentEvent = Event.MouseDown;

            //处理选择节点
            if (e.Button == MouseButtons.Left)
            {
                m_SelectionStartPosition = m_MouseWorldPoint;
                BaseNodeDesigner node = MouseOverNode();
                //Transition transition = MouseOverTransition();

                if (node != null)
                {
                    if (!m_SelectionNodes.Contains(node))
                    {
                        m_SelectionNodes.Clear();
                        m_SelectionNodes.Add(node);
                    }
                }
            }

        }

        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            m_CurrentEvent = Event.MouseUp;
            m_SelectionNodes.Clear();
            m_DragingNode = false;

        }

        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 currentMousePoint = (Vector2)e.Location;
            m_Deltal = currentMousePoint - m_MouseLocalPoint;
            m_MouseLocalPoint = currentMousePoint;
            m_MouseWorldPoint = LocalToWorldPoint(m_MouseLocalPoint);

            Console.WriteLine("当前Local坐标：" + m_MouseLocalPoint + " 当前世界坐标:" + m_MouseWorldPoint);

            if (MouseButtons == MouseButtons.Left && m_CurrentEvent == Event.MouseDown && m_SelectionNodes.Count > 0)
            {
                m_DragingNode = true;
                for (int i = 0; i < m_SelectionNodes.Count; i++)
                {
                    BaseNodeDesigner node = m_SelectionNodes[i];
                    if (node == null)
                        continue;
                    node.Rect += m_Deltal;
                }
            }
            m_DragingNode = false;
        }

        private void DragNodes()
        {


        }

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

        private void AutoPanNodes(float speed)
        {
            if (MouseButtons != MouseButtons.Left)
                return;

            if (m_SelectionNodes.Count == 0)
                return;

            Vector2 delta = Vector2.zero;
            var a = MousePosition;

            if (m_MouseLocalPoint.x > m_ScaledViewSize.width + m_ScrollPosition.x - 50)
            {
                delta.x -= speed;
            }

            if ((m_MouseLocalPoint.x < m_ScrollPosition.x + 50) && m_ScrollPosition.x > 0f)
            {
                delta.x += speed;
            }

            if (m_MouseLocalPoint.y > m_ScaledViewSize.height + m_ScrollPosition.y - 50f)
            {
                delta.y -= speed;
            }

            if ((m_MouseLocalPoint.y < m_ScaledViewSize.y + 50f) && m_ScrollPosition.y > 0f)
            {
                delta.y += speed;
            }

            if (delta != Vector2.zero)
            {
                for (int i = 0; i < m_SelectionNodes.Count; i++)
                {
                    BaseNodeDesigner node = m_SelectionNodes[i];
                    if (node == null)
                        continue;
                    node.Rect -= delta;
                }
                UpdateScrollPosition(m_ScrollPosition + delta);
            }
        }

        public Vector2 LocalToWorldPoint(Vector2 point)
        {
            return point / m_ZoomScale + m_Offset;
        }

        public Vector2 WorldToLocalPoint(Vector2 point)
        {
            return (point - m_Offset) * m_ZoomScale;
        }


    }
}
