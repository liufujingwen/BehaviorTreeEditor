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
