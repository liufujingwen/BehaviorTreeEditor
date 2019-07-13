using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BehaviorTreeViewer
{
    public class RepeatVector3FieldDesigner : BaseFieldDesigner
    {
        private List<Vector3> m_Value = new List<Vector3>();

        [Category("常规"), DisplayName("Vector3数组"), Description("Vector3数组")]
        public List<Vector3> Value
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
