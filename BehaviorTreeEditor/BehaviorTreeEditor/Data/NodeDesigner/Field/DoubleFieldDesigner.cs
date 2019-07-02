using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DoubleFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Double值"), Description("Double值")]
        public double Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
