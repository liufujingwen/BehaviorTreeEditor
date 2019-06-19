using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public static class EditorEx
    {
        public static float Magitude(this Point point)
        {
            return (float)Math.Sqrt((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static float SqrMagitude(this Point point)
        {
            return (float)((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static float Magitude(this PointF point)
        {
            return (float)Math.Sqrt((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static float SqrMagitude(this PointF point)
        {
            return (float)((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static Point Normalized(this Point point)
        {
            return Mathf.Normalize(point);
        }

        public static PointF Normalized(this PointF point)
        {
            return Mathf.Normalize(point);
        }

        public static PointF Size(this RectangleF rectangle)
        {
            return new PointF(rectangle.Width, rectangle.Height);
        }

        public static Point Size(this Rectangle rectangle)
        {
            return new Point(rectangle.Width, rectangle.Height);
        }

    }
}
