using System.Collections.Generic;
using System.ComponentModel;

namespace BehaviorTreeEditor
{
    public class RepeatLongFieldDesigner : BaseFieldDesigner
    {
        private List<long> m_Value = new List<long>();

        [Category("常规"), DisplayName("Long数组"),Description("Long数组")]
        public List<long> Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public override string ToString()
        {
            string content = string.Empty;
            content += "[";
            for (int i = 0; i < m_Value.Count; i++)
            {
                content += m_Value[i] + (i < m_Value.Count - 1 ? "," : string.Empty);
            }
            content += "]";
            return content;
        }
    }
}
