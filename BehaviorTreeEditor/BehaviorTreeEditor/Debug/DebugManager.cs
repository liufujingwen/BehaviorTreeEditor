using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private List<DebugNode> m_Nodes = new List<DebugNode>();

        public bool Debugging
        {
            get { return State != DebugState.None; }
        }

        public void Debug(AgentDesigner agent)
        {
            m_Nodes.Clear();
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
            ContentUserControl.Instance.OnDebugStart();
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
                //概率选择
                else if (node.ClassType == "RateSelector")
                {
                    DebugRateSelector debugRateSelector = new DebugRateSelector();
                    debugNode = debugRateSelector;
                }
                else
                {
                    //统一用默认的组合节点，（顺序节点）
                    DebugCompositeNode debugCompositeNode = new DebugCompositeNode();
                    debugNode = debugCompositeNode;
                }
            }
            //装饰节点
            else if (node.NodeType == NodeType.Decorator)
            {
                //失败节点
                if (node.ClassType == "Failure")
                {
                    DebugFailureNode debugFailureNode = new DebugFailureNode();
                    debugNode = debugFailureNode;
                }
                //成功节点
                else if (node.ClassType == "Success")
                {
                    DebugSuccessNode debugSuccessNode = new DebugSuccessNode();
                    debugNode = debugSuccessNode;
                }
                //执行帧节点
                else if (node.ClassType == "Frames")
                {
                    DebugFramesNode debugFramesNode = new DebugFramesNode();
                    debugNode = debugFramesNode;
                }
                //输出log节点
                else if (node.ClassType == "Log")
                {
                    DebugLogNode debugLogNode = new DebugLogNode();
                    debugNode = debugLogNode;
                }
                //循环节点
                else if (node.ClassType == "Loop")
                {
                    DebugLoopNode debugLoopNode = new DebugLoopNode();
                    debugNode = debugLoopNode;
                }
                //取反节点
                else if (node.ClassType == "Not")
                {
                    DebugNotNode debugNotNode = new DebugNotNode();
                    debugNode = debugNotNode;
                }
                //指定时间内运行
                else if (node.ClassType == "Time")
                {
                    DebugTimeNode debugTimeNode = new DebugTimeNode();
                    debugNode = debugTimeNode;
                }
                //等待直到子节点返回成功
                else if (node.ClassType == "WaitUntil")
                {
                    DebugWaitUntilNode debugWaitUntilNode = new DebugWaitUntilNode();
                    debugNode = debugWaitUntilNode;
                }
                else
                {
                    DebugDecoratorNode debugDecoratorNode = new DebugDecoratorNode();
                    debugDecoratorNode.CanChangeStatus = true;
                    debugNode = debugDecoratorNode;
                }
            }
            //条件节点
            else if (node.NodeType == NodeType.Condition)
            {
                DebugConditionNode debugConditionNode = new DebugConditionNode();
                debugConditionNode.CanChangeStatus = true;
                debugNode = debugConditionNode;
            }
            //动作节点
            else if (node.NodeType == NodeType.Action)
            {
                //等待一段时间
                if (node.ClassType == "Wait")
                {
                    DebugNodeWait debugActionNode = new DebugNodeWait();
                    debugNode = debugActionNode;
                }
                else
                {
                    DebugActionNode debugActionNode = new DebugActionNode();
                    debugNode = debugActionNode;
                    debugNode.CanChangeStatus = true;
                }
            }

            debugNode.Node = node;
            m_Nodes.Add(debugNode);

            for (int i = 0; i < node.Transitions.Count; i++)
            {
                Transition transition = node.Transitions[i];
                NodeDesigner childNode = agent.FindByID(transition.ToNodeID);
                DebugNode childDebugNode = CreateDebugNode(agent, childNode);
                childDebugNode.ParentNode = debugNode;
                debugNode.Childs.Add(childDebugNode);
            }

            return debugNode;
        }

        public DebugNode FindByID(int ID)
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

        public void DoTransitions(Graphics graphics, Vec2 offset)
        {
            if (State == DebugState.None)
                return;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                DebugNode node = m_Nodes[i];
                if (node.Node.ParentNode == null)
                    continue;
                if (node.Status == DebugNodeStatus.None)
                    continue;
                if (node.Status == DebugNodeStatus.Transition)
                {
                    BezierLink.DrawNodeToNode_Debug(graphics, node.ParentNode, node, offset);
                }
                else
                {
                    BezierLink.DrawNodeToNode_Debug(graphics, node.ParentNode, node, offset);
                }
            }
        }

        public void DoNodes(Graphics graphics, Vec2 offset, float deltatime)
        {
            if (State == DebugState.None)
                return;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                DebugNode debugNode = m_Nodes[i];
                EditorUtility.Draw_Debug(debugNode, graphics, offset, deltatime);
            }
        }

        public void Stop()
        {
            if (State == DebugState.None)
                return;

            m_DebugNode = null;
            m_Nodes.Clear();
            State = DebugState.None;
            MainForm.Instance.ShowInfo("停止成功 时间:" + DateTime.Now);
        }
    }
}
