using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class IntFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Int值"), Description("Int值")]
        public int Value { get; set; }

        public override string FieldContent()
        {
            return string.Format("{0}:{1}", FieldName, Value);
        }

        public override string ToString()
        {
            return "int";
        }
    }
}
