using System;
using System.Collections.Generic;

namespace BehaviorTreeEditor
{
    public class BehaviorTreeDesigner
    {
        private string m_TreeID;
        private string m_GroupName;
        private string m_Describe;
        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();
        private List<VariableFieldDesigner> m_BehaviorTreeVariables = new List<VariableFieldDesigner>();
        private List<NodeDesigner> m_Nodes = new List<NodeDesigner>();


        public string ID
        {
            get { return m_TreeID; }
            set { m_TreeID = value; }
        }

        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
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

        public List<VariableFieldDesigner> BehaviorTreeVariableFields
        {
            get { return m_BehaviorTreeVariables; }
            set { m_BehaviorTreeVariables = value; }
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
            if (node == null)
                return;

            //删除指向该节点的父节点
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node_i = Nodes[i];

                if (node_i.ID != node.ID && node_i.Transitions.Count > 0)
                {
                    for (int j = 0; j < node_i.Transitions.Count; j++)
                    {
                        Transition transition = node_i.Transitions[j];
                        if (transition.ToNodeID == node.ID)
                        {
                            node.ParentNode = null;
                            node_i.Transitions.RemoveAt(j);
                            break;
                        }
                    }
                }
            }

            if (node.Transitions.Count > 0)
            {
                for (int i = 0; i < node.Transitions.Count; i++)
                {
                    Transition transition = node.Transitions[i];
                    NodeDesigner childNode = FindByID(transition.ToNodeID);
                    if (childNode != null)
                    {
                        childNode.ParentNode = null;
                    }
                }
            }

            Nodes.Remove(node);
        }

        //更换开始节点
        public void ChangeStartNode(NodeDesigner node)
        {
            if (node == null)
                return;

            if (node.NodeType != NodeType.Composite)
                return;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner tempNode = m_Nodes[i];
                tempNode.StartNode = false;
                if (tempNode == node || tempNode.ID == node.ID)
                {
                    if (tempNode.ParentNode != null)
                    {
                        for (int j = 0; j < tempNode.ParentNode.Transitions.Count; j++)
                        {
                            Transition transition = tempNode.ParentNode.Transitions[j];
                            if (transition.ToNode == tempNode)
                            {
                                RemoveTranstion(transition);
                                break;
                            }
                        }
                    }

                    tempNode.StartNode = true;
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

        /// <summary>
        /// 获取开始节点
        /// </summary>
        /// <returns></returns>
        public NodeDesigner GetStartNode()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.StartNode)
                    return node;
            }
            return null;
        }

        /// <summary>
        /// 是否存在开始节点
        /// </summary>
        /// <returns></returns>
        public bool ExistStartNode()
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node != null && node.StartNode)
                    return true;
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

        /// <summary>
        /// 查找指定节点的父节点
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public NodeDesigner FindParentNode(int ID)
        {
            for (int i = 0; i < Nodes.Count; i++)
            {
                NodeDesigner node = Nodes[i];
                if (node.Transitions.Count > 0)
                {
                    for (int ii = 0; ii < node.Transitions.Count; ii++)
                    {
                        Transition transition = node.Transitions[ii];
                        if (transition.ToNodeID == ID)
                            return node;
                    }
                }
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
                    MainForm.Instance.ShowInfo(string.Format("字段名字{0}相同,添加失败！！！", temp.FieldName));
                    MainForm.Instance.ShowMessage(string.Format("字段名字{0}相同,添加失败！！！", temp.FieldName), "警告");
                    return false;
                }
            }

            m_Fields.Add(field);

            return true;
        }

        /// <summary>
        /// 添加行为树变量
        /// </summary>
        /// <param name="field">行为树变量</param>
        /// <returns></returns>
        public bool AddBehaviorTreeVar(VariableFieldDesigner field)
        {
            if (field == null)
            {
                return false;
            }

            if (field.VariableFieldType == FieldType.None)
            {
                MainForm.Instance.ShowInfo("行为树变量类型为None,添加失败！！！");
                MainForm.Instance.ShowMessage("行为树变量类型为None,添加失败！！！", "警告");
                return false;
            }

            if (string.IsNullOrEmpty(field.VariableFieldName))
            {
                MainForm.Instance.ShowInfo("行为树变量名为空,添加失败！！！");
                MainForm.Instance.ShowMessage("行为树变量名为空,添加失败！！！", "警告");
                return false;
            }

            for (int i = 0; i < m_BehaviorTreeVariables.Count; i++)
            {
                VariableFieldDesigner temp = m_BehaviorTreeVariables[i];
                if (temp.VariableFieldName == field.VariableFieldName)
                {
                    MainForm.Instance.ShowInfo(string.Format("行为树变量名字{0}相同,添加失败！！！", temp.VariableFieldName));
                    MainForm.Instance.ShowMessage(string.Format("行为树变量名字{0}相同,添加失败！！！", temp.VariableFieldName), "警告");
                    return false;
                }
            }

            m_BehaviorTreeVariables.Add(field);

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
        /// 判断行为树变量是否已存在
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool ExistBehaviorTreeVar(string varName)
        {
            for (int i = 0; i < m_BehaviorTreeVariables.Count; i++)
            {
                VariableFieldDesigner temp = m_BehaviorTreeVariables[i];
                if (temp.VariableFieldName == varName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检验行为树是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyBehaviorTreeID()
        {
            if (string.IsNullOrEmpty(m_TreeID))
                return new VerifyInfo("m_TreeID为空");
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
                    return new VerifyInfo(string.Format("行为树[{0}]\n存在空字段名", m_TreeID));
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
                        return new VerifyInfo(string.Format("行为树[{0}]\n存在重复字段:{1}", m_TreeID, field_ii.FieldName));
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
                            return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型为空", m_TreeID, field.FieldName));
                        }

                        CustomEnum customEnum = MainForm.Instance.NodeTemplate.FindEnum(enumFieldDesigner.EnumType);
                        if (customEnum == null)
                        {
                            return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型[{2}]不存在", m_TreeID, field.FieldName, enumFieldDesigner.EnumType));
                        }
                        else
                        {
                            EnumItem enumItem = customEnum.FindEnum(enumFieldDesigner.Value);
                            if (enumItem == null)
                                return new VerifyInfo(string.Format("行为树[{0}]的字段[{1}]的枚举类型[{2}]\n不存在选项[{3}]", m_TreeID, field.FieldName, customEnum.EnumType, enumFieldDesigner.Value));
                        }
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 检验开始节点，必须拥有开始节点，并且只有一个
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyStartNode()
        {
            int startNodeCount = 0;
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner node = m_Nodes[i];
                if (node != null && node.StartNode)
                {
                    startNodeCount++;
                }
            }

            if (startNodeCount == 0)
            {
                return new VerifyInfo(string.Format("行为树[{0}]不存在开始节点", m_TreeID));
            }
            else if (startNodeCount > 1)
            {
                return new VerifyInfo(string.Format("行为树[{0}]拥有多个开始节点，请保证只有一个", m_TreeID));
            }

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 检验节点的父节点是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyParentNode()
        {
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner node = m_Nodes[i];
                NodeDesigner parentNode = FindParentNode(node.ID);

                if (node.StartNode)
                {
                    if (parentNode != null)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的开始节\n不能有父节点，请删除开始节点的父节点", m_TreeID));
                    }
                }
                else
                {
                    if (parentNode == null)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的节点[{1}]\n没有父节点，请添加父节点", m_TreeID, node.ClassType));
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }


        /// <summary>
        /// 检验节点的子节点是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyChildNode()
        {
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner node = m_Nodes[i];
                if (node.StartNode)
                {
                    if (node.Transitions.Count == 0)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的开始节不存在子节点", m_TreeID));
                    }
                }
                else if (node.NodeType == NodeType.Composite)
                {
                    if (node.Transitions.Count == 0)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的组合节点[{1}]没有子节点，请添加子节点", m_TreeID, node.ClassType));
                    }
                }
                else if (node.NodeType == NodeType.Decorator)
                {
                    if (node.Transitions.Count == 0)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的装饰节点[{1}]没有子节点，请添加一个子节点", m_TreeID, node.ClassType));
                    }
                    else if (node.Transitions.Count > 1)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的装饰节点[{1}]添加了多个子节点，请保证有且只有一个子节点", m_TreeID, node.ClassType));
                    }
                }
                else if (node.NodeType == NodeType.Action)
                {
                    if (node.Transitions.Count > 0)
                    {
                        return new VerifyInfo(string.Format("行为树[{0}]的动作节点[{1}]存在子节点，请删除所有子节点", m_TreeID, node.ClassType));
                    }
                }
            }

            return VerifyInfo.DefaultVerifyInfo;
        }


        /// <summary>
        /// 校验行为树是否合法
        /// </summary>
        /// <returns></returns>
        public VerifyInfo VerifyBehaviorTree()
        {
            //检验ID是否合法
            VerifyInfo verifyBehaviroTreeID = VerifyBehaviorTreeID();
            if (verifyBehaviroTreeID.HasError)
                return verifyBehaviroTreeID;

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

            //校验开始节点
            VerifyInfo verifyStartNode = VerifyStartNode();
            if (verifyStartNode.HasError)
                return verifyStartNode;

            //校验父节点
            VerifyInfo verifyParentNode = VerifyParentNode();
            if (verifyParentNode.HasError)
                return verifyParentNode;

            //检验子节点
            VerifyInfo verifyChildNode = VerifyChildNode();
            if (verifyChildNode.HasError)
                return verifyChildNode;

            return VerifyInfo.DefaultVerifyInfo;
        }

        /// <summary>
        /// 更新BehaviorTree内容，但Nodes不能改
        /// </summary>
        /// <param name="behaviorTree"></param>
        public void UpdateBehaviorTree(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == this)
                return;

            m_TreeID = behaviorTree.ID;
            m_Describe = behaviorTree.Describe;

            m_Fields.Clear();
            m_Fields.AddRange(behaviorTree.Fields.ToArray());

            m_BehaviorTreeVariables.Clear();
            m_BehaviorTreeVariables.AddRange(behaviorTree.BehaviorTreeVariableFields.ToArray());
        }

        /// <summary>
        /// 移除未定义的节点
        /// </summary>
        public bool RemoveUnDefineNode()
        {
            bool remove = false;

            for (int i = m_Nodes.Count - 1; i >= 0; i--)
            {
                NodeDesigner node = m_Nodes[i];
                NodeDefine nodeDefine = MainForm.Instance.NodeTemplate.FindNode(node.ClassType);
                if (nodeDefine == null)
                {
                    RemoveNode(node);
                    remove = true;
                }
            }

            return remove;
        }

        /// <summary>
        /// 修正数据
        /// </summary>
        public bool AjustData()
        {
            bool ajust = false;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner node = m_Nodes[i];
                NodeDefine nodeDefine = MainForm.Instance.NodeTemplate.FindNode(node.ClassType);

                //修正节点标签
                if (node.Label != nodeDefine.Label)
                {
                    node.Label = nodeDefine.Label;
                    ajust = true;
                }

                //移除模板中没有的字段
                for (int ii = node.Fields.Count - 1; ii >= 0; ii--)
                {
                    FieldDesigner field = node.Fields[ii];
                    if (!nodeDefine.ExistFieldName(field.FieldName))
                    {
                        ajust = true;
                        node.Fields.RemoveAt(ii);
                    }
                }

                //修正类型不匹配的(节点字段和模板字段类型不匹配)
                for (int ii = 0; ii < node.Fields.Count; ii++)
                {
                    FieldDesigner field = node.Fields[ii];
                    NodeField nodeField = nodeDefine.FindField(field.FieldName);
                    if (field.FieldType != nodeField.FieldType)
                    {
                        //重新给默认值
                        node.Fields[ii] = EditorUtility.CreateFieldByNodeField(nodeField);
                        ajust = true;
                    }
                }

                //添加不存的字段
                for (int ii = nodeDefine.Fields.Count - 1; ii >= 0; ii--)
                {
                    NodeField nodeField = nodeDefine.Fields[ii];
                    FieldDesigner field = node.FindFieldByName(nodeField.FieldName);
                    //不存在的字段要添加
                    if (field == null)
                    {
                        FieldDesigner newField = EditorUtility.CreateFieldByNodeField(nodeField);
                        node.AddField(newField);
                        ajust = true;
                    }
                }

                //排序字段（要和模板中一致）
                for (int ii = 0; ii < nodeDefine.Fields.Count; ii++)
                {
                    NodeField nodeField = nodeDefine.Fields[ii];
                    int index = node.GetFieldIndex(nodeField.FieldName);
                    if (index != ii)
                    {
                        //交换
                        FieldDesigner tempField_ii = node.Fields[ii];
                        node.Fields[ii] = node.Fields[index];
                        node.Fields[index] = tempField_ii;
                        ajust = true;
                    }
                }

                //修正Label
                for (int ii = 0; ii < node.Fields.Count; ii++)
                {
                    FieldDesigner field = node.Fields[ii];
                    NodeField nodeField = nodeDefine.Fields[ii];
                    if (field.Label != nodeField.Label)
                    {
                        field.Label = nodeField.Label;
                        ajust = true;
                    }
                }
            }

            return ajust;
        }

        /// <summary>
        /// 修改节点名字
        /// </summary>
        /// <param name="old">旧的classType</param>
        /// <param name="newType">新的classType</param>
        public void UpdateClassType(string old, string newType)
        {
            for (int i = 0; i < m_Nodes.Count; i++)
            {
                NodeDesigner node = m_Nodes[i];
                if (node.ClassType == old)
                {
                    node.ClassType = newType;
                }
            }
        }
    }
}
