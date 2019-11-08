using System;
using System.Collections.Generic;
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

        public NodeDesigner(string label, string classType, Rect rect)
        {
            m_Label = label;
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

        [XmlIgnore]
        public NodeDefine NodeDefine { get; set; }

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

        /// <summary>
        /// 指定点是否在控件范围内
        /// </summary>
        /// <param name="point">指定点</param>
        /// <returns>true:在控件范围内，false:不在控件范围内</returns>
        public bool IsContains(Vec2 point)
        {
            return Rect.Contains(point);
        }

        public bool ExistChildNode(NodeDesigner node)
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

        public void AddChildNode(NodeDesigner node)
        {
            if (node == null)
                return;

            if (ExistChildNode(node))
            {
                throw new Exception(string.Format("已存在节点id:{0},name:{1}", node.ID, node.ClassType));
            }

            node.ParentNode = this;
            Transition transition = new Transition();
            transition.Set(node, this);
            Transitions.Add(transition);

            Sort();
        }

        /// <summary>
        /// 通过字段名查找字段
        /// </summary>
        /// <param name="fieldName">字段名字</param>
        /// <returns></returns>
        public FieldDesigner FindFieldByName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;

            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field = m_Fields[i];
                if (field.FieldName == fieldName)
                    return field;
            }

            return null;
        }

        /// <summary>
        /// 获取字段索引
        /// </summary>
        /// <param name="fieldName">字段名字</param>
        /// <returns></returns>
        public int GetFieldIndex(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return -1;

            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field = m_Fields[i];
                if (field.FieldName == fieldName)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool AddField(FieldDesigner field)
        {
            FieldDesigner tempField = FindFieldByName(field.FieldName);
            if (tempField == null)
            {
                m_Fields.Add(field);
                return true;
            }
            return false;
        }


        //根据y值排序,y大的节点在后面
        public void Sort()
        {
            Transitions.Sort(delegate (Transition t1, Transition t2)
            {
                return t1.ToNode.Rect.y.CompareTo(t2.ToNode.Rect.y);
            });
        }

        public FieldDesigner this[string fieldName]
        {
            get
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    FieldDesigner field = Fields[i];
                    if (field == null)
                        continue;
                    if (field.FieldName == fieldName)
                        return field;
                }
                return null;
            }
        }

        public override string ToString()
        {
            string content = string.Empty;
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field = m_Fields[i];
                if (field.Field != null)
                    content += field.Field + (i != m_Fields.Count - 1 ? "," : string.Empty);
            }
            return content;
        }

        public string ShowContent()
        {
            if (NodeDefine == null)
                NodeDefine = MainForm.Instance.NodeTemplate.FindNode(ClassType);

            if (NodeDefine == null)
                throw new Exception(ClassType + "的" + nameof(NodeDefine) + "为空");

            string content = string.Empty;

            int showCount = 0;
            for (int i = 0; i < m_Fields.Count; i++)
            {
                FieldDesigner field = m_Fields[i];
                NodeField nodeField = NodeDefine.Fields[i];
                if (nodeField.Show && field.Field != null)
                {
                    if (showCount == 0)
                        content += "[";
                    else
                        content += ",";

                    showCount++;
                    content += field.Field;
                }
            }

            if (showCount > 0)
                content += "]";
            else
                content = Describe;

            return content;
        }
    }
}
