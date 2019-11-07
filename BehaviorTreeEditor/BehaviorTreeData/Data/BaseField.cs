using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public abstract class BaseField : Binary
    {
        public string FieldName;

        #region operator

        public static implicit operator int(BaseField field)
        {
            return (field as IntField).Value;
        }

        public static implicit operator List<int>(BaseField field)
        {
            return (field as RepeatIntField).Value;
        }

        public static implicit operator float(BaseField field)
        {
            return (field as FloatField).Value;
        }

        public static implicit operator List<float>(BaseField field)
        {
            return (field as RepeatFloatField).Value;
        }

        public static implicit operator double(BaseField field)
        {
            return (field as DoubleField).Value;
        }

        public static implicit operator List<double>(BaseField field)
        {
            return (field as RepeatDoubleField).Value;
        }

        public static implicit operator long(BaseField field)
        {
            return (field as LongField).Value;
        }

        public static implicit operator List<long>(BaseField field)
        {
            return (field as RepeatLongField).Value;
        }

        public static implicit operator string(BaseField field)
        {
            return (field as StringField).Value;
        }

        public static implicit operator bool(BaseField field)
        {
            return (field as BooleanField).Value;
        }

        #endregion
    }
}
