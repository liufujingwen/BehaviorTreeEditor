using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class NodeClass
    {
        private string m_ClassType;

        [CategoryAttribute("常规")]
        [DescriptionAttribute("类名")]
        public string ClassType
        {
            get { return m_ClassType; }
            set { m_ClassType = value; }
        }

        //节点类型
        private NodeType m_NodeType;
        public NodeType NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        private string m_Describe;

        [CategoryAttribute("常规")]
        [DescriptionAttribute("描述")]
        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();

        [CategoryAttribute("常规")]
        [DescriptionAttribute("类所有字段")]
        public List<FieldDesigner> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }
    }
}
