using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    [TypeConverter(typeof(PropertySorter))]
    public class NodeField
    {
        [Category("常规"), DisplayName("字段名称"), Description("字段名称"), PropertyOrder(1)]
        public string FieldName { get; set; }

        private FieldType m_FieldType;

        [Category("常规"), DisplayName("字段类型"), Description("字段类型"), PropertyOrder(2)]
        public FieldType FieldType
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
                    case FieldType.ColorField:
                        DefaultValue = new ColorDefaultValue();
                        break;
                    case FieldType.Vector2:
                        DefaultValue = new Vector2DefaultValue();
                        break;
                    case FieldType.Vector3:
                        DefaultValue = new Vector3DefaultValue();
                        break;
                    case FieldType.EnumField:
                        DefaultValue = new EnumDefaultValue();
                        break;
                    case FieldType.BooleanField:
                        DefaultValue = new BooleanDefaultValue();
                        break;
                    case FieldType.RepeatIntField:
                        break;
                    case FieldType.RepeatLongField:
                        break;
                    case FieldType.RepeatFloatField:
                        break;
                    case FieldType.RepeatVector2Field:
                        break;
                    case FieldType.RepeatVector3Field:
                        break;
                    case FieldType.RepeatStringField:
                        break;
                }
            }
        }

        [Category("常规"), DisplayName("描述"), Description("描述"), PropertyOrder(3)]
        public string Describe { get; set; }

        [Category("常规"), DisplayName("默认值"), Description("默认值"), PropertyOrder(4)]
        public BaseDefaultValue DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue == null ? string.Empty : DefaultValue.ToString();
        }
    }
}
