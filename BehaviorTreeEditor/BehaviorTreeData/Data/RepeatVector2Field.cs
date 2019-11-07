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
    }
}
