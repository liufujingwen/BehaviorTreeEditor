using System;
using System.Collections.Generic;
using System.Text;

namespace Serializable
{
    public class BaseEnum
    {
        public int enumValue;
        public string enumString;
        public string typeStr;

        public BaseEnum(string typeStr, int enumValue, string enumString)
        {
            this.typeStr = typeStr; 
            this.enumValue = enumValue;
            this.enumString = enumString;
        }

        public override int GetHashCode()
        {
            return this.enumValue;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", this.enumString, this.enumValue);
        }

    }
}
