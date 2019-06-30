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

        public bool ExistEnumStr(string enumStr, EnumItem ignore = null)
        {
            if (string.IsNullOrEmpty(enumStr))
                throw new Exception("CustomEnum.ExistEnumItem() error: enumStr = null");

            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (ignore != null && ignore == item)
                    continue;
                if (item.EnumStr == enumStr)
                    return true;
            }
            return false;
        }

        public bool ExistEnumValue(int enumValue, EnumItem ignore = null)
        {
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem item = m_Enums[i];
                if (item == null)
                    continue;
                if (ignore != null && ignore == item)
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

        /// <summary>
        /// 是否存在空的枚举选项名
        /// </summary>
        /// <returns></returns>
        public bool ExistEmptyEnumStr()
        {
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem enumItem = m_Enums[i];
                if (string.IsNullOrEmpty(enumItem.EnumStr))
                {
                    MainForm.Instance.ShowMessage("存在空枚举选项名");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存相同枚举值
        /// </summary>
        /// <returns></returns>
        public bool ExistSameEnumValue()
        {
            //检测EnumValue是否存在相同
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem enum_i = m_Enums[i];
                for (int ii = i + 1; ii < m_Enums.Count; ii++)
                {
                    EnumItem enum_ii = m_Enums[ii];
                    if (enum_i.EnumValue == enum_ii.EnumValue)
                    {
                        MainForm.Instance.ShowMessage(string.Format("存在重复枚举值:{0}", enum_ii.EnumValue));
                        MainForm.Instance.ShowInfo(string.Format("存在重复枚举值:{0} 时间:{1}", enum_ii.EnumValue, DateTime.Now));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否存在相同枚举选项名
        /// </summary>
        /// <returns></returns>
        public bool ExistSameEnumStr()
        {
            //检测EnumStr是否重复
            for (int i = 0; i < m_Enums.Count; i++)
            {
                EnumItem enum_i = m_Enums[i];
                for (int ii = i + 1; ii < m_Enums.Count; ii++)
                {
                    EnumItem enum_ii = m_Enums[ii];
                    if (enum_i.EnumStr == enum_ii.EnumStr)
                    {
                        MainForm.Instance.ShowMessage(string.Format("存在重复枚举选项:{0}", enum_ii.EnumStr));
                        MainForm.Instance.ShowInfo(string.Format("存在重复枚举选项:{0} 时间:{1}", enum_ii.EnumStr, DateTime.Now));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 更新美剧内容
        /// </summary>
        /// <param name="customEnum"></param>
        public void UpdateEnum(CustomEnum customEnum)
        {
            if (customEnum == null)
                return;

            if (customEnum == this)
                return;

            m_EnumType = customEnum.EnumType;
            m_Describe = customEnum.Describe;

            m_Enums.Clear();
            m_Enums.AddRange(customEnum.Enums);
        }
    }

    public class EnumItem
    {
        [Category("常规"), DisplayName("枚举字符"), Description()]
        public string EnumStr { get; set; }

        [Category("常规"), DisplayName("枚举值"), Description("枚举选项对应的值")]
        public int EnumValue { get; set; }

        [Category("常规"), DisplayName("描述"), Description("描述该枚举项是什么")]
        public string Describe { get; set; }

        public override string ToString()
        {
            return EnumStr + " " + EnumValue;
        }
    }
}
