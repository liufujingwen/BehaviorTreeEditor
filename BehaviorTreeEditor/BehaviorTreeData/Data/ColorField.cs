using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class ColorField : BaseField
    {
        public int Value;

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
            ColorField field = new ColorField();
            field.FieldName = FieldName;
            field.Value = Value;
            return field;
        }
    }
}
