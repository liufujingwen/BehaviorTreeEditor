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
        private const float MaxViewSize = 50000f;
        private const int GridMinorSize = 12;
        private const int GridMajorSize = 120;
        private Graphics m_Graphics = null;
        private BufferedGraphicsContext m_BufferedGraphicsContext = null;
        private BufferedGraphics m_BufferedGraphics = null;
        private Pen m_PenNormal = null;
        private Pen m_PenBold = null;
        private Point m_Offset = new Point(0, 0);
        private Rect m_ViewSize;//原来视窗大小，没有经过缩放的大小
        private Rect m_ScaledViewSize; //缩放后的视窗大小

        BaseNodeDesigner m_Node1;
        BaseNodeDesigner m_Node2;

        private float m_ZoomScale = 1.0f;

        private BaseNodeDesigner SelectedNode = null;
        private Rect Rect;

        public ContentUserControl()
        {
            InitializeComponent();
            m_Node1 = new BaseNodeDesigner("并行节点", new Rectangle(100, 100, 150, 50));
            m_Node2 = new BaseNodeDesigner("顺序节点", new Rectangle(400, 100, 150, 50));
        }

        private void ContentUserControl_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            this.MouseWheel += ContentUserControl_MouseWheel;

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

        private void Begin(object sender, PaintEventArgs e)
        {
            //获取视窗大小
            m_ViewSize = new Rect(e.ClipRectangle);
            float scale = 1.0f / m_ZoomScale;
            m_ScaledViewSize = new Rect(m_ViewSize.x * scale, m_ViewSize.y * scale, m_ViewSize.width * scale, m_ViewSize.height * scale);
            Rect scaledViewSize = new Rect((int)m_ScaledViewSize.x, (int)m_ScaledViewSize.y, (int)m_ScaledViewSize.width, (int)m_ScaledViewSize.height);

            m_BufferedGraphicsContext = BufferedGraphicsManager.Current;
            m_BufferedGraphics = m_BufferedGraphicsContext.Allocate(e.Graphics, scaledViewSize);
            m_Graphics = m_BufferedGraphics.Graphics;
            m_Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            m_Graphics.Clear(this.BackColor);

            Matrix matrix = m_Graphics.Transform;
            matrix.Scale(m_ZoomScale, m_ZoomScale);
            m_Graphics.Transform = matrix;

            DrawGrid();
            DrawIcon();

            m_BufferedGraphics.Render();
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
        private void DrawGridLines(Pen pen, RectangleF rect, int gridSize, PointF offset)
        {
            for (float i = rect.X + (offset.X < 0 ? gridSize : 0) + offset.X % gridSize; i < rect.X + rect.Width; i = i + gridSize)
            {
                this.DrawLine(pen, new PointF(i, rect.Y), new PointF(i, rect.Y + rect.Height));
            }
            for (float j = rect.Y + (offset.Y < 0 ? gridSize : 0) + offset.Y % gridSize; j < rect.Y + rect.Height; j = j + gridSize)
            {
                this.DrawLine(pen, new PointF(rect.X, j), new PointF(rect.X + rect.Width, j));
            }
        }

        private void DrawLine(Pen pen, PointF p1, PointF p2)
        {
            m_Graphics.DrawLine(pen, p1, p2);
        }

        private void ContentUserControl_Resize(object sender, EventArgs e)
        {
        }

        public void DrawIcon()
        {
            BezierLink.Draw(m_Graphics, m_Node1, m_Node2, Color.Blue, 2);
            m_Node1.Draw(m_PenNormal, m_Graphics);
            m_Node2.Draw(m_PenNormal, m_Graphics);

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
            Vector2 offset = (m_ScaledViewSize.size - m_ViewSize.size) * 0.5f;
            m_ZoomScale += e.Delta * 0.0003f;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale, 0.5f, 2.0f);
        }

        //
        private void ContentUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            Point point = TranformPoint(e.Location);

            if (m_Node1.IsContains(point))
            {
                SelectedNode = m_Node1;
            }
            else if (m_Node2.IsContains(point))
            {
                SelectedNode = m_Node2;
            }

            Console.WriteLine(point);
        }

        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {

            Point point = TranformPoint(e.Location);

            if (e.Button == MouseButtons.Left)
            {
                //point.X = (int)(point.X * m_ZoomScale);
                //point.Y = (int)(point.Y * m_ZoomScale);
                if (SelectedNode != null)
                {
                    SelectedNode.Drag(point);
                }
            }

        }

        private Point TranformPoint(Point point)
        {
            point.X = (int)(point.X / m_ZoomScale);
            point.Y = (int)(point.Y / m_ZoomScale);
            return point;
        }

        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedNode = null;
        }
    }
}
