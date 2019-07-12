using System.ComponentModel;

namespace BehaviorTreeViewer
{
    public class BooleanFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Boolean值"), Description("Boolean值")]
        public bool Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}