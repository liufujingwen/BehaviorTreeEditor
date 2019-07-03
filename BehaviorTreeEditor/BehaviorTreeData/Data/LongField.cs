using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class LongField : BaseField
    {
        public long Value;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }

        #region operator

        public static implicit operator long(LongField field)
        {
            return field.Value;
        }

        public static explicit operator LongField(long value)
        {
            return new LongField { Value = value };
        }

        public static bool operator ==(LongField lhs, long rhs)
        {
            return lhs.Value == rhs;
        }

        public static bool operator !=(LongField lhs, long rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator ==(long lhs, LongField rhs)
        {
            return lhs == rhs.Value;
        }

        public static bool operator !=(long lhs, LongField rhs)
        {
            return !(lhs == rhs.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is long)
            {
                long field = (long)other;
                return this.Value.Equals(field);
            }
            else if (other is LongField)
            {
                LongField field = (LongField)other;
                return this.Value.Equals(field.Value);
            }

            return false;
        }

        #endregion
    }
}
