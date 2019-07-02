using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class EnumItemTypeConverter : TypeConverter
    {
        private CustomEnum m_CustomEnum;
        private List<string> m_EnumItemList = new List<string>();
        private string m_EnumStr;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            m_EnumStr = null;
            if (context.Instance is EnumDeaultValue)
            {
                EnumDeaultValue enumDeaultValue = context.Instance as EnumDeaultValue;
                if (enumDeaultValue != null)
                {
                    m_EnumStr = enumDeaultValue.DefaultValue;
                    if (string.IsNullOrEmpty(enumDeaultValue.EnumType))
                    {
                        MainForm.Instance.ShowMessage("请先选择枚举类型");
                    }
                    else
                    {
                        m_CustomEnum = MainForm.Instance.NodeClasses.FindEnum(enumDeaultValue.EnumType);
                        if (m_CustomEnum == null)
                        {
                            MainForm.Instance.ShowMessage(string.Format("不存在枚举类型:{0},请先注册", enumDeaultValue.EnumType));
                        }
                    }
                }
            }
            else if (context.Instance is EnumFieldDesigner)
            {
                EnumFieldDesigner enumFieldDesigner = context.Instance as EnumFieldDesigner;
                if (enumFieldDesigner != null)
                {
                    m_EnumStr = enumFieldDesigner.Value;
                    if (string.IsNullOrEmpty(enumFieldDesigner.EnumType))
                    {
                        MainForm.Instance.ShowMessage("请先选择枚举类型");
                    }
                    else
                    {
                        m_CustomEnum = MainForm.Instance.NodeClasses.FindEnum(enumFieldDesigner.EnumType);
                        if (m_CustomEnum == null)
                        {
                            MainForm.Instance.ShowMessage(string.Format("不存在枚举类型:{0},请先注册", enumFieldDesigner.EnumType));
                        }
                    }
                }
            }

            if (m_CustomEnum != null)
            {
                //绑定
                m_EnumItemList.Clear();
                if (m_CustomEnum.Enums.Count > 0)
                {
                    int selectedIndex = 0;

                    for (int i = 0; i < m_CustomEnum.Enums.Count; i++)
                    {
                        EnumItem enumItem = m_CustomEnum.Enums[i];
                        m_EnumItemList.Add(enumItem.EnumStr);
                        if (m_EnumStr == enumItem.EnumStr)
                            selectedIndex = i;
                    }
                }

                return new StandardValuesCollection(m_EnumItemList);
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
