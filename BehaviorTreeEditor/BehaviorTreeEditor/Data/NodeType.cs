using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public enum NodeType
    {
        Start,//开始节点
        Composite,//组合节点
        Decorator,//修饰节点
        Condition,//条件节点
        Action,//叶节点
    }
}
