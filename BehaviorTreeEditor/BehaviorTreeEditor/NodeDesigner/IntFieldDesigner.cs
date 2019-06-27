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
        [CategoryAttribute("常规"), DescriptionAttribute("Int值")]
        public int Value { get; set; }

        public override string ToString()
        {
            return "Int";
        }
    }
}
