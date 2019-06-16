using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class BaseNodeDesigner
    {
        public string Name = string.Empty;//名字

        public TextureBrush SelectedImage;//选中状态图片
        public Rectangle TitleRect;//标题矩形
        public Rectangle ContentRect;//内容矩形

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(TitleRect.X, TitleRect.Y, ContentRect.Width, TitleRect.Height + ContentRect.Height);
            }
        }

        //左边连接点
        public Point LeftLinkPoint
        {
            get
            {
                return new Point(TitleRect.X, TitleRect.Y + TitleRect.Height / 2);
            }
        }

        //右边连接点
        public Point RightLinkPoint
        {
            get
            {
                return new Point(TitleRect.X + TitleRect.Width, TitleRect.Y + TitleRect.Height / 2);
            }
        }

        public BaseNodeDesigner(string name, Rectangle rect)
        {
            Name = name;
            TitleRect = new Rectangle(rect.X, rect.Y, rect.Width, EditorUtility.TitleNodeHeight);
            ContentRect = new Rectangle(rect.X, rect.Y + EditorUtility.TitleNodeHeight, rect.Width, rect.Height);
        }


        public void Draw(Pen pen, Graphics graphics)
        {
            //画标题底框
            graphics.FillRectangle(EditorUtility.TitleBrush, TitleRect);
            //标题
            graphics.DrawString(Name, EditorUtility.NodeFont, EditorUtility.NodeBrush, TitleRect.X + TitleRect.Width / 2, TitleRect.Y + TitleRect.Height / 2 + 1, EditorUtility.TitleFormat);
            //画内容底框
            graphics.FillRectangle(EditorUtility.ContentBrush, ContentRect);
            //画标题边框
            graphics.DrawRectangle(pen, TitleRect);
            //画内容边框
            graphics.DrawRectangle(pen, Rect);
        }

        /// <summary>
        /// 指定点是否在控件范围内
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns>true:在控件范围内，false:不在控件范围内</returns>
        public bool IsContains(Point point)
        {
            return Rect.Contains(point);
        }

        bool flag = false;
        public void Drag(Point point)
        {

            Point offset = new Point(0, 0);
            if (!flag)
            {
                flag = true;
                offset = new Point(point.X - TitleRect.X, point.Y - TitleRect.Y);
            }

            TitleRect.X = point.X - offset.X;
            TitleRect.Y = point.Y - offset.Y;

            ContentRect.X = TitleRect.X;
            ContentRect.Y = TitleRect.Y + EditorUtility.TitleNodeHeight;
        }
    }
}
