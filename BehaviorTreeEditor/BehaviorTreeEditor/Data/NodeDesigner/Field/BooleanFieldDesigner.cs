using System.ComponentModel;

namespace BehaviorTreeEditor
{
    public class BooleanFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Boolean值"), Description("Boolean值")]
        public bool Value { get; set; }

        public override string FieldContent()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return "bool";
        }
    }
}