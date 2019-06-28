using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public struct Vec2
    {
        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vec2(PointF point)
        {
            this.x = point.X;
            this.y = point.Y;
        }

        private static readonly Vec2 zeroVector = new Vec2(0.0f, 0.0f);
        private static readonly Vec2 oneVector = new Vec2(1f, 1f);
        private static readonly Vec2 upVector = new Vec2(0.0f, 1f);
        private static readonly Vec2 downVector = new Vec2(0.0f, -1f);
        private static readonly Vec2 leftVector = new Vec2(-1f, 0.0f);
        private static readonly Vec2 rightVector = new Vec2(1f, 0.0f);
        private static readonly Vec2 positiveInfinityVector = new Vec2(float.PositiveInfinity, float.PositiveInfinity);
        private static readonly Vec2 negativeInfinityVector = new Vec2(float.NegativeInfinity, float.NegativeInfinity);

        public float x;
        public float y;
        public const float kEpsilon = 1E-05f;

        public float this[int index]
        {
            get
            {
                if (index == 0)
                    return this.x;
                if (index == 1)
                    return this.y;
                throw new IndexOutOfRangeException("Invalid Vector2 index!");
            }
            set
            {
                if (index != 0)
                {
                    if (index != 1)
                        throw new IndexOutOfRangeException("Invalid Vector2 index!");
                    this.y = value;
                }
                else
                    this.x = value;
            }
        }

        public void Set(float newX, float newY)
        {
            this.x = newX;
            this.y = newY;
        }

        public static Vec2 Lerp(Vec2 a, Vec2 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Vec2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        public static Vec2 LerpUnclamped(Vec2 a, Vec2 b, float t)
        {
            return new Vec2(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t);
        }

        public static Vec2 MoveTowards(Vec2 current, Vec2 target, float maxDistanceDelta)
        {
            Vec2 vector2 = target - current;
            float magnitude = vector2.magnitude;
            if ((double)magnitude <= (double)maxDistanceDelta || (double)magnitude == 0.0)
                return target;
            return current + vector2 / magnitude * maxDistanceDelta;
        }

        public static Vec2 Scale(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x * b.x, a.y * b.y);
        }

        public void Scale(Vec2 scale)
        {
            this.x *= scale.x;
            this.y *= scale.y;
        }

        public void Normalize()
        {
            float magnitude = this.magnitude;
            if ((double)magnitude > 9.99999974737875E-06)
                this = this / magnitude;
            else
                this = Vec2.zero;
        }

        public Vec2 normalized
        {
            get
            {
                Vec2 vector2 = new Vec2(this.x, this.y);
                vector2.Normalize();
                return vector2;
            }
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1})", (object)this.x, (object)this.y);
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1})", (object)this.x.ToString(format), (object)this.y.ToString(format));
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.y.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vec2))
                return false;
            Vec2 vector2 = (Vec2)other;
            return this.x.Equals(vector2.x) && this.y.Equals(vector2.y);
        }

        public static Vec2 Reflect(Vec2 inDirection, Vec2 inNormal)
        {
            return -2f * Vec2.Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        public static float Dot(Vec2 lhs, Vec2 rhs)
        {
            return (float)((double)lhs.x * (double)rhs.x + (double)lhs.y * (double)rhs.y);
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt((float)((double)this.x * (double)this.x + (double)this.y * (double)this.y));
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return (float)((double)this.x * (double)this.x + (double)this.y * (double)this.y);
            }
        }

        public static float Angle(Vec2 from, Vec2 to)
        {
            return (float)Math.Acos(Mathf.Clamp(Vec2.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
        }

        public static float Distance(Vec2 a, Vec2 b)
        {
            return (a - b).magnitude;
        }

        public static Vec2 ClampMagnitude(Vec2 vector, float maxLength)
        {
            if ((double)vector.sqrMagnitude > (double)maxLength * (double)maxLength)
                return vector.normalized * maxLength;
            return vector;
        }

        public static float SqrMagnitude(Vec2 a)
        {
            return (float)((double)a.x * (double)a.x + (double)a.y * (double)a.y);
        }

        public float SqrMagnitude()
        {
            return (float)((double)this.x * (double)this.x + (double)this.y * (double)this.y);
        }

        public static Vec2 Min(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y));
        }

        public static Vec2 Max(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y));
        }

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x + b.x, a.y + b.y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.x - b.x, a.y - b.y);
        }

        public static Vec2 operator -(Vec2 a)
        {
            return new Vec2(-a.x, -a.y);
        }

        public static Vec2 operator *(Vec2 a, float d)
        {
            return new Vec2(a.x * d, a.y * d);
        }

        public static Vec2 operator *(float d, Vec2 a)
        {
            return new Vec2(a.x * d, a.y * d);
        }

        public static Vec2 operator /(Vec2 a, float d)
        {
            return new Vec2(a.x / d, a.y / d);
        }

        public static bool operator ==(Vec2 lhs, Vec2 rhs)
        {
            return (double)(lhs - rhs).sqrMagnitude < 9.99999943962493E-11;
        }

        public static bool operator !=(Vec2 lhs, Vec2 rhs)
        {
            return !(lhs == rhs);
        }

        public static Vec2 zero
        {
            get
            {
                return Vec2.zeroVector;
            }
        }

        public static Vec2 one
        {
            get
            {
                return Vec2.oneVector;
            }
        }

        public static Vec2 up
        {
            get
            {
                return Vec2.upVector;
            }
        }

        public static Vec2 down
        {
            get
            {
                return Vec2.downVector;
            }
        }

        public static Vec2 left
        {
            get
            {
                return Vec2.leftVector;
            }
        }

        public static Vec2 right
        {
            get
            {
                return Vec2.rightVector;
            }
        }

        public static Vec2 positiveInfinity
        {
            get
            {
                return Vec2.positiveInfinityVector;
            }
        }

        public static Vec2 negativeInfinity
        {
            get
            {
                return Vec2.negativeInfinityVector;
            }
        }

        public static implicit operator PointF(Vec2 vector)
        {
            return new PointF(vector.x, vector.y);
        }

        public static explicit operator Vec2(PointF point)
        {
            return new Vec2(point);
        }

        public static implicit operator Point(Vec2 vector)
        {
            return new Point((int)vector.x, (int)vector.y);
        }

        public static explicit operator Vec2(Point point)
        {
            return new Vec2(point);
        }
    }
}
