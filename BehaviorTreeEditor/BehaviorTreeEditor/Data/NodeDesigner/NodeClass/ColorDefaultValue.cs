using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class ColorDefaultValue : BaseDefaultValue
    {
        [Category("常规"), DisplayName("R默认值"), Description("R默认值")]
        public int R { get; set; }

        [Category("常规"), DisplayName("G默认值"), Description("G默认值")]
        public int G { get; set; }

        [Category("常规"), DisplayName("B默认值"), Description("B默认值")]
        public int B { get; set; }

        [Category("常规"), DisplayName("A默认值"), Description("A默认值")]
        public int A { get; set; }

        public override string ToString()
        {
            return string.Format("[R:{0},G:{1},B:{2},A:{3}]", R, G, B, A);
        }
    }
}
