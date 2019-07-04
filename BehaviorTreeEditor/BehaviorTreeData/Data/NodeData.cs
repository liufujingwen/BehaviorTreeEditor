using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviorTreeData
{
    public partial class NodeData : Binary
    {
        public int ID;
        public string ClassType;
        public string ClassName;
        public List<BaseField> Fileds = new List<BaseField>();
        public List<NodeData> Childs = null;

        public override void Read(ref Reader reader)
        {
            reader.Read(ref ID).Read(ref ClassType).Read(ref ClassName).Read(ref Fileds).Read(ref Childs);
        }

        public override void Write(ref Writer writer)
        {
            writer.Write(ID).Write(ClassType).Write(ClassName).Write(Fileds).Write(Childs);
        }

        public BaseField this[string fieldName]
        {
            get
            {
                for (int i = 0; i < Fileds.Count; i++)
                {
                    BaseField field = Fileds[i];
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
