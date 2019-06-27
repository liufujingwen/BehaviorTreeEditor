using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTreeEditor
{
    public class AgentDesigner
    {
        private List<FieldDesigner> m_Fields = new List<FieldDesigner>();

        public List<FieldDesigner> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }


        public List<NodeDesigner> Nodes = new List<NodeDesigner>();

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

        public void AddNode(NodeDesigner node)
        {
            if (node == null)
                return;

            if (Exist(node))
            {
                throw new Exception(string.Format("已存在节点id:{0},name:{1}", node.ID, node.Name));
            }

            Nodes.Add(node);
        }

        public void Remove(NodeDesigner node)
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

        public bool Exist(NodeDesigner node)
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
    }
}