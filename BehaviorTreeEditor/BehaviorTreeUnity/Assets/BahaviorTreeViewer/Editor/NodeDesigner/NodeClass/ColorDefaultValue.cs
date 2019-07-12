using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class ColorDefaultValue : BaseDefaultValue
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public override string ToString()
        {
            return string.Format("[R:{0},G:{1},B:{2},A:{3}]", R, G, B, A);
        }
    }
}
