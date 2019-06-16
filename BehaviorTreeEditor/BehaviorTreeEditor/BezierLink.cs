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
            graphics.DrawBezier(pen, fromNode.RightLinkPoint, fromNode.RightLinkPoint, toNode.LeftLinkPoint, toNode.LeftLinkPoint);
        }
    }
}
