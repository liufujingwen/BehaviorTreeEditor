using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public static class BezierLink
    {
        private static Point[] ArrowPoint = new Point[3];
        private static Pen ms_LinePen = null;
        private static Brush ms_ArrowBrush = null;

        public static void Draw(Graphics graphics, BaseNodeDesigner fromNode, BaseNodeDesigner toNode, Color linkColor, float linkWidth)
        {
            if (ms_LinePen == null)
            {
                ms_LinePen = new Pen(linkColor, linkWidth);
            }

            if (ms_ArrowBrush == null)
            {
                ms_ArrowBrush = new SolidBrush(linkColor);
            }

            Rectangle fromTitleRect = fromNode.TitleRect;
            Rectangle toTitleRect = toNode.TitleRect;

            Point fromPoint = default(Point);
            Point toPoint = default(Point);

            int fromTangentDir = 1;
            int toTangentDir = 1;

            //结束点x在开始点x右边
            if (toTitleRect.X > fromTitleRect.X + toTitleRect.Width)
            {
                fromPoint = fromNode.RightLinkPoint;
                toPoint = toNode.LeftLinkPoint;
                fromTangentDir = 1;
                toTangentDir = -1;
            }
            //结束点x在起始点x + width范围
            else if (toTitleRect.X >= fromTitleRect.X && fromTitleRect.X <= (fromTitleRect.X + fromTitleRect.Width))
            {
                fromPoint = fromNode.RightLinkPoint;
                toPoint = toNode.RightLinkPoint;
                fromTangentDir = 1;
                toTangentDir = 1;
            }
            else if ((toTitleRect.X + toTitleRect.Width) >= fromTitleRect.X)
            {
                fromPoint = fromNode.LeftLinkPoint;
                toPoint = toNode.LeftLinkPoint;
                fromTangentDir = -1;
                toTangentDir = -1;
            }
            //结束点x在开始点x左边
            else if ((toTitleRect.X + toTitleRect.Width) < fromTitleRect.X)
            {
                fromPoint = fromNode.LeftLinkPoint;
                toPoint = toNode.RightLinkPoint;
                fromTangentDir = -1;
                toTangentDir = 1;
            }

            ////求出两点距离
            double distance = Math.Sqrt(Math.Abs(toPoint.X - fromPoint.X) * Math.Abs(toPoint.X - fromPoint.X) + Math.Abs(toPoint.Y - fromPoint.Y) * Math.Abs(toPoint.Y - fromPoint.Y));
            int num = (int)Math.Min(distance * 0.5f, 40);

            Point fromTangent = fromPoint;
            fromTangent.X = fromTangent.X + (int)(fromTangentDir * num);

            Point tempPoint = toPoint;
            toPoint.X = toPoint.X + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Point toTangent = toPoint;
            toTangent.X = toTangent.X + (int)(toTangentDir * num);

            graphics.DrawBezier(ms_LinePen, fromPoint, fromTangent, toTangent, toPoint);

            //画箭头
            ArrowPoint[0] = tempPoint;
            ArrowPoint[1] = new Point(tempPoint.X + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.Y + 5);
            ArrowPoint[2] = new Point(tempPoint.X + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.Y - 5);
            graphics.FillPolygon(ms_ArrowBrush, ArrowPoint);

        }
    }
}
