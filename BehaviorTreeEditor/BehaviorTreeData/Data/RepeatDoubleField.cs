using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class RepeatDoubleField : BaseField
    {
        public List<double> Value;

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
            RepeatDoubleField field = new RepeatDoubleField();
            field.FieldName = FieldName;
            if (Value != null)
            {
                field.Value = new List<double>(Value.Count);
                for (int i = 0; i < Value.Count; i++)
                {
                    field.Value.Add(Value[i]);
                }
            }
            return field;
        }
    }
}
