using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public struct VerifyInfo
    {
        public static VerifyInfo DefaultVerifyInfo = default(VerifyInfo);

        public VerifyInfo(string msg)
        {
            Msg = msg;
        }

        public string Msg { get; set; }
        public bool HasError { get { return !string.IsNullOrEmpty(Msg); } }
    }
}
