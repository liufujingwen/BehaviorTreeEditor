using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    [Serializable]
    [XmlInclude(typeof(IntFieldDesigner))]
    [XmlInclude(typeof(RepeatIntFieldDesigner))]
    [XmlInclude(typeof(LongFieldDesigner))]
    [XmlInclude(typeof(RepeatLongFieldDesigner))]
    [XmlInclude(typeof(FloatFieldDesigner))]
    [XmlInclude(typeof(RepeatFloatFieldDesigner))]
    [XmlInclude(typeof(BooleanFieldDesigner))]
    [XmlInclude(typeof(Vector2FieldDesigner))]
    [XmlInclude(typeof(RepeatVector2FieldDesigner))]
    [XmlInclude(typeof(Vector3FieldDesigner))]
    [XmlInclude(typeof(RepeatVector3FieldDesigner))]
    [XmlInclude(typeof(ColorFieldDesigner))]
    [XmlInclude(typeof(EnumFieldDesigner))]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public abstract class BaseFieldDesigner
    {
        public abstract string FieldContent();
    }
}
