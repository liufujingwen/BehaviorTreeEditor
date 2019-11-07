using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class DoubleField : BaseField
    {
        public double Value;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }

        #region operator

        public static implicit operator double(DoubleField field)
        {
            return field.Value;
        }

        public static explicit operator DoubleField(double value)
        {
            return new DoubleField { Value = value };
        }

        #endregion
    }
}
