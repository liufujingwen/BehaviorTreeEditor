using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class RepeatStringFieldDesigner : BaseFieldDesigner
    {
        private List<string> m_Value = new List<string>();

        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Category("常规"), DisplayName("String数组"), Description("String数组")]
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
