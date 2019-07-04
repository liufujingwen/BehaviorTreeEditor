using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class RepeatLongField : BaseField
    {
        public List<long> Value;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }

        public override BaseField Clone()
        {
            RepeatLongField field = new RepeatLongField();
            field.FieldName = FieldName;
            if (Value != null)
            {
                field.Value = new List<long>(Value.Count);
                for (int i = 0; i < Value.Count; i++)
                {
                    field.Value.Add(Value[i]);
                }
            }
            return field;
        }
    }
}
