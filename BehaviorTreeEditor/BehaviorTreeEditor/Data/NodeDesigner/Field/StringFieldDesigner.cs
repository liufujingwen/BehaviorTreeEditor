using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class StringFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Boolean值"), Description("Boolean值")]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Value) ? string.Empty : Value;
        }
    }

}
