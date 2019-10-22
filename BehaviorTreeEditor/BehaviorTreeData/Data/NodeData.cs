using System;
using System.Collections.Generic;
using System.Text;

namespace BTData
{
    public partial class NodeData : Binary
    {
        public int X;
        public int Y;

        public int ID;
        public string ClassType;
        public string Label;
        public List<BaseField> Fields = new List<BaseField>();
        public List<NodeData> Childs = null;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref X).Read(ref Y).Read(ref ID).Read(ref ClassType).Read(ref Label).Read(ref Fields).Read(ref Childs);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(X).Write(Y).Write(ID).Write(ClassType).Write(Label).Write(Fields).Write(Childs);
        }

        public BaseField this[string fieldName]
        {
            get
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    BaseField field = Fields[i];
                    if (field == null)
                        continue;
                    if (field.FieldName == fieldName)
                        return field;
                }

                return null;
            }
        }
    }
}
