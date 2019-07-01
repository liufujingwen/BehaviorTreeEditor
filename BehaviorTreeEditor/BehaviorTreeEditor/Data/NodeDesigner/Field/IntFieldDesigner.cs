using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace BehaviorTreeEditor
{
    public class IntFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Int值"), Description("Int值")]
        public int Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
