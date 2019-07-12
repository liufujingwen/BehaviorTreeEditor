using System.ComponentModel;

namespace BehaviorTreeViewer
{
    public class LongFieldDesigner : BaseFieldDesigner
    {
        [Category("常规"), DisplayName("Long值"), Description("Long值")]
        public long Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
