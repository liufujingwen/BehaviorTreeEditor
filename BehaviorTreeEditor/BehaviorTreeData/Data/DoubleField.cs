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

        public static bool operator ==(DoubleField lhs, double rhs)
        {
            return lhs.Value == rhs;
        }

        public static bool operator !=(DoubleField lhs, double rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator ==(double lhs, DoubleField rhs)
        {
            return lhs == rhs.Value;
        }

        public static bool operator !=(double lhs, DoubleField rhs)
        {
            return !(lhs == rhs.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is double)
            {
                double field = (double)other;
                return this.Value.Equals(field);
            }
            else if (other is DoubleField)
            {
                DoubleField field = (DoubleField)other;
                return this.Value.Equals(field.Value);
            }

            return false;
        }

        #endregion

        public override BaseField Clone()
        {
            DoubleField field = new DoubleField();
            field.FieldName = FieldName;
            field.Value = Value;
            return field;
        }
    }
}
