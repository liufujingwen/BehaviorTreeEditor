using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class AgentData : Binary
    {
        public NodeData StartNode;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref StartNode);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(StartNode);
        }
    }
}
