using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class NodeItem : TreeViewItem
    {
        public NodeTypeItem NodeTypeItem;
        public CategoryItem CategoryItem;
        public NodeDefine NodeDefine;

        public NodeType OldNodeType;
        public string OldCategory = string.Empty;
    }
}
