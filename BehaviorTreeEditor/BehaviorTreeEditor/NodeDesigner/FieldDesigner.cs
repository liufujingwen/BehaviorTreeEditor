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
        private int m_ID;

        [Category("常规")]
        [Description("ID")]
        [ReadOnly(true)]
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


        private FieldType m_FieldType;

        [Category("常规")]
        [DisplayName("字段类型")]
        [Description("字段类型")]
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
                    case FieldType.FloatField:
                    case FieldType.DoubleField:
                    case FieldType.ColorField:
                    case FieldType.EnumField:
                    case FieldType.BooleanField:
                    case FieldType.RepeatIntField:
                        Field = new IntFieldDesigner();
                        break;
                    case FieldType.RepeatLongField:
                    case FieldType.RepeatFloatField:
                    case FieldType.RepeatDoubleField:
                        break;
                }
            }
        }

        [Category("常规")]
        [DisplayName("字段详细")]
        [Description("字段详细")]
        public BaseFieldDesigner Field { get; set; }

        public override string ToString()
        {
            return Field == null ? FieldType.ToString() : Field.FieldName + "  " + FieldType.ToString();
        }
    }
}
