using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class FieldDesigner
    {
        private FieldType m_FieldType;
        public BaseFieldDesigner Field { get; set; }

        public FieldType FieldType
        {
            get { return m_FieldType; }
            set
            {
                m_FieldType = value;
                Field = null;
                switch (m_FieldType)
                {
                    case FieldType.IntFieldType:
                        Field = new IntFieldDesigner();
                        break;
                    case FieldType.LongFieldType:
                    case FieldType.FloatFieldType:
                    case FieldType.DoubleFieldType:
                    case FieldType.ColorFieldType:
                    case FieldType.EnumFieldType:
                    case FieldType.BooleanFieldType:
                    case FieldType.RepeatIntFieldType:
                        Field = new IntFieldDesigner();
                        break;
                    case FieldType.RepeatLongFieldType:
                    case FieldType.RepeatFloatFieldType:
                    case FieldType.RepeatDoubleFieldType:
                        break;
                }
            }
        }

    }
}
