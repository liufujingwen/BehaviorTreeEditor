using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class RepeatStringFieldDesigner : BaseFieldDesigner
    {
        private List<string> m_Value = new List<string>();

        public List<string> Value
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
                string temp = m_Value[i];
                content += string.IsNullOrEmpty(temp) ? string.Empty : temp;
                content += i < m_Value.Count - 1 ? "," : string.Empty;
            }
            content += "]";
            return content;
        }
    }
}
