using System;
using System.ComponentModel;

namespace BehaviorTreeEditor
{
    [TypeConverter(typeof(PropertySorter))]
    public class FieldDesigner
    {
        [Category("常规"), DisplayName("字段名称"), Description("字段名称"), PropertyOrder(1)]
        public string FieldName { get; set; }

        [Category("常规"), DisplayName("标签"), Description("标签"), PropertyOrder(1)]
        public string Label { get; set; }

        [Browsable(false)]
        public string Title { get { return string.IsNullOrEmpty(Label) ? FieldName : Label; } }

        private FieldType m_FieldType;

        [Category("常规"), DisplayName("字段类型"), Description("字段类型"), PropertyOrder(2)]
        public FieldType FieldType
        {
            get { return m_FieldType; }
            set
            {
                m_FieldType = value;
                Field = null;
                switch (m_FieldType)
                {
                    case FieldType.IntField:
                        Field = new IntFieldDesigner();
                        break;
                    case FieldType.LongField:
                        Field = new LongFieldDesigner();
                        break;
                    case FieldType.FloatField:
                        Field = new FloatFieldDesigner();
                        break;
                    case FieldType.DoubleField:
                        Field = new DoubleFieldDesigner();
                        break;
                    case FieldType.StringField:
                        Field = new StringFieldDesigner();
                        break;
                    case FieldType.ColorField:
                        Field = new ColorFieldDesigner();
                        break;
                    case FieldType.Vector2:
                        Field = new Vector2FieldDesigner();
                        break;
                    case FieldType.Vector3:
                        Field = new Vector3FieldDesigner();
                        break;
                    case FieldType.EnumField:
                        Field = new EnumFieldDesigner();
                        break;
                    case FieldType.BooleanField:
                        Field = new BooleanFieldDesigner();
                        break;
                    case FieldType.RepeatIntField:
                        Field = new RepeatIntFieldDesigner();
                        break;
                    case FieldType.RepeatLongField:
                        Field = new RepeatLongFieldDesigner();
                        break;
                    case FieldType.RepeatFloatField:
                        Field = new RepeatFloatFieldDesigner();
                        break;
                    case FieldType.RepeatVector2Field:
                        Field = new RepeatVector2FieldDesigner();
                        break;
                    case FieldType.RepeatVector3Field:
                        Field = new RepeatVector3FieldDesigner();
                        break;
                    case FieldType.RepeatStringField:
                        Field = new RepeatStringFieldDesigner();
                        break;
                }
            }
        }

        [Category("常规"), DisplayName("描述"), Description("描述"), PropertyOrder(3)]
        public string Describe { get; set; }

        [Category("常规"), DisplayName("字段详细"), Description("字段详细"), PropertyOrder(4)]
        public BaseFieldDesigner Field { get; set; }

        public override string ToString()
        {
            return Field == null ? FieldType.ToString() : FieldName + "  " + Field;
        }
    }
}