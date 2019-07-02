using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    public class NodeDesigner
    {
        public NodeDesigner()
        {
            Rect.width = EditorUtility.NodeWidth;
            Rect.height = EditorUtility.NodeHeight;
        }

        public NodeDesigner(string name, string classType, Rect rect)
        {
            m_ClassName = name;
            ClassType = classType;
            Rect = rect;
            Rect.width = Math.Max(rect.width, EditorUtility.NodeWidth);
            Rect.height = Math.Max(rect.height, EditorUtility.NodeHeight);
        }

        //节点唯一标识
        private int m_ID;
        //是否开始节点
        private bool m_StartNode;
        //名字
        private string m_ClassName = string.Empty;
        //节点类名
        private string m_ClassType;
        //节点类型
        private NodeType m_NodeType = NodeType.Composite;
        //描述
        private string m_Describe;
        //字段
        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();

        //节点位置
        public Rect Rect;
        //子节点
        public List<Transition> Transitions = new List<Transition>();
        //父节点
        [XmlIgnore]
        public NodeDesigner ParentNode;

        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public bool StartNode
        {
            get { return m_StartNode; }
            set { m_StartNode = value; }
        }

        public string ClassType
        {
            get { return m_ClassType; }
            set { m_ClassType = value; }
        }

        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        public NodeType NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        public List<FieldDesigner> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
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
                throw new Exception(string.Format("已存在节点id:{0},name:{1}", node.ID, node.ClassType));
            }

            node.ParentNode = this;
            Transition transition = new Transition();
            transition.Set(node, this);
            Transitions.Add(transition);
        }
    }
}
