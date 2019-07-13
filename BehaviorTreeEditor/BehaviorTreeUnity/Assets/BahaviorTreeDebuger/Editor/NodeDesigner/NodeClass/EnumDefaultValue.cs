using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class EnumDefaultValue : BaseDefaultValue
    {
        //枚举类型
        public string m_EnumType;

        [Category("常规"), DisplayName("枚举类型"), Description("枚举类型")]
        public string EnumType
        {
            get { return m_EnumType; }
            set
            {
                m_EnumType = value;
                DefaultValue = null;
            }
        }

        [Category("常规"), DisplayName("默认值"), Description("默认值")]
        public string DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue != null ? DefaultValue : string.Empty;
        }
    }
}
