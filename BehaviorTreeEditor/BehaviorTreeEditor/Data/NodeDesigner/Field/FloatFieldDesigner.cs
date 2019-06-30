using System.ComponentModel;

namespace BehaviorTreeEditor
{
    public class FloatFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Float值"), Description("Float值")]
        public float Value { get; set; }

        public override string FieldContent()
        {
            return Value.ToString();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
