using System.Collections.Generic;

namespace BTData
{
    public partial class GroupData : Binary
    {
        public string GroupName;
        public List<BehaviorTreeData> BehaviorTrees = new List<BehaviorTreeData>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref GroupName).Read(ref BehaviorTrees);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(GroupName).Write(BehaviorTrees);
        }
    }
}
