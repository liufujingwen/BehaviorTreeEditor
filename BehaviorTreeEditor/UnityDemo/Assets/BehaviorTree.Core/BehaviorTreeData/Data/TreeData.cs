using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class TreeData : Binary
    {
        public List<AgentData> Agents = new List<AgentData>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref Agents);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(Agents);
        }
    }
}