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
        public static void Draw(Graphics graphics, Pen pen, BaseNodeDesigner fromNode, BaseNodeDesigner toNode, Color linkColor, float linkWidth)
        {
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
            int num = (int)Math.Min(distance * 0.5f, 80);

            Point fromTangent = fromPoint;
            fromTangent.X = fromTangent.X + (int)(fromTangentDir * num);

            Point tempPoint = toPoint;
            toPoint.X = toPoint.X + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Point toTangent = toPoint;
            toTangent.X = toTangent.X + (int)(toTangentDir * num);

            graphics.DrawBezier(pen, fromPoint, fromTangent, toTangent, toPoint);


            if (toTangentDir > 0)
            {
                graphics.DrawImage(EditorUtility.LeftArrowImage, new Point(tempPoint.X, toPoint.Y - EditorUtility.LeftArrowImage.Height / 2 - 2));
            }
            else
            {
                graphics.DrawImage(EditorUtility.RightArrowImage, new Point(tempPoint.X - EditorUtility.RightArrowImage.Width, toPoint.Y - EditorUtility.RightArrowImage.Height / 2 - 2));
            }

            //画箭头
        }
    }
}
