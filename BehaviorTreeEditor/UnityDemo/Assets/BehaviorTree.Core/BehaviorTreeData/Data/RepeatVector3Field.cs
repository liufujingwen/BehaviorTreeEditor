using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public class RepeatVector3Field : BaseField
    {
        public List<Vector3> Value = new List<Vector3>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref FieldName).Read(ref Value);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(FieldName).Write(Value);
        }
    }
}
