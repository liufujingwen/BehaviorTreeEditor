using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class CustomEnum
    {
        private string m_EnumType = string.Empty;

        public string EnumType
        {
            get { return m_EnumType; }
            set { m_EnumType = value; }
        }

        private List<EnumItem> m_Enums = new List<EnumItem>();

        public List<EnumItem> Enums
        {
            get { return m_Enums; }
            set { m_Enums = value; }
        }

        private string m_Describe;

        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }
    }

    public class EnumItem
    {
        public string EnumStr { get; set; }

        public int EnumValue { get; set; }

        public string Describe { get; set; }

        public override string ToString()
        {
            return EnumStr + " " + EnumValue;
        }
    }
}
