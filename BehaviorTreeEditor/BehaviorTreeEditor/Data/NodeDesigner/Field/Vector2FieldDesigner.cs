using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BehaviorTreeEditor
{
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class Vector2FieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("X(int)"), Description("X(int)")]
        public int X { get; set; }

        [Category("常规"), DisplayName("Y(int)"), Description("Y(int)")]
        public int Y { get; set; }

        public override string FieldContent()
        {
            return string.Format("[{0},{1}]", X, Y);
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }
    }
}