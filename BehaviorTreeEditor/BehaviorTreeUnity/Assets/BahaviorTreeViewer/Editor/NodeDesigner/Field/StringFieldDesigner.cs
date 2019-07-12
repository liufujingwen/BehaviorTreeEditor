using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class StringFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("String值"), Description("String值")]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Value) ? string.Empty : Value;
        }
    }

}
