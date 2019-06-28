using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public struct Rect
    {
        private float m_XMin;
        private float m_YMin;
        private float m_Width;
        private float m_Height;

        public Rect(float x, float y, float width, float height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public Rect(RectangleF rect)
        {
            this.m_XMin = rect.X;
            this.m_YMin = rect.Y;
            this.m_Width = rect.Width;
            this.m_Height = rect.Height;
        }

        public Rect(Vec2 position, Vec2 size)
        {
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = size.x;
            this.m_Height = size.y;
        }

        public Rect(Rect source)
        {
            this.m_XMin = source.m_XMin;
            this.m_YMin = source.m_YMin;
            this.m_Width = source.m_Width;
            this.m_Height = source.m_Height;
        }

        public static Rect zero
        {
            get
            {
                return new Rect(0.0f, 0.0f, 0.0f, 0.0f);
            }
        }

        public static Rect MinMaxRect(float xmin, float ymin, float xmax, float ymax)
        {
            return new Rect(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        public void Set(float x, float y, float width, float height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public float x
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                this.m_XMin = value;
            }
        }

        public float y
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                this.m_YMin = value;
            }
        }

        public Vec2 position
        {
            get
            {
                return new Vec2(this.m_XMin, this.m_YMin);
            }
            set
            {
                this.m_XMin = value.x;
                this.m_YMin = value.y;
            }
        }

        public Vec2 center
        {
            get
            {
                return new Vec2(this.x + this.m_Width / 2f, this.y + this.m_Height / 2f);
            }
            set
            {
                this.m_XMin = value.x - this.m_Width / 2f;
                this.m_YMin = value.y - this.m_Height / 2f;
            }
        }

        public Vec2 min
        {
            get
            {
                return new Vec2(this.xMin, this.yMin);
            }
            set
            {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }

        public Vec2 max
        {
            get
            {
                return new Vec2(this.xMax, this.yMax);
            }
            set
            {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }

        public float width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }

        public float height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public Vec2 size
        {
            get
            {
                return new Vec2(this.m_Width, this.m_Height);
            }
            set
            {
                this.m_Width = value.x;
                this.m_Height = value.y;
            }
        }

        public float xMin
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                float xMax = this.xMax;
                this.m_XMin = value;
                this.m_Width = xMax - this.m_XMin;
            }
        }

        public float yMin
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                float yMax = this.yMax;
                this.m_YMin = value;
                this.m_Height = yMax - this.m_YMin;
            }
        }

        public float xMax
        {
            get
            {
                return this.m_Width + this.m_XMin;
            }
            set
            {
                this.m_Width = value - this.m_XMin;
            }
        }

        public float yMax
        {
            get
            {
                return this.m_Height + this.m_YMin;
            }
            set
            {
                this.m_Height = value - this.m_YMin;
            }
        }

        public bool Contains(Vec2 point)
        {
            return (double)point.x >= (double)this.xMin && (double)point.x < (double)this.xMax && (double)point.y >= (double)this.yMin && (double)point.y < (double)this.yMax;
        }

        private static Rect OrderMinMax(Rect rect)
        {
            if ((double)rect.xMin > (double)rect.xMax)
            {
                float xMin = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = xMin;
            }
            if ((double)rect.yMin > (double)rect.yMax)
            {
                float yMin = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = yMin;
            }
            return rect;
        }

        public bool Overlaps(Rect other)
        {
            return (double)other.xMax > (double)this.xMin && (double)other.xMin < (double)this.xMax && (double)other.yMax > (double)this.yMin && (double)other.yMin < (double)this.yMax;
        }

        public bool Overlaps(Rect other, bool allowInverse)
        {
            Rect rect = this;
            if (allowInverse)
            {
                rect = Rect.OrderMinMax(rect);
                other = Rect.OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }

        public static Vec2 NormalizedToPoint(Rect rectangle, Vec2 normalizedRectCoordinates)
        {
            return new Vec2(Mathf.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), Mathf.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }

        public static bool operator !=(Rect lhs, Rect rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(Rect lhs, Rect rhs)
        {
            return (double)lhs.x == (double)rhs.x && (double)lhs.y == (double)rhs.y && (double)lhs.width == (double)rhs.width && (double)lhs.height == (double)rhs.height;
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^ this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is Rect))
                return false;
            Rect rect = (Rect)other;
            return this.x.Equals(rect.x) && this.y.Equals(rect.y) && this.width.Equals(rect.width) && this.height.Equals(rect.height);
        }

        public override string ToString()
        {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", (object)this.x, (object)this.y, (object)this.width, (object)this.height);
        }

        public string ToString(string format)
        {
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", (object)this.x.ToString(format), (object)this.y.ToString(format), (object)this.width.ToString(format), (object)this.height.ToString(format));
        }

        public static Rect operator +(Rect a, Vec2 b)
        {
            a.x += b.x;
            a.y += b.y;
            return a;
        }

        public static Rect operator -(Rect a, Vec2 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            return a;
        }

        public static implicit operator RectangleF(Rect rect)
        {
            return new RectangleF(rect.x, rect.y, rect.width, rect.height);
        }

        public static explicit operator Rect(RectangleF point)
        {
            return new Rect(point.X, point.Y, point.Width, point.Height);
        }

        public static implicit operator Rectangle(Rect rect)
        {
            return new Rectangle((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
        }

        public static explicit operator Rect(Rectangle point)
        {
            return new Rect(point);
        }
    }
}
