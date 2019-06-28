using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    [Serializable]
    public class FieldDesigner
    {
        private FieldType m_FieldType;

        [Category("常规"), DisplayName("字段类型"), Description("字段类型")]
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
                        break;
                    case FieldType.ColorField:
                        Field = new ColorFieldDesigner();
                        break;
                    case FieldType.EnumField:
                        Field = new EnumFieldDesigner();
                        break;
                    case FieldType.BooleanField:
                        Field = new BooleanFieldDesigner();
                        break;
                    case FieldType.RepeatIntField:
                        Field = new IntFieldDesigner();
                        break;
                    case FieldType.RepeatLongField:
                        Field = new RepeatLongFieldDesigner();
                        break;
                    case FieldType.RepeatFloatField:
                        Field = new RepeatFloatFieldDesigner();
                        break;
                }
            }
        }

        [Category("常规"), DisplayName("字段详细"), Description("字段详细")]
        public BaseFieldDesigner Field { get; set; }

        public override string ToString()
        {
            return Field == null ? FieldType.ToString() : Field.FieldName + "  " + FieldType.ToString();
        }
    }
}
