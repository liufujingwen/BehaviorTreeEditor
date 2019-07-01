using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BehaviorTreeEditor
{
    public class RepeatVector2FieldDesigner : BaseFieldDesigner
    {
        private List<Vector2> m_Value = new List<Vector2>();

        [Category("常规"), DisplayName("Vector2数组"), Description("Vector2数组")]
        public List<Vector2> Value
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
