using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class RepeatFloatFieldDesigner : BaseFieldDesigner
    {
        private List<float> m_Value = new List<float>();

        [Category("常规"), DisplayName("Float数组"), Description("Float数组")]
        public List<float> Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public override string FieldContent()
        {
            string content = FieldName;
            content += ":[";
            for (int i = 0; i < m_Value.Count; i++)
            {
                content += m_Value[i] + (i < m_Value.Count - 1 ? "," : string.Empty);
            }
            content += "]";
            return content;
        }

        public override string ToString()
        {
            return "float[]";
        }
    }
}
