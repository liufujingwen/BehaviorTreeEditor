using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

namespace BehaviorTreeViewer
{
    public class NodeDesigner
    {
        //节点唯一标识
        private int m_ID;
        //是否开始节点
        private bool m_StartNode;
        //名字
        private string m_Label = string.Empty;
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

        public string Label
        {
            get { return m_Label; }
            set { m_Label = value; }
        }

        public string Title
        {
            get { return !string.IsNullOrEmpty(m_Label) ? m_Label : m_ClassType; }
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
    }
}
