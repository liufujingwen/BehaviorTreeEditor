using System;
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

        private List<NodeField> m_Fields = new List<NodeField>();

        [Category("常规"), DisplayName("类所有字段"), Description("类所有字段")]
        public List<NodeField> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public bool ExistFieldName(string fieldName)
        {
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField temp = m_Fields[i];
                if (temp.FieldName == fieldName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddField(NodeField field)
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
                NodeField temp = m_Fields[i];
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

        /// <summary>
        /// 是否存在空的字段名字
        /// </summary>
        /// <returns></returns>
        public bool ExistEmptyFieldName()
        {
            //检测是否有空字段
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField field = m_Fields[i];
                if (string.IsNullOrEmpty(field.FieldName))
                {
                    MainForm.Instance.ShowMessage("存在空字段");
                    MainForm.Instance.ShowInfo("存在空字段");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在相同字段名字
        /// </summary>
        /// <returns></returns>
        public bool ExistSameFieldName()
        {
            //检测字段是否重复
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField field_i = m_Fields[i];
                for (int ii = i + 1; ii < m_Fields.Count; ii++)
                {
                    NodeField field_ii = m_Fields[ii];
                    if (field_i.FieldName == field_ii.FieldName)
                    {
                        MainForm.Instance.ShowMessage(string.Format("存在重复字段:{0}", field_ii.FieldName));
                        MainForm.Instance.ShowInfo(string.Format("存在重复字段:{0} 时间:{1}", field_ii.FieldName, DateTime.Now));
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 更新NodeClass的内容
        /// </summary>
        /// <param name="nodeClass"></param>
        public void UpdateNodeClass(NodeClass nodeClass)
        {
            if (nodeClass == null)
                return;

            if (nodeClass == this)
                return;

            m_ClassType = nodeClass.ClassType;
            m_ClassName = nodeClass.ClassName;
            m_NodeType = nodeClass.NodeType;
            m_Describe = nodeClass.Describe;

            m_Fields.Clear();
            m_Fields.AddRange(nodeClass.Fields);
        }
    }
}