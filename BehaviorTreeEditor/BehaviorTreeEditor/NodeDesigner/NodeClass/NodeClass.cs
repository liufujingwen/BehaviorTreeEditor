using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    public class NodeClass
    {
        private string m_ClassType;

        [CategoryAttribute("常规")]
        [DescriptionAttribute("类名")]
        [XmlAttribute]
        public string ClassType
        {
            get { return m_ClassType; }
            set { m_ClassType = value; }
        }

        //节点类型
        private NodeType m_NodeType;

        [CategoryAttribute("常规")]
        [DescriptionAttribute("节点类型")]
        [XmlAttribute]
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

        [XmlIgnore]
        private bool m_Dirty = false;

        public bool Dirty
        {
            get { return m_Dirty; }
            set { m_Dirty = value; }
        }

        public bool ExistFieldName(string fieldName)
        {
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner temp = m_Fields[i];
                if (temp.Field.FieldName == fieldName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddField(FieldDesigner field)
        {
            if (field == null)
            {
                return false;
            }

            if (field.FieldType == FieldType.None)
            {
                MainForm.Instance.ShowInfo("字段类型为None,添加失败！！！");
                MainForm.Instance.ShowMessage("字段类型为None,添加失败！！！", "警告");
                return false;
            }

            if (string.IsNullOrEmpty(field.Field.FieldName))
            {
                MainForm.Instance.ShowInfo("字段名为空,添加失败！！！");
                MainForm.Instance.ShowMessage("字段名为空,添加失败！！！", "警告");
                return false;
            }

            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner temp = m_Fields[i];
                if (temp.Field.FieldName == field.Field.FieldName)
                {
                    MainForm.Instance.ShowInfo("字段名字相同,添加失败！！！");
                    MainForm.Instance.ShowMessage("字段名字相同,添加失败！！！", "警告");
                    return false;
                }
            }

            m_Fields.Add(field);

            return true;
        }
    }
}
