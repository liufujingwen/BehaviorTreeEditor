using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
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

        public Vector2 Clone()
        {
            Vector2 vector2 = new Vector2();
            vector2.X = X;
            vector2.Y = Y;
            return vector2;
        }
    }
}