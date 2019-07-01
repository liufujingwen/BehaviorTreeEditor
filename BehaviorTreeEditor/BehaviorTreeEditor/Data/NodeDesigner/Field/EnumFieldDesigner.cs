using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace BehaviorTreeEditor
{
    public class EnumFieldDesigner : BaseFieldDesigner
    {
        private string m_EnumType;
        private string m_Value;

        [Editor(typeof(EnumTypeUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("常规"), DisplayName("枚举类型"), Description("枚举类型")]
        public string EnumType
        {
            get { return m_EnumType; }
            set { m_EnumType = value; }
        }

        [Editor(typeof(EnumItemUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("常规"), DisplayName("枚举值"), Description("当前选中的枚举值")]
        public string Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(m_Value) ? string.Empty : m_Value;
        }
    }
}
