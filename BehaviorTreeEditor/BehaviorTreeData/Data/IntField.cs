using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class IntField : BaseField
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

        #region operator

        public static implicit operator int(IntField field)
        {
            return field.Value;
        }

        public static explicit operator IntField(int value)
        {
            return new IntField { Value = value };
        }

        public static bool operator ==(IntField lhs, int rhs)
        {
            return lhs.Value == rhs;
        }

        public static bool operator !=(IntField lhs, int rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator ==(int lhs, IntField rhs)
        {
            return lhs == rhs.Value;
        }

        public static bool operator !=(int lhs, IntField rhs)
        {
            return !(lhs == rhs.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is int)
            {
                int field = (int)other;
                return this.Value.Equals(field);
            }
            else if (other is IntField)
            {
                IntField field = (IntField)other;
                return this.Value.Equals(field.Value);
            }

            return false;
        }

        #endregion

        public override BaseField Clone()
        {
            IntField field = new IntField();
            field.FieldName = FieldName;
            field.Value = Value;
            return field;
        }
    }
}
