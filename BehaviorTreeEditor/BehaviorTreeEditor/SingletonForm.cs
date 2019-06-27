using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class SingletonForm<T> where T : Form, new()
    {
        private static T ms_Instance;

        protected SingletonForm()
        {
            ms_Instance = this as T;
        }

    }
}
