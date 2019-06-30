using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace BehaviorTreeEditor
{
    public class IntFieldDesigner : BaseFieldDesigner
    {

        private List<string> m_EnumStrs = new List<string>();
        [Category("常规"), DisplayName("枚举值数组"), Description("枚举值数组")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public List<string> EnumStrs
        {
            get { return m_EnumStrs; }
            set { m_EnumStrs = value; }
        }


        [Category("常规"), DisplayName("Int值"), Description("Int值")]
        public int Value { get; set; }

        public override string FieldContent()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return "int";
        }
    }
}
