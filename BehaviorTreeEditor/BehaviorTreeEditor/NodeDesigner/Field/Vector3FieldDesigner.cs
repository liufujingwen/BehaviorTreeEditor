using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class Vector3FieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("X(int)"), Description("X(int)")]
        public int X { get; set; }

        [Category("常规"), DisplayName("Y(int)"), Description("Y(int)")]
        public int Y { get; set; }

        [Category("常规"), DisplayName("Z(int)"), Description("Z(int)")]
        public int Z { get; set; }

        public override string FieldContent()
        {
            return string.Format("{0}:{1},{2},{3}", FieldName, X, Y, Z);
        }

        public override string ToString()
        {
            return "vector3";
        }
    }
}
