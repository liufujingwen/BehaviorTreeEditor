using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace BTData
{
    public abstract class Binary
    {
        public abstract void Read(ref Reader reader);
        public abstract void Write(ref Writer writer);
    }
}
