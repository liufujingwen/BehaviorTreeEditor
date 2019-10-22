using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class BehaviorTree : Binary
    {
        public string ID;
        public List<BaseField> Fields = new List<BaseField>();
        public NodeData StartNode;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref ID).Read(ref Fields).Read(ref StartNode);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(ID).Write(Fields).Write(StartNode);
        }
    }
}
