using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class FloatFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Float值"), Description("Float值")]
        public float Value { get; set; }

        public override string FieldContent()
        {
            return string.Format("{0}:{1}", FieldName, Value);
        }

        public override string ToString()
        {
            return "float";
        }
    }
}
