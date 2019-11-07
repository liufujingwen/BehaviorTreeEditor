using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public class Vector3Field : BaseField
    {
        public int X;
        public int Y;
        public int Z;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref X).Read(ref Y).Read(ref Z);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(X).Write(Y).Write(Z);
        }
    }
}
