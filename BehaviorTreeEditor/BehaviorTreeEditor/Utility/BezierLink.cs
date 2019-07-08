using System;
using System.Collections.Generic;
using System.Drawing;

namespace BehaviorTreeEditor
{
    public static class BezierLink
    {
        private static PointF[] ArrowPoint = new PointF[3];
        //private static Pen ms_LinePen = null;
        //private static Brush ms_ArrowBrush = null;

        /// <summary>
        /// 绘制贝塞尔曲线，节点 -> 指定点
        /// </summary>
        /// <param name="graphics">graphics</param>
        /// <param name="fromNode">开始节点</param>
        /// <param name="endPoint">结束坐标点</param>
        /// <param name="linkColor">线颜色</param>
        /// <param name="linkWidth">线宽度</param>
        /// <param name="offset">偏移</param>
        public static void DrawNodeToPoint(Graphics graphics, NodeDesigner fromNode, Vec2 endPoint, Vec2 offset)
        {
            Rect fromTitleRect = EditorUtility.GetTitleRect(fromNode, offset);

            Vec2 fromPoint = EditorUtility.GetRightLinkPoint(fromNode, offset); ;
            Vec2 toPoint = endPoint;

            int fromTangentDir = 1;
            int toTangentDir = 1;

            //结束点x在开始点x右边
            if (endPoint.x > fromTitleRect.x + fromTitleRect.width)
            {
                toTangentDir = -1;
            }
            else
            {
                toTangentDir = 1;
            }

            ////求出两点距离
            double distance = (toPoint - fromPoint).magnitude;
            int num = (int)Math.Min(distance * 0.5f, 40);

            Vec2 fromTangent = fromPoint;
            fromTangent.x = fromTangent.x + (int)(fromTangentDir * num);

            Vec2 tempPoint = toPoint;
            toPoint.x = toPoint.x + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Vec2 toTangent = toPoint;
            toTangent.x = toTangent.x + (int)(toTangentDir * num);

            graphics.DrawBezier(EditorUtility.TransitionNormalPen, fromPoint, fromTangent, toTangent, toPoint);

            //画箭头
            ArrowPoint[0] = tempPoint;
            ArrowPoint[1] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y + 5);
            ArrowPoint[2] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y - 5);
            graphics.FillPolygon(EditorUtility.ArrowNormalBrush, (PointF[])ArrowPoint);

        }

        public static void DrawNodeToNode(Graphics graphics, NodeDesigner fromNode, NodeDesigner toNode, bool selected, Vec2 offset)
        {
            Pen pen = selected ? EditorUtility.TransitionSelectedPen : EditorUtility.TransitionNormalPen;
            Brush brush = selected ? EditorUtility.ArrowSelectedBrush : EditorUtility.ArrowNormalBrush;

            Rect fromTitleRect = EditorUtility.GetTitleRect(fromNode, offset);
            Rect toTitleRect = EditorUtility.GetContentRect(toNode, offset);

            Vec2 fromPoint = EditorUtility.GetRightLinkPoint(fromNode, offset);
            Vec2 toPoint = EditorUtility.GetLeftLinkPoint(toNode, offset);

            float linkPointHalfSize = EditorUtility.NodeLinkPointSize / 2.0f;
            fromPoint.x += linkPointHalfSize;
            toPoint.x -= linkPointHalfSize;

            int fromTangentDir = 1;
            int toTangentDir = -1;

            //求出两点距离
            double distance = (toPoint - fromPoint).magnitude;
            int num = (int)Math.Min(distance * 0.5f, 40);

            Vec2 fromTangent = fromPoint;
            fromTangent.x = fromTangent.x + (int)(fromTangentDir * num);

            Vec2 tempPoint = toPoint;
            toPoint.x = toPoint.x + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Vec2 toTangent = toPoint;
            toTangent.x = toTangent.x + (int)(toTangentDir * num);

            graphics.DrawBezier(pen, fromPoint, fromTangent, toTangent, toPoint);

            //画箭头
            ArrowPoint[0] = tempPoint;
            ArrowPoint[1] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y + 5);
            ArrowPoint[2] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y - 5);
            graphics.FillPolygon(brush, (PointF[])ArrowPoint);
        }

        //检测某点是否点击了曲线
        public static bool CheckPointAt(NodeDesigner fromNode, NodeDesigner toNode, Vec2 localPoint, Vec2 offset)
        {
            Rect fromTitleRect = EditorUtility.GetTitleRect(fromNode, offset);
            Rect toTitleRect = EditorUtility.GetContentRect(toNode, offset);

            Vec2 fromPoint = EditorUtility.GetRightLinkPoint(fromNode, offset);
            Vec2 toPoint = EditorUtility.GetLeftLinkPoint(toNode, offset);

            float linkPointHalfSize = EditorUtility.NodeLinkPointSize / 2.0f;
            fromPoint.x += linkPointHalfSize;
            toPoint.x -= linkPointHalfSize;

            int fromTangentDir = 1;
            int toTangentDir = -1;

            //求出两点距离
            double distance = (toPoint - fromPoint).magnitude;
            int num = (int)Math.Min(distance * 0.5f, 40);

            Vec2 fromTangent = fromPoint;
            fromTangent.x = fromTangent.x + (int)(fromTangentDir * num);

            toPoint.x = toPoint.x + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Vec2 toTangent = toPoint;
            toTangent.x = toTangent.x + (int)(toTangentDir * num);

            float tmpDistance = float.MaxValue;

            if (distance > 0)
            {
                for (float dis = 0; dis < distance;)
                {
                    dis += 6;
                    float t = (float)(dis / distance);
                    Vec2 bezierPoint = BezierPoint(t, fromPoint, fromTangent, toTangent, toPoint);
                    float temp = (localPoint - bezierPoint).magnitude;
                    if (temp < tmpDistance)
                        tmpDistance = temp;
                }
            }

            if (tmpDistance <= 6)
            {
                return true;
            }

            return false;
        }

        //调试
        public static void DrawNodeToNode_Debug(Graphics graphics, DebugNode fromNode, DebugNode toNode, Vec2 offset)
        {
            Rect fromTitleRect = EditorUtility.GetTitleRect(fromNode.Node, offset);
            Rect toTitleRect = EditorUtility.GetContentRect(toNode.Node, offset);

            Vec2 fromPoint = EditorUtility.GetRightLinkPoint(fromNode.Node, offset);
            Vec2 toPoint = EditorUtility.GetLeftLinkPoint(toNode.Node, offset);

            float linkPointHalfSize = EditorUtility.NodeLinkPointSize / 2.0f;
            fromPoint.x += linkPointHalfSize;
            toPoint.x -= linkPointHalfSize;

            int fromTangentDir = 1;
            int toTangentDir = -1;

            //求出两点距离
            double distance = (toPoint - fromPoint).magnitude;
            int num = (int)Math.Min(distance * 0.5f, 40);

            Vec2 fromTangent = fromPoint;
            fromTangent.x = fromTangent.x + (int)(fromTangentDir * num);

            Vec2 tempPoint = toPoint;
            toPoint.x = toPoint.x + (int)(toTangentDir * EditorUtility.ArrowWidth);
            Vec2 toTangent = toPoint;
            toTangent.x = toTangent.x + (int)(toTangentDir * num);

            float t = toNode.TransitionElapsedTime / DebugManager.TransitionTime;

            Pen pen = EditorUtility.Debug_TransitionPen;
            Brush brush = EditorUtility.Debug_TransitionBrush;

            if (toNode.Status == DebugNodeStatus.Transition)
            {
                pen = EditorUtility.Debug_TransitionPen;
                brush = EditorUtility.Debug_TransitionBrush;
            }
            else if (toNode.Status == DebugNodeStatus.Error)
            {
                pen = EditorUtility.Debug_Error_TransitionPen;
                brush = EditorUtility.Debug_Error_TransitionBrush;
            }
            else if (toNode.Status == DebugNodeStatus.Running)
            {
                pen = EditorUtility.Debug_Running_TransitionPen;
                brush = EditorUtility.Debug_Running_TransitionBrush;
            }
            else if (toNode.Status == DebugNodeStatus.Failed)
            {
                pen = EditorUtility.Debug_Failed_TransitionPen;
                brush = EditorUtility.Debug_Failed_TransitionBrush;
            }
            else if (toNode.Status == DebugNodeStatus.Success)
            {
                pen = EditorUtility.Debug_Successed_TransitionPen;
                brush = EditorUtility.Debug_Successed_TransitionBrush;
            }

            if (toNode.Status == DebugNodeStatus.Transition && t < 1f)
            {
                Vec2 point = BezierPoint(t, fromPoint, fromTangent, toTangent, toPoint);
                toNode.TransitionPoints.Add(point);

                for (int i = 1; i < toNode.TransitionPoints.Count; i++)
                {
                    graphics.DrawLine(pen, toNode.TransitionPoints[i - 1], toNode.TransitionPoints[i]);
                }
            }
            else
            {
                graphics.DrawBezier(pen, fromPoint, fromTangent, toTangent, toPoint);
                //画箭头
                ArrowPoint[0] = tempPoint;
                ArrowPoint[1] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y + 5);
                ArrowPoint[2] = new PointF(tempPoint.x + (toTangentDir * EditorUtility.ArrowWidth), tempPoint.y - 5);
                graphics.FillPolygon(brush, (PointF[])ArrowPoint);
            }
        }

        public static Vec2 BezierPoint(float t, Vec2 a, Vec2 b, Vec2 c, Vec2 d)
        {
            float t2 = t * t;
            float t3 = t2 * t;
            return a + (-a * 3 + t * (3 * a - a * t)) * t
            + (3 * b + t * (-6 * b + b * 3 * t)) * t
            + (c * 3 - c * 3 * t) * t2
            + d * t3;
        }
    }
}
