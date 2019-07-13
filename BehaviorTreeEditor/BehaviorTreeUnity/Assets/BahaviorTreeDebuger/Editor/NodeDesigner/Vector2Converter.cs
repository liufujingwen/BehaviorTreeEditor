using System;
using System.ComponentModel;

namespace BehaviorTreeViewer
{
    public class Vector2Converter : TypeConverter
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

            Vector2 vector = new Vector2();
            vector.X = int.Parse(mStrs[0]);
            vector.Y = int.Parse(mStrs[1]);
            return vector;
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}