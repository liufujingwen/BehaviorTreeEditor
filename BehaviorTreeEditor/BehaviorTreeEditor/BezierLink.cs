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
        private static PointF[] ArrowPoint = new PointF[3];
        private static Pen ms_LinePen = null;
        private static Brush ms_ArrowBrush = null;

        public static void Draw(Graphics graphics, BaseNodeDesigner fromNode, BaseNodeDesigner toNode, Color linkColor, float linkWidth, Vector2 offset)
        {
            if (ms_LinePen == null)
            {
                ms_LinePen = new Pen(linkColor, linkWidth);
            }

            if (ms_ArrowBrush == null)
            {
                ms_ArrowBrush = new SolidBrush(linkColor);
            }

            Rect fromTitleRect = EditorUtility.GetTitleRect(fromNode, offset);
            Rect toTitleRect = EditorUtility.GetContentRect(toNode, offset);

            Vector2 fromPoint = Vector2.zero;
            Vector2 toPoint = Vector2.zero;

            int fromTangentDir = 1;
            int toTangentDir = 1;

            //结束点x在开始点x右边
            if (toTitleRect.x > fromTitleRect.x + toTitleRect.width)
            {
                fromPoint = EditorUtility.GetRightLinkPoint(fromNode, offset);
                toPoint = EditorUtility.GetLeftLinkPoint(toNode, offset);
                fromTangentDir = 1;
                toTangentDir = -1;
            }
            //结束点x在起始点x + width范围
            else if (toTitleRect.x >= fromTitleRect.x && fromTitleRect.x <= (fromTitleRect.x + fromTitleRect.width))
            {
                fromPoint = EditorUtility.GetRightLinkPoint(fromNode, offset);
                toPoint = EditorUtility.GetRightLinkPoint(toNode, offset);
                fromTangentDir = 1;
                toTangentDir = 1;
            }
            else if ((toTitleRect.x + toTitleRect.width) >= fromTitleRect.x)
            {
                fromPoint = EditorUtility.GetLeftLinkPoint(fromNode, offset);
                toPoint = EditorUtility.GetLeftLinkPoint(toNode, offset);
                fromTangentDir = -1;
                toTangentDir = -1;
            }
            //结束点x在开始点x左边
            else if ((toTitleRect.x + toTitleRect.width) < fromTitleRect.x)
            {
                fromPoint = EditorUtility.GetLeftLinkPoint(fromNode, offset);
                toPoint = EditorUtility.GetRightLinkPoint(toNode, offset);
                fromTangentDir = -1;
                toTangentDir = 1;
            }

            ////求出两点距离
            double distance = (toPoint - fromPoint).magnitude;
            int num = (int)Math.Min(distance * 0.5f, 40);

            Vector2 fromTangent = fromPoint;
            fromTangent.x = fromTangent.x + (int)(fromTangentDir * num);

            Vector2 tempPoint = toPoint;
            toPoint.x = toPoint.x + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Vector2 toTangent = toPoint;
            toTangent.x = toTangent.x + (int)(toTangentDir * num);

            graphics.DrawBezier(ms_LinePen, fromPoint, fromTangent, toTangent, toPoint);

            //画箭头
            ArrowPoint[0] = tempPoint;
            ArrowPoint[1] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y + 5);
            ArrowPoint[2] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y - 5);
            graphics.FillPolygon(ms_ArrowBrush, (PointF[])ArrowPoint);

        }
    }
}
