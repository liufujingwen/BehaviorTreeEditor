using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace R7BehaviorTree
{

    struct EProxyType_Comparer : IEqualityComparer<EProxyType>
    {
        public bool Equals(EProxyType x, EProxyType y)
        {
            return x == y;
        }

        public int GetHashCode(EProxyType obj)
        {
            return (int)obj;
        }
    }
}