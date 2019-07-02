using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class EnumTypeConverter : TypeConverter
    {
        private NodeClasses m_NodeClasses = null;
        private List<string> m_EnumTypeList = new List<string>();

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            m_NodeClasses = MainForm.Instance.NodeClasses;

            if (m_NodeClasses != null)
            {
                if (m_NodeClasses.Enums.Count == 0)
                {
                    MainForm.Instance.ShowMessage("没有任何枚举,请先注册枚举");
                }
                else
                {
                    m_EnumTypeList.Clear();
                    for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
                    {
                        CustomEnum customEnum = m_NodeClasses.Enums[i];
                        if (customEnum == null)
                            continue;
                        m_EnumTypeList.Add(customEnum.EnumType);
                    }

                    return new StandardValuesCollection(m_EnumTypeList);
                }
            }

            return base.GetStandardValues(context);
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
