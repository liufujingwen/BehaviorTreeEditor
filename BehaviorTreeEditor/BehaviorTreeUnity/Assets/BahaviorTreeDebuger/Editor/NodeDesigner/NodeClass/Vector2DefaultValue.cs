using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class Vector2DefaultValue : BaseDefaultValue
    {
        [Category("常规"), DisplayName("X默认值"), Description("X默认值")]
        public int X { get; set; }

        [Category("常规"), DisplayName("Y默认值"), Description("Y默认值")]
        public int Y { get; set; }

        public override string ToString()
        {
            return string.Format("[X:{0},Y:{1}]", X, Y);
        }
    }
}
