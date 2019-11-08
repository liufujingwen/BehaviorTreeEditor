using System.Collections.Generic;

namespace BTData
{
    public class VariableData : Binary
    {
        public List<BaseField> VariableFields = new List<BaseField>();

        public override void Read(ref Reader reader)
        {
            reader.Read(ref VariableFields);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(VariableFields);
        }
    }
}
