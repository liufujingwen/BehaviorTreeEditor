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
        private Pen m_PenNormal = null;
        private Pen m_PenBold = null;
        private Point m_Offset = new Point(0, 0);
        private Rectangle m_Rect;

        BaseNodeDesigner m_Node1;
        BaseNodeDesigner m_Node2;

        private float m_ZoomScale = 1;

        private BaseNodeDesigner SelectedNode = null;
        private Rectangle Rect;

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
            m_PenNormal = new Pen(Color.Black, 1f);
            m_PenBold = new Pen(Color.Black, 2);
            timer1.Start();
        }

        //定时器控制刷新频率
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void ContentUserControl_Paint(object sender, PaintEventArgs e)
        {
            m_BufferedGraphicsContext = BufferedGraphicsManager.Current;
            Rect = e.ClipRectangle;
            Rect.Width = m_ZoomScale <= 1.0f ? (int)(Rect.Width / m_ZoomScale) : Rect.Width;
            Rect.Height = m_ZoomScale <= 1.0f ? (int)(Rect.Height / m_ZoomScale) : Rect.Height;
            m_BufferedGraphics = m_BufferedGraphicsContext.Allocate(e.Graphics, Rect);
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

        /// <summary>
        /// 画背景格子
        /// </summary>
        private void DrawGrid()
        {
            this.DrawGridLines(m_PenNormal, Rect, GridMinorSize, m_Offset);
            this.DrawGridLines(m_PenBold, Rect, GridMajorSize, m_Offset);
        }

        /// <summary>
        /// 画格子线
        /// </summary>
        private void DrawGridLines(Pen pen, Rectangle rect, int gridSize, Point offset)
        {
            for (int i = rect.X + (offset.X < 0 ? gridSize : 0) + offset.X % gridSize; i < rect.X + rect.Width; i = i + gridSize)
            {
                this.DrawLine(pen, new Point(i, rect.Y), new Point(i, rect.Y + rect.Height));
            }
            for (int j = rect.Y + (offset.Y < 0 ? gridSize : 0) + offset.Y % gridSize; j < rect.Y + rect.Height; j = j + gridSize)
            {
                this.DrawLine(pen, new Point(rect.X, j), new Point(rect.X + rect.Width, j));
            }
        }

        private void DrawLine(Pen pen, Point p1, Point p2)
        {
            m_Graphics.DrawLine(pen, p1, p2);
        }

        private void ContentUserControl_Resize(object sender, EventArgs e)
        {
        }

        public void DrawIcon()
        {
            BezierLink.Draw(m_Graphics, EditorUtility.LineNormalPen, m_Node1, m_Node2, Color.Green, 1);
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
        /// 鼠标滚轮时间
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">ScrollEventArgs</param>
        private void ContentUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            m_ZoomScale += e.Delta * 0.0003f;
            m_ZoomScale = Math.Max(0.5f, m_ZoomScale);
            m_ZoomScale = Math.Min(2.0f, m_ZoomScale);
        }

        //
        private void ContentUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_Node1.IsContains(e.Location))
            {
                SelectedNode = m_Node1;
            }
            else if (m_Node2.IsContains(e.Location))
            {
                SelectedNode = m_Node2;
            }
        }

        private void ContentUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("鼠标移动：" + e);
            //if (m_Node != null)
            //{
            //    m_Node.Drag(e.Location);
            //}
            //Matrix matrix = m_Graphics.Transform;
            //Point[] points = new Point[3];
            //var a = matrix* e.Location;
            if (e.Button == MouseButtons.Left)
            {
                Point point = e.Location;
                //point.X = (int)(point.X * m_ZoomScale);
                //point.Y = (int)(point.Y * m_ZoomScale);

                if (SelectedNode != null)
                {
                    SelectedNode.Drag(point);
                }
            }
            
        }

        private void ContentUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedNode = null;
        }
    }
}
