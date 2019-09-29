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

        #endregion
    }
}
