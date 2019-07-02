using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class EnumDeaultValue : BaseDefaultValue
    {
        public EnumDeaultValue()
        {
            if (MainForm.Instance.NodeClasses == null)
                return;

            if (MainForm.Instance.NodeClasses.Enums.Count > 0)
            {
                CustomEnum customEnum = MainForm.Instance.NodeClasses.Enums[0];
                EnumType = customEnum.EnumType;
            }
        }

        //枚举类型
        public string m_EnumType;

        [TypeConverter(typeof(EnumTypeConverter))]
        [Category("常规"), DisplayName("枚举类型"), Description("枚举类型")]
        public string EnumType
        {
            get { return m_EnumType; }
            set
            {
                m_EnumType = value;
                DefaultValue = null;

                if (MainForm.Instance.NodeClasses == null)
                    return;

                CustomEnum customEnum = MainForm.Instance.NodeClasses.FindEnum(m_EnumType);
                if (customEnum != null)
                {
                    EnumItem defaultEnumItem = customEnum.GetDefaultEnumItem();
                    if (defaultEnumItem != null)
                        DefaultValue = defaultEnumItem.EnumStr;
                }
            }
        }

        [TypeConverter(typeof(EnumItemTypeConverter))]
        [Category("常规"), DisplayName("默认值"), Description("默认值")]
        public string DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue != null ? DefaultValue : string.Empty;
        }
    }
}
