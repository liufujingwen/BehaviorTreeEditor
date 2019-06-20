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

        private float m_ZoomScale = 1.0f;

        private BaseNodeDesigner SelectedNode = null;

        private float m_Deltaltime;
        private DateTime m_DrawTime;
        private int m_Fps = 0;

        //记录上一次鼠标位置
        private Vector2 m_MousePoint;

        private bool m_PanNode;

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

            m_Node1 = new BaseNodeDesigner("并行节点", new Rect(100 + m_Offset.x, 100 + m_Offset.y, 150, 100));
            m_Node2 = new BaseNodeDesigner("顺序节点", new Rect(400 + m_Offset.x, 100 + m_Offset.y, 150, 100));

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

            DrawGrid();
            DrawIcon();

            m_BufferedGraphics.Render();

            AutoPanNodes(3.5f);

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

        private void ContentUserControl_Resize(object sender, EventArgs e)
        {
        }

        public void DrawIcon()
        {
            BezierLink.Draw(m_Graphics, m_Node1, m_Node2, Color.Blue, 2, m_Offset);
            EditorUtility.Draw(m_Node1, m_PenNormal, m_Graphics, m_Offset, m_ZoomScale);
            EditorUtility.Draw(m_Node2, m_PenNormal, m_Graphics, m_Offset, m_ZoomScale);
        }

        private void ContentUserControl_MouseClick(object sender, MouseEventArgs e)
        {
            //if (m_Node.Contains(e.Location))
            //{
            //    MessageBox.Show(e.ToString());
            //}
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
            Vector2 worldMousePoint = LocalToWorldPoint(m_MousePoint);

            if (m_Node1.IsContains(worldMousePoint))
            {
                SelectedNode = m_Node1;
            }
            else if (m_Node2.IsContains(worldMousePoint))
            {
                SelectedNode = m_Node2;
            }
        }

        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 currentMousePoint = (Vector2)e.Location;
            Vector2 mouseOffset = currentMousePoint - m_MousePoint;
            m_MousePoint = currentMousePoint;

            if (e.Button == MouseButtons.Left)
            {
                if (SelectedNode != null)
                {
                    Console.WriteLine(LocalToWorldPoint(mouseOffset) + "    " + mouseOffset);
                    DragNodes(LocalToWorldPoint(mouseOffset));
                }
            }
        }

        /// <summary>
        /// 拖拽节点
        /// </summary>
        /// <param name="delta">偏移量</param>
        private void DragNodes(Vector2 delta)
        {
            if (SelectedNode == null)
                return;
            SelectedNode.AddPoint(delta);
        }

        private void AutoPanNodes(float speed)
        {
            if (SelectedNode == null)
                return;

            if (MouseButtons != MouseButtons.Left)
                return;

            m_PanNode = false;
            Vector2 delta = Vector2.zero;
            var a = MousePosition;

            if (m_MousePoint.x > m_ScaledViewSize.width + m_ScrollPosition.x - 50)
            {
                delta.x -= speed;
            }

            if ((m_MousePoint.x < m_ScrollPosition.x + 50) && m_ScrollPosition.x > 0f)
            {
                delta.x += speed;
            }

            if (m_MousePoint.y > m_ScaledViewSize.height + m_ScrollPosition.y - 50f)
            {
                delta.y -= speed;
            }

            if ((m_MousePoint.y < m_ScaledViewSize.y + 50f) && m_ScrollPosition.y > 0f)
            {
                delta.y += speed;
            }

            if (delta != Vector2.zero)
            {
                m_PanNode = true;
                m_Node2.AddPoint(delta);
                UpdateScrollPosition(m_ScrollPosition + delta);
            }
        }

        public Vector2 LocalToWorldPoint(Vector2 point)
        {
            return point / m_ZoomScale - m_Offset;
        }

        public Vector2 WorldToLocalPoint(Vector2 point)
        {
            return (point + m_Offset) * m_ZoomScale;
        }

        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedNode = null;
        }


    }
}
