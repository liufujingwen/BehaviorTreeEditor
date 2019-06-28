using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class RepeatVector3FieldDesigner : BaseFieldDesigner
    {
        private List<List<int>> m_Value = new List<List<int>>();

        [Category("常规"), DisplayName("Vector3数组"), Description("Vector3数组")]
        public List<List<int>> Value
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
                if (i == 0) content += "[";
                content += m_Value[i] + (i < m_Value.Count - 1 ? "," : string.Empty);
                if (i == m_Value.Count - 1) content += "]";

            }
            content += "]";
            return content;
        }

        public override string ToString()
        {
            return "vector3[]";
        }
    }
}
