using BehaviorTreeEditor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public static class EditorUtility
    {
        static EditorUtility()
        {
            TitleBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
            ContentBrush.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
            TitleFormat.LineAlignment = StringAlignment.Center;
            TitleFormat.Alignment = StringAlignment.Center;
        }

        public static Font NodeFont = new Font("宋体", 15, FontStyle.Regular);
        public static Brush NodeBrush = new SolidBrush(Color.White);
        public static int TitleNodeHeight = 30;//标题节点高
        public static TextureBrush TitleBrush = new TextureBrush(Resources.NodeBackground_Dark);//普通状态图片
        public static TextureBrush ContentBrush = new TextureBrush(Resources.NodeBackground_Light);//普通状态图片
        public static StringFormat TitleFormat = new StringFormat(StringFormatFlags.NoWrap);
        public static Pen LineNormalPen = new Pen(Color.Green, 2);//节点连线
        public static int ArrowWidth = 17;//箭头宽度像素
        public static int ArrowHeight = 10;//箭头高度度像素

        //节点标题Rect
        public static Rect GetTitleRect(BaseNodeDesigner node, Vector2 offset)
        {
            return new Rect(node.Rect.x - offset.x, node.Rect.y - offset.y, node.Rect.width, EditorUtility.TitleNodeHeight);
        }

        //节点内存Rect
        public static Rect GetContentRect(BaseNodeDesigner node, Vector2 offset)
        {
            return new Rect(node.Rect.x - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight - offset.y, node.Rect.width, node.Rect.height - EditorUtility.TitleNodeHeight);
        }

        //左边连接点
        public static Vector2 GetLeftLinkPoint(BaseNodeDesigner node, Vector2 offset)
        {
            return new Vector2(node.Rect.x - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f - offset.y);
        }

        //右边连接点
        public static Vector2 GetRightLinkPoint(BaseNodeDesigner node, Vector2 offset)
        {
            return new Vector2(node.Rect.x + node.Rect.width - offset.x, node.Rect.y + EditorUtility.TitleNodeHeight / 2.0f - offset.y);
        }

        public static bool Contains(BaseNodeDesigner node, Vector2 offset, Vector2 point)
        {
            Rect localRect = node.Rect - offset;
            return localRect.Contains(point);
        }

        public static void Draw(BaseNodeDesigner node, Pen pen, Graphics graphics, Vector2 offset, float scale)
        {
            Rect titleRect = GetTitleRect(node, offset);
            Rect contentRect = GetContentRect(node, offset);

            //画标题底框
            graphics.FillRectangle(EditorUtility.TitleBrush, titleRect);
            //标题
            graphics.DrawString(node.Name, EditorUtility.NodeFont, EditorUtility.NodeBrush, titleRect.x + titleRect.width / 2, titleRect.y + titleRect.height / 2 + 1, EditorUtility.TitleFormat);
            //画内容底框
            graphics.FillRectangle(EditorUtility.ContentBrush, contentRect);
            //画边框
            graphics.DrawRectangle(pen, node.Rect - offset);

            graphics.DrawString(node.Rect.x + " " + node.Rect.y, EditorUtility.NodeFont, EditorUtility.NodeBrush, titleRect.x + titleRect.width / 2, titleRect.y + titleRect.height / 2 + contentRect.height / 3 + 1, EditorUtility.TitleFormat);
            graphics.DrawString(node.Rect.x + " " + node.Rect.y, EditorUtility.NodeFont, EditorUtility.NodeBrush, titleRect.x + titleRect.width / 2, titleRect.y + titleRect.height / 2 + contentRect.height + 1, EditorUtility.TitleFormat);

        }
    }
}
