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

        public void Debug(BehaviorTreeDesigner behaviorTree)
        {
            if (behaviorTree == null)
            {
                MainForm.Instance.ShowMessage("行为树为空");
                return;
            }

            m_Nodes.Clear();
            VerifyInfo verifyBehaviorTree = behaviorTree.VerifyBehaviorTree();
            if (verifyBehaviorTree.HasError)
            {
                MainForm.Instance.ShowMessage("确保行为树编辑正确后才能调试\n" + verifyBehaviorTree.Msg);
                return;
            }

            for (int i = 0; i < behaviorTree.Nodes.Count; i++)
            {
                NodeDesigner node = behaviorTree.Nodes[i];
                if (node.StartNode)
                {
                    m_DebugNode = CreateDebugNode(behaviorTree, node);
                    break;
                }
            }

            State = DebugState.Running;
            ContentUserControl.Instance.OnDebugStart();
            MainForm.Instance.ShowInfo("播放成功 时间:" + DateTime.Now);
        }

        private DebugNode CreateDebugNode(BehaviorTreeDesigner behaviorTree, NodeDesigner node)
        {
            DebugNode debugNode = null;
            //组合节点
            if (node.NodeType == NodeType.Composite)
            {
                //顺序节点
                if (node.ClassType == "Sequence")
                {
                    Debug__Sequence debug__Sequence = new Debug__Sequence();
                    debugNode = debug__Sequence;
                }
                //并行节点
                else if (node.ClassType == "Parallel")
                {
                    EnumFieldDesigner successPolicy = node.Fields[0].Field as EnumFieldDesigner;
                    EnumFieldDesigner failedPolicy = node.Fields[0].Field as EnumFieldDesigner;

                    Debug__Parallel debug__Parallel = new Debug__Parallel();
                    debug__Parallel.SuccessPolicy = (Debug__Parallel.SUCCESS_POLICY)successPolicy.ValueInt;
                    debug__Parallel.FailurePolicy = (Debug__Parallel.FAILURE_POLICY)failedPolicy.ValueInt;

                    debugNode = debug__Parallel;
                }
                //随机节点
                else if (node.ClassType == "Random")
                {
                    Debug__Random debug__Random = new Debug__Random();
                    debugNode = debug__Random;
                }
                //随机序列
                else if (node.ClassType == "RandomSequence")
                {
                    Debug__RandomSequence__Node debug__RandomSequence__Node = new Debug__RandomSequence__Node();
                    debugNode = debug__RandomSequence__Node;
                }
                //随机选择节点
                else if (node.ClassType == "RandomSelector")
                {
                    Debug__RandomSelector debug__RandomSelector = new Debug__RandomSelector();
                    debugNode = debug__RandomSelector;
                }
                //ifelse节点
                else if (node.ClassType == "IfElse")
                {
                    Debug__IfElse debug__IfElse = new Debug__IfElse();
                    debugNode = debug__IfElse;
                }
                //选择节点
                else if (node.ClassType == "Selector")
                {
                    Debug__Selector debug__Selector = new Debug__Selector();
                    debugNode = debug__Selector;
                }
                //概率选择
                else if (node.ClassType == "RateSelector")
                {
                    Debug__SelectorProbability debug__SelectorProbability = new Debug__SelectorProbability();
                    debugNode = debug__SelectorProbability;
                }
                else
                {
                    //统一用默认的组合节点，（顺序节点）
                    Debug__Composite debug__Composite = new Debug__Composite();
                    debugNode = debug__Composite;
                }
            }
            //装饰节点
            else if (node.NodeType == NodeType.Decorator)
            {
                //失败节点
                if (node.ClassType == "Failure")
                {
                    Debug__Failure debug__Failure = new Debug__Failure();
                    debugNode = debug__Failure;
                }
                //成功节点
                else if (node.ClassType == "Success")
                {
                    Debug__Success debug__Success = new Debug__Success();
                    debugNode = debug__Success;
                }
                //执行帧节点
                else if (node.ClassType == "Frames")
                {
                    Debug__Frames debug__Frames = new Debug__Frames();
                    debugNode = debug__Frames;
                }
                //循环节点
                else if (node.ClassType == "Loop")
                {
                    Debug__Loop debug__Loop = new Debug__Loop();
                    debugNode = debug__Loop;
                }
                //取反节点
                else if (node.ClassType == "Not")
                {
                    Debug__Not debug__Not = new Debug__Not();
                    debugNode = debug__Not;
                }
                //指定时间内运行
                else if (node.ClassType == "Time")
                {
                    Debug__Time debug__Time = new Debug__Time();
                    debugNode = debug__Time;
                }
                //等待直到子节点返回成功
                else if (node.ClassType == "SuccessUntil")
                {
                    Debug__SuccessUntil debug__SuccessUntil = new Debug__SuccessUntil();
                    debugNode = debug__SuccessUntil;
                }
                else
                {
                    Debug__Decorator debug__Decorator = new Debug__Decorator();
                    debug__Decorator.CanChangeStatus = true;
                    debugNode = debug__Decorator;
                }
            }
            //条件节点
            else if (node.NodeType == NodeType.Condition)
            {
                Debug__Condition debug__Condition = new Debug__Condition();
                debug__Condition.CanChangeStatus = true;
                debugNode = debug__Condition;
            }
            //动作节点
            else if (node.NodeType == NodeType.Action)
            {
                //等待一段时间
                if (node.ClassType == "Wait")
                {
                    Debug__Wait debug__Wait = new Debug__Wait();
                    debugNode = debug__Wait;
                }
                //赋值节点Int
                else if (node.ClassType == "AssignmentInt")
                {
                    Debug__AssignmentInt debug__AssignmentInt = new Debug__AssignmentInt();
                    debugNode = debug__AssignmentInt;
                }
                //赋值节点Float
                else if (node.ClassType == "AssignmentFloat")
                {
                    Debug__AssignmentFloat debug__AssignmentFloat = new Debug__AssignmentFloat();
                    debugNode = debug__AssignmentFloat;
                }
                //赋值节点String
                else if (node.ClassType == "AssignmentString")
                {
                    Debug__AssignmentString debug__AssignmentString = new Debug__AssignmentString();
                    debugNode = debug__AssignmentString;
                }
                //空操作节点
                else if (node.ClassType == "Noop")
                {
                    Debug_Noop debug_Noop = new Debug_Noop();
                    debugNode = debug_Noop;
                }
                //输出log节点
                else if (node.ClassType == "Log")
                {
                    Debug__Log debug__Log = new Debug__Log();
                    debugNode = debug__Log;
                }
                else
                {
                    Debug__Action debug__Action = new Debug__Action();
                    debugNode = debug__Action;
                    debugNode.CanChangeStatus = true;
                }
            }

            debugNode.Node = node;
            m_Nodes.Add(debugNode);

            for (int i = 0; i < node.Transitions.Count; i++)
            {
                Transition transition = node.Transitions[i];
                NodeDesigner childNode = behaviorTree.FindByID(transition.ToNodeID);
                DebugNode childDebugNode = CreateDebugNode(behaviorTree, childNode);
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
                BezierLink.DrawNodeToNode_Debug(graphics, node.ParentNode, node, offset);
            }
        }

        public void DoNodes(Graphics graphics, float deltatime)
        {
            if (State == DebugState.None)
                return;

            for (int i = 0; i < m_Nodes.Count; i++)
            {
                DebugNode debugNode = m_Nodes[i];
                EditorUtility.Draw_Debug(debugNode, graphics, deltatime);
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
