using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Vector3DefaultValue : BaseDefaultValue
    {
        [Category("常规"), DisplayName("X默认值"), Description("X默认值")]
        public int X { get; set; }

        [Category("常规"), DisplayName("Y默认值"), Description("Y默认值")]
        public int Y { get; set; }

        [Category("常规"), DisplayName("Z默认值"), Description("Z默认值")]
        public int Z { get; set; }

        public override string ToString()
        {
            return string.Format("[X:{0},Y:{1},Z:{2}]", X, Y, Z);
        }
    }
}
