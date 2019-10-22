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

        public static implicit operator bool(BaseField field)
        {
            return (field as BooleanField).Value;
        }

        public static bool operator ==(BaseField lhs, bool rhs)
        {
            if (!(lhs is BooleanField))
                return false;

            return (lhs as BooleanField) == rhs;
        }

        public static bool operator !=(BaseField lhs, bool rhs)
        {
            if (!(lhs is BooleanField))
                return false;

            return !((lhs as BooleanField) == rhs);
        }

        public static bool operator ==(bool lhs, BaseField rhs)
        {
            if (!(rhs is BooleanField))
                return false;

            return (rhs as BooleanField) == lhs;
        }

        public static bool operator !=(bool lhs, BaseField rhs)
        {
            if (!(rhs is BooleanField))
                return false;

            return !((rhs as BooleanField) == lhs);
        }

        //double
        public static bool operator ==(BaseField lhs, double rhs)
        {
            if (!(lhs is DoubleField))
                return false;

            return (lhs as DoubleField) == rhs;
        }

        public static bool operator !=(BaseField lhs, double rhs)
        {
            if (!(lhs is DoubleField))
                return false;

            return !((lhs as DoubleField) == rhs);
        }

        public static bool operator ==(double lhs, BaseField rhs)
        {
            if (!(rhs is DoubleField))
                return false;

            return (rhs as DoubleField) == lhs;
        }

        public static bool operator !=(double lhs, BaseField rhs)
        {
            if (!(rhs is DoubleField))
                return false;

            return !((rhs as DoubleField) == lhs);
        }


        //float
        public static bool operator ==(BaseField lhs, float rhs)
        {
            if (!(lhs is FloatField))
                return false;

            return (lhs as FloatField) == rhs;
        }

        public static bool operator !=(BaseField lhs, float rhs)
        {
            if (!(lhs is FloatField))
                return false;

            return !((lhs as FloatField) == rhs);
        }

        public static bool operator ==(float lhs, BaseField rhs)
        {
            if (!(rhs is FloatField))
                return false;

            return (rhs as FloatField) == lhs;
        }

        public static bool operator !=(float lhs, BaseField rhs)
        {
            if (!(rhs is FloatField))
                return false;

            return !((rhs as FloatField) == lhs);
        }

        //int
        public static bool operator ==(BaseField lhs, int rhs)
        {
            if (!(lhs is IntField))
                return false;

            return (lhs as IntField) == rhs;
        }

        public static bool operator !=(BaseField lhs, int rhs)
        {
            if (!(lhs is IntField))
                return false;

            return !((lhs as IntField) == rhs);
        }

        public static bool operator ==(int lhs, BaseField rhs)
        {
            if (!(rhs is IntField))
                return false;

            return (rhs as IntField) == lhs;
        }

        public static bool operator !=(int lhs, BaseField rhs)
        {
            if (!(rhs is IntField))
                return false;

            return !((rhs as IntField) == lhs);
        }

        //long
        public static bool operator ==(BaseField lhs, long rhs)
        {
            if (!(lhs is LongField))
                return false;

            return (lhs as LongField) == rhs;
        }

        public static bool operator !=(BaseField lhs, long rhs)
        {
            if (!(lhs is LongField))
                return false;

            return !((lhs as LongField) == rhs);
        }

        public static bool operator ==(long lhs, BaseField rhs)
        {
            if (!(rhs is LongField))
                return false;

            return (rhs as LongField) == lhs;
        }

        public static bool operator !=(long lhs, BaseField rhs)
        {
            if (!(rhs is LongField))
                return false;

            return !((rhs as LongField) == lhs);
        }

        //string
        public static bool operator ==(BaseField lhs, string rhs)
        {
            if (!(lhs is StringField))
                return false;

            return (lhs as StringField) == rhs;
        }

        public static bool operator !=(BaseField lhs, string rhs)
        {
            if (!(lhs is StringField))
                return false;

            return !((lhs as StringField) == rhs);
        }

        public static bool operator ==(string lhs, BaseField rhs)
        {
            if (!(rhs is StringField))
                return false;

            return (rhs as StringField) == lhs;
        }

        public static bool operator !=(string lhs, BaseField rhs)
        {
            if (!(rhs is StringField))
                return false;

            return !((rhs as StringField) == lhs);
        }

        #endregion

        public abstract BaseField Clone();
    }
}
