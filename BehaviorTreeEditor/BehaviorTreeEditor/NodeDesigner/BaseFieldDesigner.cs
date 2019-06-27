using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    [Serializable]
    [XmlInclude(typeof(IntFieldDesigner))]
    [XmlInclude(typeof(RepeatIntFieldDesigner))]
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public abstract class BaseFieldDesigner
    {
        [Category("常规")]
        [DisplayName("ID")]
        [Description("ID")]
        [ReadOnly(true)]
        public int ID { get; set; }

        [Category("常规")]
        [DisplayName("字段名称")]
        [Description("字段名称")]
        public string FieldName { get; set; }

        [Category("常规")]
        [DisplayName("描述")]
        [Description("描述")]
        public string Describe { get; set; }
    }
}
