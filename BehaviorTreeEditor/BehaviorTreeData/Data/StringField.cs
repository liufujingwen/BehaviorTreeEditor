using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class StringField : BaseField
    {
        public string Value;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }

        #region operator

        public static implicit operator string(StringField field)
        {
            return field.Value;
        }

        public static explicit operator StringField(string value)
        {
            return new StringField { Value = value };
        }

        public static bool operator ==(StringField lhs, string rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator !=(StringField lhs, string rhs)
        {
            return !(lhs.Value == rhs);
        }

        public static bool operator ==(string lhs, StringField rhs)
        {
            return lhs == rhs.Value;
        }

        public static bool operator !=(string lhs, StringField rhs)
        {
            return !(lhs == rhs.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is string)
            {
                string field = (string)other;
                return this.Value.Equals(field);
            }
            else if (other is StringField)
            {
                StringField field = (StringField)other;
                return this.Value.Equals(field.Value);
            }
            return false;
        }

        #endregion
    }
}
