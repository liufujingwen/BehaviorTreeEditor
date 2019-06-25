using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class NodeData : Binary
    {
        public int ID;
        public string ClassType;
        public List<BaseFiled> Fileds = new List<BaseFiled>();
        public List<NodeData> Childs = null;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref ID).Read(ref ClassType).Read(ref Fileds).Read(ref Childs);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(ID).Write(ClassType).Write(Fileds).Write(Childs);
        }
    }
}
