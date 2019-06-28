using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class EnumFieldDesigner : BaseFieldDesigner
    {
        private int m_Value;
        private List<string> m_EnumStrs = new List<string>();

        [Category("常规"), DisplayName("枚举值"), Description("当前选中的枚举值")]
        public int Value
        {
            get { return m_Value; }
            set { m_Value = Value; }
        }

        [Category("常规"), DisplayName("枚举值数组"), Description("枚举值数组")]
        public List<string> EnumStrs
        {
            get { return m_EnumStrs; }
            set { m_EnumStrs = value; }
        }

        public override string FieldContent()
        {
            string content = FieldName + ":";
            if (m_Value >= 0 && m_Value < m_EnumStrs.Count - 1)
            {
                content += m_EnumStrs[m_Value];
            }
            else
            {
                content += "无效值";
            }

            return content;
        }

        public override string ToString()
        {
            return "enum";
        }
    }
}
