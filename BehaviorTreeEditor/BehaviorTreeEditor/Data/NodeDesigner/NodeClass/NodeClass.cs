using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BehaviorTreeEditor
{
    public class NodeClass
    {
        private string m_ClassType;

        [Category("常规"), DisplayName("类"), Description("类,唯一标识")]
        [XmlAttribute]
        public string ClassType
        {
            get { return m_ClassType; }
            set { m_ClassType = value; }
        }

        private string m_ClassName;

        [Category("常规"), DisplayName("类名"), Description("类名(中文名)")]
        [XmlAttribute]
        public string ClassName
        {
            get { return m_ClassName; }
            set { m_ClassName = value; }
        }

        //节点类型
        private NodeType m_NodeType;

        [Category("常规"), DisplayName("节点类型"), Description("节点类型")]
        [XmlAttribute]
        public NodeType NodeType
        {
            get { return m_NodeType; }
            set { m_NodeType = value; }
        }

        private string m_Describe;

        [Category("常规"), DisplayName("描述"), Description("描述")]
        public string Describe
        {
            get { return m_Describe; }
            set { m_Describe = value; }
        }

        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();

        [Category("常规"), DisplayName("类所有字段"), Description("类所有字段")]
        public List<FieldDesigner> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public bool ExistFieldName(string fieldName)
        {
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner temp = m_Fields[i];
                if (temp.FieldName == fieldName)
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

            if (string.IsNullOrEmpty(field.FieldName))
            {
                MainForm.Instance.ShowInfo("字段名为空,添加失败！！！");
                MainForm.Instance.ShowMessage("字段名为空,添加失败！！！", "警告");
                return false;
            }

            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner temp = m_Fields[i];
                if (temp.FieldName == field.FieldName)
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
