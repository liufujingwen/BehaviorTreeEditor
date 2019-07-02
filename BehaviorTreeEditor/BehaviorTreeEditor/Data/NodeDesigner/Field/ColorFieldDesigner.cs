using System.ComponentModel;

namespace BehaviorTreeEditor
{
    public class ColorFieldDesigner : BaseFieldDesigner
    {
        public ColorFieldDesigner()
        {
            m_r = 255;
            m_g = 255;
            m_b = 255;
            m_a = 255;
        }

        private int m_r;
        private int m_g;
        private int m_b;
        private int m_a;

        [Category("常规"), DisplayName("R默认值"), Description("R默认值")]
        public int R
        {
            get { return m_r; }
            set
            {
                m_r = value;
                m_r = Mathf.Clamp(m_r, 0, 255);
            }
        }

        [Category("常规"), DisplayName("G默认值"), Description("G默认值")]
        public int G
        {
            get { return m_g; }
            set
            {
                m_g = value;
                m_g = Mathf.Clamp(m_g, 0, 255);
            }
        }

        [Category("常规"), DisplayName("B默认值"), Description("B默认值")]
        public int B
        {
            get { return m_b; }
            set
            {
                m_b = value;
                m_b = Mathf.Clamp(m_b, 0, 255);
            }
        }

        [Category("常规"), DisplayName("A默认值"), Description("A默认值")]
        public int A
        {
            get { return m_a; }
            set
            {
                m_a = value;
                m_a = Mathf.Clamp(m_a, 0, 255);
            }
        }

        public override string ToString()
        {
            return string.Format("[R:{0},G:{1},B:{2},A:{3}]", m_r, m_g, m_b, m_a);
        }
    }
}
