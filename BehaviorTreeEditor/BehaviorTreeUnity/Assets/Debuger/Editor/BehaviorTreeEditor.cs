using BehaviorTree;
using BehaviorTreeData;
using BehaviorTreeViewer;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace BT.Debuger
{
    public class BehaviorTreeEditor : NodeEditor
    {
        public static BehaviorTreeEditor Instance;

        private AgentDesigner Agent;

        public void SetAgent(Agent agent)
        {
            Agent = new AgentDesigner();


            //创建节点
            for (int i = 0; i < agent.BTree.AllNodes.Count; i++)
            {
                BaseNode baseNode = agent.BTree.AllNodes[i];

                NodeDesigner nodeDesigner = new NodeDesigner();
                nodeDesigner.baseNode = baseNode;
                nodeDesigner.NodeData = baseNode.Fields;
                nodeDesigner.Rect = new Rect(nodeDesigner.NodeData.X, nodeDesigner.NodeData.Y, BehaviorTreeEditorStyles.StateWidth, BehaviorTreeEditorStyles.StateHeight);
                Agent.Nodes.Add(nodeDesigner);
            }

            //初始化Transition
            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    SetTransition(node, node.NodeData);
                }
                CenterView();
            }
        }

        public void SetTransition(NodeDesigner node, NodeData nodeData)
        {
            if (nodeData.Childs != null && nodeData.Childs.Count > 0)
            {
                node.Transitions = new List<Transition>(nodeData.Childs.Count);

                for (int i = 0; i < nodeData.Childs.Count; i++)
                {
                    NodeData tempData = nodeData.Childs[i];
                    Transition transition = new Transition();
                    transition.Set(FindById(tempData.ID), node);
                    node.Transitions.Add(transition);
                }
            }
        }

        public NodeDesigner FindById(int id)
        {
            if (Agent == null)
                return null;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner nodeDesigner = Agent.Nodes[i];
                if (nodeDesigner != null && nodeDesigner.ID == id)
                    return nodeDesigner;
            }

            return null;
        }

        private bool centerView;
        private NodeDesigner fromNode;
        private Rect propertyRect;
        private Rect preferencesRect;
        private Rect shortcutRect;
        private MainToolbar mainToolbar;

        public static BehaviorTreeEditor ShowWindow(Agent agent)
        {
            BehaviorTreeEditor window = EditorWindow.GetWindow<BehaviorTreeEditor>("行为树调试器");
            window.SetAgent(agent);
            return window;
        }

        private void OnDestroy()
        {
            Selection.activeObject = null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Instance = this;
            if (mainToolbar == null)
                mainToolbar = new MainToolbar();
            mainToolbar.OnEnable();

            centerView = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
        }

        private void Update()
        {
            this.Repaint();
        }

        protected override void OnGUI()
        {
            GetCanvasSize();

            mainToolbar.OnGUI();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginHorizontal(GUILayout.Width(propertyRect.width));
                {
                    GUILayout.BeginVertical();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                {

                    ZoomableArea.Begin(new Rect(propertyRect.width, 0f, scaledCanvasSize.width, scaledCanvasSize.height + 21), scale, IsDocked);
                    Begin();

                    if (Agent != null)
                    {
                        DoNodes();
                    }
                    else
                    {
                        ZoomableArea.End();
                    }
                    End();

                    preferencesRect.x -= propertyRect.width;
                    if (centerView)
                    {
                        CenterView();
                        centerView = false;
                    }

                    //GUI.Label(new Rect(5, 20, 300, 200), "Right click to create a node.", BehaviorTreeEditorStyles.instructionLabel);
                    Event ev = Event.current;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
        }

        protected override Rect GetCanvasSize()
        {
            shortcutRect = new Rect(canvasSize.width - 250, 17, 250, canvasSize.height - 17);
            return new Rect(0, 17, position.width - propertyRect.width, position.height);
        }

        private void DoNodes()
        {
            DoTransitions();
            DoChildIndex();

            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    DoNode(node, false);
                }
            }

            ZoomableArea.End();
            NodeContextMenu();
        }

        private void DoNode(NodeDesigner node, bool on)
        {
            GUIStyle style = BehaviorTreeEditorStyles.GetNodeStyle(node);
            GUI.Box(node.Rect, node.NodeData.ClassType + ":" + node.NodeData.ID, style);
        }

        private void DoTransitions()
        {
            if (Agent == null)
                return;

            if (fromNode != null)
            {
                DrawConnection(fromNode.Rect.center, mousePosition, Color.green, 1, false);
                Repaint();
            }

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                for (int j = 0; j < node.Transitions.Count; j++)
                {
                    Transition transition = node.Transitions[j];
                    DoTransition(transition);
                }
            }
        }

        private void DoTransition(Transition trnansition)
        {
            NodeDesigner toNode = trnansition.ToNode;
            NodeDesigner fromNode = trnansition.FromNode;
            if (toNode != null && fromNode != null)
            {
                Color color = BehaviorTreeEditorStyles.GetTransition(toNode);
                DrawConnection(fromNode.Rect.center, toNode.Rect.center, color, 1, false);
            }
        }

        private void DoChildIndex()
        {
            if (Agent == null)
                return;

            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];

                if (node.Transitions.Count > 1)
                {
                    for (int j = 0; j < node.Transitions.Count; j++)
                    {
                        Transition transition = node.Transitions[j];
                        Vector3 start = transition.FromNode.Rect.center;
                        Vector3 end = transition.ToNode.Rect.center;

                        Vector3 vector3 = (end + start) * 0.5f;
                        GUI.Label(new Rect(vector3.x, vector3.y, 0, 0), j.ToString(), BehaviorTreeEditorStyles.instructionLabel);
                    }
                }

            }
        }

        protected override void CanvasContextMenu()
        {
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1)
            {
                return;
            }

            if (Agent == null)
                return;

            GenericMenu canvasMenu = new GenericMenu();
            canvasMenu.ShowAsContext();
        }

        private void NodeContextMenu()
        {
            if (currentEvent.type != EventType.MouseDown || currentEvent.button != 1 || currentEvent.clickCount != 1)
            {
                return;
            }

            NodeDesigner node = MouseOverNode();
            if (node == null)
            {
                return;
            }
            GenericMenu nodeMenu = new GenericMenu();

            nodeMenu.AddItem(FsmContent.makeTransition, false, delegate ()
            {
                fromNode = node;
            });

            nodeMenu.AddItem(FsmContent.deleteStr, false, delegate ()
            {
            });

            nodeMenu.AddSeparator("");

            nodeMenu.ShowAsContext();
            Event.current.Use();
        }

        private NodeDesigner MouseOverNode()
        {
            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                if (node.Rect.Contains(mousePosition))
                {
                    return node;
                }
            }
            return null;
        }

        private void UpdateUnitySelection()
        {
            //Selection.objects = selection1.ToArray();
        }

        public void Fresh()
        {
        }

        public void CenterView()
        {
            if (Agent == null)
                return;

            Vector3 center = Vector3.zero;
            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    center += new Vector3(node.Rect.center.x - scaledCanvasSize.width * 0.5f, node.Rect.center.y - scaledCanvasSize.height * 0.5f);
                }
                center /= Agent.Nodes.Count;
            }
            else
            {
                center = NodeEditor.Center;
            }
            UpdateScrollPosition(center);
            Repaint();
        }

        //保存
        public bool Save()
        {
            return false;
        }

        public void ShowNotification(string showStr)
        {
            ShowNotification(new GUIContent(showStr));
        }

        public static void RepaintAll()
        {
            if (Instance != null)
            {
                Instance.Repaint();
            }
        }

        public bool IsDocked
        {
            get
            {
                BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
                MethodInfo isDockedMethod = typeof(EditorWindow).GetProperty("docked", fullBinding).GetGetMethod(true);
                return (bool)isDockedMethod.Invoke(this, null);
            }
        }
    }
}
