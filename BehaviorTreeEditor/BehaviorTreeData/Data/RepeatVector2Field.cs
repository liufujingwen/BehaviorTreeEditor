using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public class RepeatVector2Field : BaseField
    {
        public List<Vector2> Value = new List<Vector2>();

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
            RepeatVector2Field field = new RepeatVector2Field();
            field.FieldName = FieldName;
            if (Value != null)
            {
                field.Value = new List<Vector2>(Value.Count);
                for (int i = 0; i < Value.Count; i++)
                {
                    Vector2 vector = Value[i];
                    field.Value.Add(vector != null ? vector.Clone() : null);
                }
            }
            return field;
        }
    }
}
