using System.ComponentModel;

namespace BehaviorTreeEditor
{
    [TypeConverter(typeof(PropertySorter))]
    public class VariableFieldDesigner
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
                Value = null;
                switch (m_FieldType)
                {
                    case FieldType.IntField:
                        Value = new IntFieldDesigner();
                        break;
                    case FieldType.LongField:
                        Value = new LongFieldDesigner();
                        break;
                    case FieldType.FloatField:
                        Value = new FloatFieldDesigner();
                        break;
                    case FieldType.DoubleField:
                        Value = new DoubleFieldDesigner();
                        break;
                    case FieldType.StringField:
                        Value = new StringFieldDesigner();
                        break;
                    case FieldType.BooleanField:
                        Value = new BooleanFieldDesigner();
                        break;
                }
            }
        }

        [Category("常规"), DisplayName("变量值"), Description("变量值"), PropertyOrder(4)]
        public BaseFieldDesigner Value { get; set; }

        public override string ToString()
        {
            return Value == null ? string.Empty : Value.ToString();
        }
    }
}
