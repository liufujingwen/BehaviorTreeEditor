using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
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

        public override BaseField Clone()
        {
            RepeatVector3Field field = new RepeatVector3Field();
            field.FieldName = FieldName;
            if (Value != null)
            {
                field.Value = new List<Vector3>(Value.Count);
                for (int i = 0; i < Value.Count; i++)
                {
                    Vector3 vector = Value[i];
                    field.Value.Add(vector != null ? vector.Clone() : null);
                }
            }
            return field;
        }
    }
}
