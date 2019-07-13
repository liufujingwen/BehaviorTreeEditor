using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    [TypeConverter(typeof(Vector3Converter))]
    public class Vector3
    {
        private int m_X;
        private int m_Y;
        private int m_Z;

        [DisplayName("X"), DefaultValue(0)]
        public int X { get { return m_X; } set { m_X = value; } }

        [DisplayName("Y"), DefaultValue(0)]
        public int Y { get { return m_Y; } set { m_Y = value; } }

        [DisplayName("Z"), DefaultValue(0)]
        public int Z { get { return m_Z; } set { m_Z = value; } }

        public Vector3(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        public Vector3()
        {
            m_X = m_Y = m_Z = 0;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1},{2}]", m_X, m_Y, m_Z);
        }

        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }
    }
}
