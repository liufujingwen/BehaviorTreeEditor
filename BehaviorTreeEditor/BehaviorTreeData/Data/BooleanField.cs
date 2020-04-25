using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public class BooleanField : BaseField
    {
        public bool Value;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }

        #region operator

        public static implicit operator bool(BooleanField field)
        {
            return field.Value;
        }

        public static explicit operator BooleanField(bool value)
        {
            return new BooleanField { Value = value };
        }

        #endregion
    }
}
