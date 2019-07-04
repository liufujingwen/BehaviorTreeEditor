using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
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

        public static bool operator ==(BooleanField lhs, bool rhs)
        {
            return lhs.Value == rhs;
        }

        public static bool operator !=(BooleanField lhs, bool rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator ==(bool lhs, BooleanField rhs)
        {
            return lhs == rhs.Value;
        }

        public static bool operator !=(bool lhs, BooleanField rhs)
        {
            return !(lhs == rhs.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is bool)
            {
                bool field = (bool)other;
                return this.Value.Equals(field);
            }
            else if (other is BooleanField)
            {
                BooleanField field = (BooleanField)other;
                return this.Value.Equals(field.Value);
            }

            return false;
        }

        #endregion

        public override BaseField Clone()
        {
            BooleanField field = new BooleanField();
            field.FieldName = FieldName;
            field.Value = Value;
            return field;
        }
    }
}
