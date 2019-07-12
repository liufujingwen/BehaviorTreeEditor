using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BehaviorTreeViewer
{
    [XmlInclude(typeof(IntDefaultValue))]
    [XmlInclude(typeof(LongDefaultValue))]
    [XmlInclude(typeof(FloatDefaultValue))]
    [XmlInclude(typeof(DoubleDefaultValue))]
    [XmlInclude(typeof(BooleanDefaultValue))]
    [XmlInclude(typeof(StringDefaultValue))]
    [XmlInclude(typeof(Vector2DefaultValue))]
    [XmlInclude(typeof(Vector3DefaultValue))]
    [XmlInclude(typeof(ColorDefaultValue))]
    [XmlInclude(typeof(EnumDefaultValue))]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public abstract class BaseDefaultValue
    {
    }
}