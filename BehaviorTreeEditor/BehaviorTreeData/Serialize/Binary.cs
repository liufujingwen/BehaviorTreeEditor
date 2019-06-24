using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BehaviorTreeData
{
    public abstract class Binary
    {
        public abstract void Read(ref Reader reader);
        public abstract void Write(ref Writer writer);
    }

    //public enum write_type
    //{
    //    write_type_vaint = 0,	//int32、int64、uint32、uint64、bool、enum、double、float
    //    write_type_string = 1,
    //    write_type_message = 2,
    //    write_type_repeated = 3,
    //}
}
