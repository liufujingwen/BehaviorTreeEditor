using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
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

        public override BaseField Clone()
        {
            Vector3Field field = new Vector3Field();
            field.FieldName = FieldName;
            field.X = X;
            field.Y = Y;
            field.Z = Z;
            return field;
        }
    }
}
