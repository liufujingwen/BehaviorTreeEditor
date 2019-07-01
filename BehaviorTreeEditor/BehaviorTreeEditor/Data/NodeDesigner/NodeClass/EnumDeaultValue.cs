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
                if (customEnum.Enums.Count > 0)
                {
                    EnumItem enumItem = customEnum.Enums[0];
                    DefaultValue = enumItem.EnumStr;
                }
            }
        }

        [Editor(typeof(EnumTypeUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("常规"), DisplayName("枚举类型"), Description("枚举类型")]
        public string EnumType { get; set; }

        [Editor(typeof(EnumItemUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Category("常规"), DisplayName("默认值"), Description("默认值")]
        public string DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue != null ? DefaultValue : string.Empty;
        }
    }
}
