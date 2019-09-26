using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public class Vector2Field : BaseField
    {
        public int X;
        public int Y;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref X).Read(ref Y);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(X).Write(Y);
        }

        public override BaseField Clone()
        {
            Vector2Field field = new Vector2Field();
            field.FieldName = FieldName;
            field.X = X;
            field.Y = Y;
            return field;
        }
    }
}
