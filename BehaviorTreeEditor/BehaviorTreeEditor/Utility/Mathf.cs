using System;
using System.Drawing;

namespace BehaviorTreeEditor
{
    public static class Mathf
    {
        public static Point zero = new Point(0, 0);
        public static PointF zeroF = new PointF(0f, 0f);

        public static float Clamp(float value, float min, float max)
        {
            if ((double)value < (double)min)
                value = min;
            else if ((double)value > (double)max)
                value = max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }

        public static float Clamp01(float value)
        {
            if ((double)value < 0.0)
                return 0.0f;
            if ((double)value > 1.0)
                return 1f;
            return value;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Mathf.Clamp01(t);
        }

        public static double Distance(Point a, Point b)
        {
            Point point = new Point(a.X - b.X, a.Y - b.Y);
            return (float)Math.Sqrt(((double)point.X * (double)point.X + (double)point.Y * (double)point.Y));
        }

        public static float Distance(PointF a, PointF b)
        {
            PointF point = new PointF(a.X - b.X, a.Y - b.Y);
            return (float)Math.Sqrt(((double)point.X * (double)point.X + (double)point.Y * (double)point.Y));
        }

        public static float Magitude(Point point)
        {
            return (float)Math.Sqrt((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static float SqrMagitude(Point point)
        {
            return (float)((double)point.X * (double)point.X + (double)point.Y * (double)point.Y);
        }

        public static float Dot(Point lhs, Point rhs)
        {
            return (float)((double)lhs.X * (double)rhs.X + (double)lhs.Y * (double)rhs.Y);
        }

        public static float Dot(PointF lhs, PointF rhs)
        {
            return (float)((double)lhs.X * (double)rhs.X + (double)lhs.Y * (double)rhs.Y);
        }

        public static Point Normalize(Point value)
        {
            float num = value.Magitude();
            if ((double)num > 9.99999974737875E-06)
                return new Point((int)(value.X / num), (int)(value.Y / num));
            return zero;
        }

        public static PointF Normalize(PointF value)
        {
            float num = value.Magitude();
            if ((double)num > 9.99999974737875E-06)
                return new PointF((float)(value.X / num), (float)(value.Y / num));
            return zeroF;
        }

        public static float Angle(Point from, Point to)
        {
            return (float)Math.Acos(Mathf.Clamp(Dot(from.Normalized(), to.Normalized()), -1f, 1f)) * 57.29578f;
        }

        public static float Angle(PointF from, PointF to)
        {
            return (float)Math.Acos(Mathf.Clamp(Dot(from.Normalized(), to.Normalized()), -1f, 1f)) * 57.29578f;
        }
    }
}
