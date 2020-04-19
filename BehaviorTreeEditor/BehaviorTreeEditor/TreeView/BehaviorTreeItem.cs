using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class BehaviorTreeItem : ITreeViewItem
    {
        public TreeNode TreeNode { get; set; }
        public GroupItem GroupItem;
        public BehaviorTreeDesigner BehaviorTree;
    }
}
