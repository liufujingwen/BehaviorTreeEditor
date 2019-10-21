using System.ComponentModel;

namespace BehaviorTreeEditor
{
    [TypeConverter(typeof(PropertySorter))]
    public class VariableField
    {
        [Category("常规"), DisplayName("变量名称"), Description("变量名称"), PropertyOrder(1)]
        public string VariableFieldName { get; set; }

        private FieldType m_FieldType = FieldType.None;

        [Category("常规"), DisplayName("描述"), Description("描述"), PropertyOrder(2)]
        public string Describe { get; set; }

        [Category("常规"), DisplayName("变量类型"), Description("变量类型"), PropertyOrder(3)]
        public FieldType VariableFieldType
        {
            get { return m_FieldType; }
            set
            {
                m_FieldType = value;
                DefaultValue = null;
                switch (m_FieldType)
                {
                    case FieldType.IntField:
                        DefaultValue = new IntDefaultValue();
                        break;
                    case FieldType.LongField:
                        DefaultValue = new LongDefaultValue();
                        break;
                    case FieldType.FloatField:
                        DefaultValue = new FloatDefaultValue();
                        break;
                    case FieldType.DoubleField:
                        DefaultValue = new DoubleDefaultValue();
                        break;
                    case FieldType.StringField:
                        DefaultValue = new StringDefaultValue();
                        break;
                    case FieldType.BooleanField:
                        DefaultValue = new BooleanDefaultValue();
                        break;
                   
                }
            }
        }

        [Category("常规"), DisplayName("变量默认值"), Description("变量默认值"), PropertyOrder(4)]
        public BaseDefaultValue DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue == null ? string.Empty : DefaultValue.ToString();
        }
    }
}
