using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BehaviorTreeEditor
{
    public class DebugManager
    {
        public static DebugManager ms_Instance = new DebugManager();

        public static DebugManager Instance
        {
            get { return ms_Instance; }
        }

        public const float TransitionTime = 1.0f;

        private DebugState State = DebugState.None;

        private DebugNode m_DebugNode;

        public void Debug(AgentDesigner agent)
        {
            VerifyInfo verifyAgent = agent.VerifyAgent();
            if (verifyAgent.HasError)
            {
                MainForm.Instance.ShowMessage("确保行为树编辑正确后才能调试\n" + verifyAgent.Msg);
                return;
            }

            for (int i = 0; i < agent.Nodes.Count; i++)
            {
                NodeDesigner node = agent.Nodes[i];
                if (node.StartNode)
                {
                    m_DebugNode = CreateDebugNode(agent, node);
                    break;
                }
            }

            State = DebugState.Running;

            MainForm.Instance.ShowInfo("播放成功 时间:" + DateTime.Now);
        }

        private DebugNode CreateDebugNode(AgentDesigner agent, NodeDesigner node)
        {
            DebugNode debugNode = null;
            //组合节点
            if (node.NodeType == NodeType.Composite)
            {
                //顺序节点
                if (node.ClassType == "Sequence")
                {
                    DebugSequenceNode debugSequenceNode = new DebugSequenceNode();
                    debugNode = debugSequenceNode;
                }
                //并行节点
                else if (node.ClassType == "Parallel")
                {
                    EnumFieldDesigner successPolicy = node.Fields[0].Field as EnumFieldDesigner;
                    EnumFieldDesigner failedPolicy = node.Fields[0].Field as EnumFieldDesigner;

                    DebugParallelNode debugParallelNode = new DebugParallelNode();
                    debugParallelNode.SuccessPolicy = (DebugParallelNode.SUCCESS_POLICY)successPolicy.ValueInt;
                    debugParallelNode.FailurePolicy = (DebugParallelNode.FAILURE_POLICY)failedPolicy.ValueInt;

                    debugNode = debugParallelNode;
                }
                //随机节点
                else if (node.ClassType == "Random")
                {
                    DebugRandomNode debugRandomNode = new DebugRandomNode();
                    debugNode = debugRandomNode;
                }
                //随机序列
                else if (node.ClassType == "RandomSequence")
                {
                    DebugRandomSequence debugRandomNode = new DebugRandomSequence();
                    debugNode = debugRandomNode;
                }
                //随机选择节点
                else if (node.ClassType == "RandomSelector")
                {
                    DebugRandomSelector debugRandomNode = new DebugRandomSelector();
                    debugNode = debugRandomNode;
                }
                //ifelse节点
                else if (node.ClassType == "IfElse")
                {
                    DebugIfElseNode debugIfElseNode = new DebugIfElseNode();
                    debugNode = debugIfElseNode;
                }
                //选择节点
                else if (node.ClassType == "Selector")
                {
                    DebugSelectorNode debugIfElseNode = new DebugSelectorNode();
                    debugNode = debugIfElseNode;
                }
            }
            //装饰节点
            else if (node.NodeType == NodeType.Decorator)
            {
                DebugDecoratorNode debugDecoratorNode = new DebugDecoratorNode();
                debugNode = debugDecoratorNode;
            }
            //条件节点
            else if (node.NodeType == NodeType.Condition)
            {
                DebugConditionNode debugConditionNode = new DebugConditionNode();
                debugNode = debugConditionNode;
            }
            //动作节点
            else if (node.NodeType == NodeType.Action)
            {
                if (node.ClassType == "Wait")
                {
                    DebugNodeWait debugActionNode = new DebugNodeWait();
                    debugNode = debugActionNode;
                }
                else
                {
                    DebugActionNode debugActionNode = new DebugActionNode();
                    debugNode = debugActionNode;
                }
            }

            debugNode.Node = node;

            for (int i = 0; i < node.Transitions.Count; i++)
            {
                Transition transition = node.Transitions[i];
                NodeDesigner childNode = agent.FindByID(transition.ToNodeID);
                debugNode.Childs.Add(CreateDebugNode(agent, childNode));
            }

            return debugNode;
        }

        private DebugNode FindByID(int ID)
        {
            return FindByID(m_DebugNode, ID);
        }

        private DebugNode FindByID(DebugNode node, int ID)
        {
            if (node.Node.ID == ID)
            {
                return node;
            }
            else
            {
                if (node.Childs.Count > 0)
                {
                    for (int i = 0; i < node.Childs.Count; i++)
                    {
                        DebugNode debugNode = node.Childs[i];
                        DebugNode result = FindByID(debugNode, ID);
                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        public void Update(float deltatime)
        {
            if (State == DebugState.None)
                return;

            m_DebugNode.Update(deltatime);
        }

        public void Stop()
        {
            if (State != DebugState.None)
                return;

            State = DebugState.None;
            MainForm.Instance.ShowInfo("停止成功 时间:" + DateTime.Now);
        }
    }
}
