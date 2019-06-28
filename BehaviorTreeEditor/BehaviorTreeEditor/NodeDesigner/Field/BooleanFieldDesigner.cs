using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class BooleanFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"),DisplayName("Boolean值"), Description("Boolean值")]
        public bool Value { get; set; }

        public override string FieldContent()
        {
            return string.Format("{0}:{1}", FieldName, Value);
        }

        public override string ToString()
        {
            return "bool";
        }
    }
}