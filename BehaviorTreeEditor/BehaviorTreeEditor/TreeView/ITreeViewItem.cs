using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public interface ITreeViewItem
    {
        TreeNode TreeNode { get; set; }
    }
}
