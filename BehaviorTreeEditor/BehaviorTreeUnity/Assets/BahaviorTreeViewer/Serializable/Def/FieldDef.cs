using System;

namespace Serializable.Def
{
    public class FieldDef// : IComparable<FieldDef>
    {
        private FieldDef()
        {
        }

        /** 索引 */
        private int code;
        /** 字段名 */
        private string name;

        /** 索引 */
        public int Code { get { return code; } private set { code = value; } }
        /** 字段名 */
        public string Name { get { return name; } private set { name = value; } }

        public static FieldDef valueOf(int code, string name)
        {
            FieldDef e = new FieldDef();
            e.Name = name;
            e.Code = code;
            return e;
        }

        public object getValue(PO instance)
        {
            return instance[Name];
        }

        public void setValue(PO instance, object value)
        {
            instance.Add(Name, value);
        }

        // 实现接口方法
        //public int CompareTo (FieldDef obj) {
        //	return this.Code - obj.Code;
        //}

    }
}

