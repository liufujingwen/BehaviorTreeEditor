using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    public class NodeDesigner
    {
        //节点唯一标识
        public int ID;
        //名字
        public string Name = string.Empty;
        //节点类名
        public string ClassType = string.Empty;
        //节点类型
        public NodeType NodeType = NodeType.Start;
        //节点位置
        public Rect Rect;
        //子节点
        public List<Transition> Transitions = new List<Transition>();
        //父节点
        [XmlIgnore]
        public NodeDesigner ParentNode;

        public NodeDesigner()
        {
            Rect.width = EditorUtility.NodeWidth;
            Rect.height = EditorUtility.NodeHeight;
        }

        public NodeDesigner(string name, string classType, Rect rect)
        {
            Name = name;
            ClassType = classType;
            Rect = rect;
            Rect.width = Math.Max(rect.width, EditorUtility.NodeWidth);
            Rect.height = Math.Max(rect.height, EditorUtility.NodeHeight);
        }

        /// <summary>
        /// 指定点是否在控件范围内
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns>true:在控件范围内，false:不在控件范围内</returns>
        public bool IsContains(Vec2 point)
        {
            return Rect.Contains(point);
        }

        public bool Exist(NodeDesigner node)
        {
            if (node == null)
                return false;

            for (int i = 0; i < Transitions.Count; i++)
            {
                Transition transition = Transitions[i];
                if (transition != null)
                {
                    if (transition.ToNode == node)
                        return true;
                }
            }

            return false;
        }

        public void AddNode(NodeDesigner node)
        {
            if (node == null)
                return;

            if (Exist(node))
            {
                throw new Exception(string.Format("已存在节点id:{0},name:{1}", node.ID, node.Name));
            }

            node.ParentNode = this;
            Transition transition = new Transition();
            transition.Set(node, this);
            Transitions.Add(transition);
        }
    }
}
