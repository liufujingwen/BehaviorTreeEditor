using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class NodeField
    {
        [Category("常规"), DisplayName("字段名称"), Description("字段名称")]
        public string FieldName { get; set; }

        [Category("常规"), DisplayName("标签"), Description("标签")]
        public string Label { get; set; }

        [Browsable(false)]
        public string Title
        {
            get { return !string.IsNullOrEmpty(Label) ? Label : FieldName; }
        }

        private FieldType m_FieldType;

        [Category("常规"), DisplayName("字段类型"), Description("字段类型")]
        public FieldType FieldType
        {
            get { return m_FieldType; }
            set { m_FieldType = value; }
        }

        [Category("常规"), DisplayName("描述"), Description("描述")]
        public string Describe { get; set; }

        [Category("常规"), DisplayName("默认值"), Description("默认值")]
        public BaseDefaultValue DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue == null ? string.Empty : DefaultValue.ToString();
        }
    }
}
