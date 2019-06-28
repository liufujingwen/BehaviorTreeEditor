using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class LongFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Long值"),Description("Long值")]
        public long Value { get; set; }

        public override string FieldContent()
        {
            return string.Format("{0}:{1}", FieldName, Value);
        }

        public override string ToString()
        {
            return "long";
        }
    }
}
