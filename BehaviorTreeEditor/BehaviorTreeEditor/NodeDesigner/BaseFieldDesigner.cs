using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    [XmlInclude(typeof(IntFieldDesigner))]
    [XmlInclude(typeof(RepeatIntFieldDesigner))]
    public abstract class BaseFieldDesigner
    {
        [CategoryAttribute("常规")]
        [DescriptionAttribute("字段名称")]
        public string FieldName { get; set; }

    }
}
