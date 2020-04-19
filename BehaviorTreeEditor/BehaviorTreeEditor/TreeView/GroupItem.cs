using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public class GroupItem : ITreeViewItem
    {
        public TreeNode TreeNode { get; set; }
        public BehaviorGroup Group;

        public BehaviorTreeItem AddBehaviorTree(BehaviorTreeDesigner behaviorTree, int index = -1)
        {
            if (!Group.AddBehaviorTree(behaviorTree))
                return null;

            string name = string.IsNullOrEmpty(behaviorTree.Name) ? behaviorTree.ID : string.Format("{0} ({1})", behaviorTree.ID, behaviorTree.Name);

            TreeNode tempNode;
            if (index != -1)
            {
                tempNode = TreeNode.Nodes.Insert(index, name);
            }
            else
            {
                tempNode = TreeNode.Nodes.Add(name);
            }

            BehaviorTreeItem behaviorTreeItem = new BehaviorTreeItem();
            behaviorTreeItem.GroupItem = this;
            behaviorTreeItem.BehaviorTree = behaviorTree;
            behaviorTreeItem.TreeNode = tempNode;

            tempNode.Tag = behaviorTreeItem;

            return behaviorTreeItem;
        }

        public bool RemoveBehaviorTree(BehaviorTreeDesigner behaviroTree)
        {
            if (!Group.RemoveBehaviorTree(behaviroTree))
                return false;

            for (int i = 0; i < TreeNode.Nodes.Count; i++)
            {
                TreeNode tempNode = TreeNode.Nodes[i];
                BehaviorTreeItem behaviorTreeItem = tempNode.Tag as BehaviorTreeItem;
                if (behaviorTreeItem.BehaviorTree == behaviroTree)
                {
                    behaviorTreeItem.GroupItem = null;
                    tempNode.Remove();
                    break;
                }
            }

            return true;
        }
    }
}
