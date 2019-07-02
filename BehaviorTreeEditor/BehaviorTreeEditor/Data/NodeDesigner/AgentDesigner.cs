using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class AgentDesigner
    {
        private string m_AgentID;
        private string m_Describe;
        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();
        private List<NodeDesigner> m_Nodes = new List<NodeDesigner>();

        public string AgentID
        {
            get { return m_AgentID; }
            set { m_AgentID = value; }
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

        public List<NodeDesigner> Nodes
        {
            get { return m_Nodes; }
            set { m_Nodes = value; }
        }

        public int GenNodeID()
        {
            int id = 0;
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (id <= node.ID)
                    id = node.ID;
            }
            return ++id;
        }

        /// <summary>
        /// 通过ID查找节点
        /// </summary>
        /// <param name="ID">节点ID</param>
        /// <returns></returns>
        public NodeDesigner FindNodeByID(int ID)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.ID == ID)
                    return node;
            }
            return null;
        }

        public void AddNode(NodeDesigner node)
        {
            if (node == null)
                return;

            if (ExistNode(node))
            {
                throw new Exception(string.Format("已存在节点id:{0},name:{1}", node.ID, node.ClassType));
            }

            Nodes.Add(node);
        }

        public void RemoveNode(NodeDesigner node)
        {
            if (node != null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    NodeDesigner node_i = Nodes[i];
                    if (node_i != null && node_i.ID == node.ID)
                    {
                        if (node_i.Transitions.Count > 0)
                        {
                            //删除Transtion
                            for (int ii = 0; ii < node_i.Transitions.Count; ii++)
                            {
                                Transition transition = node_i.Transitions[ii];
                                transition.ToNode.ParentNode = null;
                            }
                            node_i.Transitions.Clear();
                        }

                        if (node_i.ParentNode != null)
                        {
                            for (int ii = 0; ii < node_i.ParentNode.Transitions.Count; ii++)
                            {
                                Transition transition = node_i.ParentNode.Transitions[ii];
                                if (transition.ToNode == node_i)
                                {
                                    node_i.ParentNode.Transitions.RemoveAt(ii);
                                    break;
                                }
                            }
                        }

                        node_i.ParentNode = null;
                        Nodes.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void RemoveTranstion(Transition transition)
        {
            if (transition == null)
                return;

            transition.FromNode.Transitions.Remove(transition);
            transition.ToNode.ParentNode = null;
        }

        public bool ExistNode(NodeDesigner node)
        {
            if (node != null)
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    NodeDesigner node_i = Nodes[i];
                    if (node_i != null && node_i.ID == node.ID)
                        return true;
                }
            }
            return false;
        }

        public NodeDesigner FindByID(int ID)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.ID == ID)
                    return node;
            }
            return null;
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

        /// <summary>
        /// 判断字段名是否已存在
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 检验AgentID是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyAgentID()
        {
            if (string.IsNullOrEmpty(m_AgentID))
                return new VerifyInfo("AgentID为空");
            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 校验字段是否存在空名字
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyEmptyFieldName()
        {
            //检测是否有空字段
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field = m_Fields[i];
                if (string.IsNullOrEmpty(field.FieldName))
                {
                    return new VerifyInfo(string.Format("行为树[{0}]存在空字段名", m_AgentID));
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 校验是否存在相同字段名字
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifySameFieldName()
        {
            //检测字段是否重复
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field_i = m_Fields[i];
                for (int ii = i + 1; ii < m_Fields.Count; ii++)
                {
                    FieldDesigner field_ii = m_Fields[ii];
                    if (field_i.FieldName == field_ii.FieldName)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]存在重复字段:{1}", m_AgentID, field_ii.FieldName));
                    }
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
                FieldDesigner field = m_Fields[i];
                if (field.FieldType == FieldType.EnumField)
                {
                    EnumFieldDesigner enumFieldDesigner = field.Field as EnumFieldDesigner;
                    if (enumFieldDesigner != null)
                    {
                        if (string.IsNullOrEmpty(enumFieldDesigner.EnumType))
                        {
                            return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型为空", m_AgentID, field.FieldName));
                        }

                        CustomEnum customEnum = MainForm.Instance.NodeClasses.FindEnum(enumFieldDesigner.EnumType);
                        if (customEnum == null)
                        {
                            return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型[{2}]不存在", m_AgentID, field.FieldName, enumFieldDesigner.EnumType));
                        }
                        else
                        {
                            EnumItem enumItem = customEnum.FindEnum(enumFieldDesigner.Value);
                            if (enumItem == null)
                                return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型[{2}]不存在选项[{3}]", m_AgentID, field.FieldName, customEnum.EnumType, enumFieldDesigner.Value));
                        }
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 校验Agent是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyAgent()
        {
            //检验ID是否合法
            VerifyInfo verifyAgentID = VerifyAgentID();
            if (verifyAgentID.HasError)
                return verifyAgentID;

            //检验是否存在空字段名
            VerifyInfo verifyEmptyFieldName = VerifyEmptyFieldName();
            if (verifyEmptyFieldName.HasError)
                return verifyEmptyFieldName;

            //检验是否存在相同名字字段
            VerifyInfo verifySameFieldName = VerifySameFieldName();
            if (verifySameFieldName.HasError)
                return verifySameFieldName;

            //检验枚举字段
            VerifyInfo verifyEnum = VerifyEnum();
            if (verifyEnum.HasError)
                return verifyEnum;

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 更新Agent内容，但Nodes不能改
        /// </summary>
        /// <param name="agent"></param>
        public void UpdateAgent(AgentDesigner agent)
        {
            if (agent == this)
                return;

            m_AgentID = agent.AgentID;
            m_Describe = agent.Describe;

            m_Fields.Clear();
            m_Fields.AddRange(agent.Fields.ToArray());
        }
    }
}