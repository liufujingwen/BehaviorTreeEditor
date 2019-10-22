using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class BehaviorTreeData : Binary
    {
        public GlobalVariableData GlobalVariable = new GlobalVariableData();
        public List<BehaviorTree> BehaviorTrees = new List<BehaviorTree>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref GlobalVariable).Read(ref BehaviorTrees);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(GlobalVariable).Write(BehaviorTrees);
        }
    }
}