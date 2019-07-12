using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BehaviorTreeViewer
{
    public class BooleanDefaultValue : BaseDefaultValue
    {
        [Category("常规"), DisplayName("默认值"), Description("默认值")]
        public bool DefaultValue { get; set; }

        public override string ToString()
        {
            return DefaultValue.ToString();
        }
    }
}
