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
        public static Pen LineNormalPen = new Pen(Color.Red, 2);//节点连线

    }
}
