using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BehaviorTreeViewer
{
    public class Vector3Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string[] mStrs = value.ToString().Split(' ');

            Vector3 vector = new Vector3();
            vector.X = int.Parse(mStrs[0]);
            vector.Y = int.Parse(mStrs[1]);
            vector.Z = int.Parse(mStrs[2]);
            return vector;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
