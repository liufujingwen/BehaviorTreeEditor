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
        public Rect TitleRect;//标题矩形
        public Rect ContentRect;//内容矩形

        public Rect Rect
        {
            get
            {
                return new Rect(TitleRect.x, TitleRect.y, ContentRect.width, TitleRect.height + ContentRect.height);
            }
        }

        //左边连接点
        public Vector2 LeftLinkPoint
        {
            get
            {
                return new Vector2(TitleRect.x, TitleRect.y + TitleRect.height / 2);
            }
        }

        //右边连接点
        public Vector2 RightLinkPoint
        {
            get
            {
                return new Vector2(TitleRect.x + TitleRect.width, TitleRect.y + TitleRect.height / 2);
            }
        }

        public BaseNodeDesigner(string name, Rect rect)
        {
            Name = name;
            TitleRect = new Rect(rect.x, rect.y, rect.width, EditorUtility.TitleNodeHeight);
            ContentRect = new Rect(rect.x, rect.y + EditorUtility.TitleNodeHeight, rect.width, rect.height);
        }


        public void Draw(Pen pen, Graphics graphics)
        {
            //画标题底框
            graphics.FillRectangle(EditorUtility.TitleBrush, TitleRect);
            //标题
            graphics.DrawString(Name, EditorUtility.NodeFont, EditorUtility.NodeBrush, TitleRect.x + TitleRect.width / 2, TitleRect.y + TitleRect.height / 2 + 1, EditorUtility.TitleFormat);
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
        public bool IsContains(Vector2 point)
        {
            return Rect.Contains(point);
        }

        bool flag = false;
        public void AddPoint(Vector2 delta)
        {
            TitleRect.x += delta.x;
            TitleRect.y += delta.y;

            ContentRect.x = TitleRect.x;
            ContentRect.y = TitleRect.y + EditorUtility.TitleNodeHeight;
        }
    }
}
