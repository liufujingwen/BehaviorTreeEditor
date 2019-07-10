using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class Singleton<T>
    {
        protected static readonly T ms_instance = Activator.CreateInstance<T>();
        public static T Instance { get { return ms_instance; } }

        protected Singleton() { }
    }
}
