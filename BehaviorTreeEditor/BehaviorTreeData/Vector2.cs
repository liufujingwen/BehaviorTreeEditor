using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public class Vector2 : Binary
    {
        public int X;
        public int Y;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref X).Read(ref Y);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(X).Write(Y);
        }
    }
}