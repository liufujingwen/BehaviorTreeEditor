using System.Collections.Generic;

namespace BTData
{
    public partial class TreeDatas : Binary
    {
        public VariableData GlobalVariable = new VariableData();
        public VariableData ContextVariable = new VariableData();
        public List<BehaviorTreeData> BehaviorTrees = new List<BehaviorTreeData>();
        public List<GroupData> Groups = new List<GroupData>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref GlobalVariable).Read(ref ContextVariable).Read(ref BehaviorTrees).Read(ref Groups);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(GlobalVariable).Write(ContextVariable).Write(BehaviorTrees).Write(Groups);
        }
    }
}