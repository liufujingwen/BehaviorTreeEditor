using System;
using System.Collections.Generic;


namespace Serializable
{
    /**
	 * 传输对象
	 */
    public class PO
    {
        public Dictionary<string, object> dic = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            dic.Add(key, value);
        }

        public object this[string key]
        {
            get
            {
                if (dic.ContainsKey(key))
                    return dic[key];
                return null;
            }
            set { dic[key] = value; }
        }

        #region Builder
        private PO() : base()
        {
        }

        //public PO(string nameSpace, string objectClass) : this()
        //{
        //    this.objectClass = objectClass;
        //    this.nameSpace = nameSpace;
        //}

        public PO(Type type) : this()
        {
            this.objectClass = type.FullName.Replace("+", ".");
            this.type = type;
        }

        #endregion

        #region Fields

        private string nameSpace;
        /** 对象类型 */
        private string objectClass;
        /**对象Type*/
        private Type type;

        #endregion

        #region Properties

        /** 对象类型 */
        public string ObjectClass
        {
            get { return objectClass; }
        }

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }


        public bool IsEmpty()
        {
            return dic.Keys.Count == 0;
        }

        #endregion

    }
}

