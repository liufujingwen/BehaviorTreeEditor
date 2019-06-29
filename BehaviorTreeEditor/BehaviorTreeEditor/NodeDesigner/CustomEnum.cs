using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class CustomEnum
    {
        private string m_EnumType = string.Empty;
        [Category("常规"), DisplayName("枚举类型"), Description("枚举的唯一标识")]
        public string EnumType
        {
            get { return m_EnumType; }
            set { m_EnumType = value; }
        }

        private List<EnumItem> m_Enums = new List<EnumItem>();

        [Category("常规"), DisplayName("枚举选项"), Description()]
        public List<EnumItem> Enums
        {
            get { return m_Enums; }
            set { m_Enums = value; }
        }

        private string m_Describe;

        [Category("常规"), DisplayName("描述"), Description("描述该枚举作用是什么")]
        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        public bool ExistEnumStr(string enumStr)
        {
            if (string.IsNullOrEmpty(enumStr))
                throw new Exception("CustomEnum.ExistEnumItem() error: enumStr = null");

            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (item.EnumStr == enumStr)
                    return true;
            }
            return false;
        }

        public bool ExistEnumValue(int enumValue)
        {
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (item.EnumValue == enumValue)
                    return true;
            }
            return false;
        }

        public EnumItem FindEnum(string enumStr)
        {
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (item.EnumStr == enumStr)
                    return item;
            }
            return null;
        }

        public bool AddEnumItem(EnumItem item)
        {
            if (item == null)
                return false;

            if (ExistEnumStr(item.EnumStr))
            {
                MainForm.Instance.ShowMessage(string.Format("枚举类型:{0},已存在枚举字符:{1}", m_EnumType, item.EnumStr));
                return false;
            }

            if (ExistEnumValue(item.EnumValue))
            {
                EnumItem findItem = FindEnum(item.EnumStr);
                MainForm.Instance.ShowMessage(string.Format("枚举类型:{0},已存在枚举值[{1}:{2}],请修改枚举值", m_EnumType, findItem.EnumStr, findItem.EnumValue));
                return false;
            }

            m_Enums.Add(item);

            return true;
        }

        public bool Remove(string enumStr)
        {
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (item.EnumStr == enumStr)
                {
                    m_Enums.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Check()
        {
            for (int i = m_Enums.Count - 1; i >= 0; i--)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;

                if (string.IsNullOrEmpty(item.EnumStr))
                    m_Enums.RemoveAt(i);
            }
        }

        public void Clear()
        {
            m_Enums.Clear();
        }

        public override string ToString()
        {
            string content = m_EnumType + ":[";
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                content += item.EnumStr + (i < m_Enums.Count - 1 ? "," : string.Empty);
            }
            content += "]";
            return content;
        }
    }

    public class EnumItem
    {
        [Category("常规"), DisplayName("枚举字符"), Description()]
        public string EnumStr { get; set; }

        [Category("常规"), DisplayName("枚举值"), Description("枚举选项对应的值")]
        public int EnumValue { get; set; }

        public override string ToString()
        {
            return EnumStr + " " + EnumValue;
        }
    }
}
