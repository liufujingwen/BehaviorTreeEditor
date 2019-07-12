using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BehaviorTreeViewer
{
    public class NodeClass
    {
        private string m_ClassType;

        [XmlAttribute]
        public string ClassType
        {
            get { return m_ClassType; }
            set { m_ClassType = value; }
        }

        private string m_Label;

        [XmlAttribute]
        public string Label
        {
            get { return m_Label; }
            set { m_Label = value; }
        }

        private string m_Category = string.Empty;
        [XmlAttribute]
        public string Category
        {
            get { return m_Category; }
            set { m_Category = value; }
        }

        private bool m_ShowContent;
        [XmlAttribute]
        public bool ShowContent
        {
            get { return m_ShowContent; }
            set { m_ShowContent = value; }
        }

        //节点类型
        private NodeType m_NodeType;

        [XmlAttribute]
        public NodeType NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        private string m_Describe;

        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        private List<NodeField> m_Fields = new List<NodeField>();

        public List<NodeField> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }
    }
}