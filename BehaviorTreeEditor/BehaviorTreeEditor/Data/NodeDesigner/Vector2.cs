
using System.ComponentModel;

namespace BehaviorTreeEditor
{
    [TypeConverter(typeof(Vector2Converter))]
    public class Vector2
    {
        private int m_X;
        private int m_Y;

        [DisplayName("X"), DefaultValue(0)]
        public int X { get { return m_X; } set { m_X = value; } }

        [DisplayName("Y"), DefaultValue(0)]
        public int Y { get { return m_Y; } set { m_Y = value; } }

        public Vector2(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public Vector2()
        {
            m_X = m_Y = 0;
        }

        public override string ToString()
        {
            return m_X + " " + m_Y;
        }

        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }
    }
}
