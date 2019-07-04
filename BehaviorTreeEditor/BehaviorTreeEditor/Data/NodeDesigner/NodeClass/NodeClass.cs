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

        private string m_Category;
        [Category("常规"), DisplayName("类别"), Description("类别")]
        [XmlAttribute]
        public string Category
        {
            get { return m_Category; }
            set { m_Category = value; }
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

        public NodeField FindField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;

            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField temp = m_Fields[i];
                if (temp.FieldName == fieldName)
                {
                    return temp;
                }
            }
            return null;
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
        /// 检验ClassType
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyClassType()
        {
            if (string.IsNullOrEmpty(m_ClassType))
                return new VerifyInfo("ClassType为空");
            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 是否存在空的字段名字
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyEmptyFieldName()
        {
            //检测是否有空字段
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField field = m_Fields[i];
                if (string.IsNullOrEmpty(field.FieldName))
                {
                    return new VerifyInfo(string.Format("节点类[{0}],存在空字段", m_ClassType));
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 检验是否存在相同字段名字
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifySameFieldName()
        {
            //检测字段是否重复
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField field_i = m_Fields[i];
                for (int ii = i + 1; ii < m_Fields.Count; ii++)
                {
                    NodeField field_ii = m_Fields[ii];
                    if (field_i.FieldName == field_ii.FieldName)
                        return new VerifyInfo(string.Format("节点类[{0}]存在重复字段[{1}]", m_ClassType, field_ii.FieldName));
                }
            }
            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 是否存在无效枚举类型
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyEnum()
        {
            //校验枚举类型
            //检测字段是否重复
            for (int i = 0; i < m_Fields.Count; i++)
            {
                NodeField field = m_Fields[i];
                if (field.FieldType == FieldType.EnumField)
                {
                    EnumDefaultValue enumDeaultValue = field.DefaultValue as EnumDefaultValue;
                    if (enumDeaultValue != null)
                    {
                        if (string.IsNullOrEmpty(enumDeaultValue.EnumType))
                        {
                            return new VerifyInfo(string.Format("节点类型[{0}]的字段[{1}]的枚举类型为空", this.ClassType, field.FieldName));
                        }

                        CustomEnum customEnum = MainForm.Instance.NodeClasses.FindEnum(enumDeaultValue.EnumType);
                        if (customEnum == null)
                        {
                            return new VerifyInfo(string.Format("节点类型[{0}]的字段[{1}]的枚举类型[{2}]不存在", this.ClassType, field.FieldName, enumDeaultValue.EnumType));
                        }
                        else
                        {
                            EnumItem enumItem = customEnum.FindEnum(enumDeaultValue.DefaultValue);
                            if (enumItem == null)
                                return new VerifyInfo(string.Format("节点类型[{0}]的字段[{1}]的枚举类型[{2}]不存在选项[{3}]", this.ClassType, field.FieldName, customEnum.EnumType, enumDeaultValue.DefaultValue));
                        }
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 检验节点是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyNodeClass()
        {
            //检验ClassType
            VerifyInfo verifyClassType = VerifyClassType();
            if (verifyClassType.HasError)
                return verifyClassType;

            //检验字段是否存在空名
            VerifyInfo verifyEmptyFieldName = VerifyEmptyFieldName();
            if (verifyEmptyFieldName.HasError)
                return verifyEmptyFieldName;

            //校验是否重名
            VerifyInfo verifySameFieldName = VerifySameFieldName();
            if (verifySameFieldName.HasError)
                return verifySameFieldName;

            //校验枚举
            VerifyInfo verifyEnum = VerifyEnum();
            if (verifyEnum.HasError)
                return verifyEnum;

            return VerifyInfo.DefaultVerifyInfo;
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
            m_Category = nodeClass.Category;
            m_NodeType = nodeClass.NodeType;
            m_Describe = nodeClass.Describe;

            m_Fields.Clear();
            m_Fields.AddRange(nodeClass.Fields);
        }

        /// <summary>
        /// 移除未定义的枚举字段
        /// </summary>
        public void RemoveUnDefineEnumField()
        {
            for (int i = m_Fields.Count - 1; i >= 0; i--)
            {
                NodeField field = m_Fields[i];
                if (field.FieldType == FieldType.EnumField)
                {
                    EnumDefaultValue enumDeaultValue = field.DefaultValue as EnumDefaultValue;
                    if (enumDeaultValue != null)
                    {
                        CustomEnum customEnum = MainForm.Instance.NodeClasses.FindEnum(enumDeaultValue.EnumType);
                        if (customEnum == null)
                        {
                            m_Fields.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}