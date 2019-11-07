using R7BehaviorTree;
using BTData;
using BehaviorTreeViewer;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace R7BehaviorTreeDebugger
{
    public class BehaviorTreeEditor : NodeEditor
    {
        public static BehaviorTreeEditor Instance;

        private Debugger m_Debugger;

        public BehaviorTreeEditor()
        {
        }

        public void SetBehaviorTree(BehaviorTree behaviorTree)
        {
            m_Debugger = new Debugger();
            CreateNode(m_Debugger, behaviorTree.StartNode);
            CenterView();
        }

        private NodeDesigner CreateNode(Debugger designer, BaseNode node)
        {
            NodeDesigner nodeDesigner = new NodeDesigner();
            nodeDesigner.baseNode = node;
            nodeDesigner.NodeData = node.NodeData;
            nodeDesigner.Rect = new Rect(nodeDesigner.NodeData.X, nodeDesigner.NodeData.Y, BehaviorTreeEditorStyles.StateWidth, BehaviorTreeEditorStyles.StateHeight);
            designer.Nodes.Add(nodeDesigner);

            if (node is CompositeNode)
            {
                CompositeNode compositeNode = node as CompositeNode;
                for (int i = 0; i < compositeNode.Childs.Count; i++)
                {
                    BaseNode childNode = compositeNode.Childs[i];
                    NodeDesigner childNodeDesigner = CreateNode(designer, childNode);
                    Transition transition = new Transition();
                    transition.FromNode = nodeDesigner;
                    transition.ToNode = childNodeDesigner;
                    nodeDesigner.Transitions.Add(transition);
                }
            }

            return nodeDesigner;
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
            if (m_Debugger == null)
                return null;

            for (int i = 0; i < m_Debugger.Nodes.Count; i++)
            {
                NodeDesigner nodeDesigner = m_Debugger.Nodes[i];
                if (nodeDesigner != null && nodeDesigner.ID == id)
                    return nodeDesigner;
            }

            return null;
        }

        private bool centerView;
        private NodeDesigner fromNode;
        private MainToolbar mainToolbar;

        [MenuItem("Tools/行为树调试器")]
        public static BehaviorTreeEditor ShowWindow()
        {
            BehaviorTreeEditor window = EditorWindow.GetWindow<BehaviorTreeEditor>("行为树调试器");

            List<BehaviorTree> behaviorTrees = BehaviorTreeManager.Instance.Runnings;

            if (behaviorTrees.Count > 0)
                window.SetBehaviorTree(behaviorTrees[0]);

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
            if (Application.isPlaying)
            {
                if (m_Debugger == null)
                {
                    if (BehaviorTreeManager.Instance.Runnings.Count > 0)
                        SetBehaviorTree(BehaviorTreeManager.Instance.Runnings[0]);
                }
            }
            else
            {
                m_Debugger = null;
            }

            Repaint();
        }

        protected override void OnGUI()
        {
            GetCanvasSize();

            mainToolbar.OnGUI();

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                {

                    ZoomableArea.Begin(new Rect(0, 0f, scaledCanvasSize.width, scaledCanvasSize.height + 21), scale, IsDocked);
                    Begin();

                    if (m_Debugger != null)
                    {
                        DoNodes();
                    }
                    else
                    {
                        ZoomableArea.End();
                    }
                    End();

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
            return new Rect(0, 17, position.width, position.height);
        }

        private void DoNodes()
        {
            DoTransitions();
            DoChildIndex();

            if (m_Debugger.Nodes.Count > 0)
            {
                for (int i = 0; i < m_Debugger.Nodes.Count; i++)
                {
                    NodeDesigner node = m_Debugger.Nodes[i];
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
            if (m_Debugger == null)
                return;

            if (fromNode != null)
            {
                DrawConnection(fromNode.Rect.center, mousePosition, Color.green, 1, false);
                Repaint();
            }

            for (int i = 0; i < m_Debugger.Nodes.Count; i++)
            {
                NodeDesigner node = m_Debugger.Nodes[i];
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
            if (m_Debugger == null)
                return;

            for (int i = 0; i < m_Debugger.Nodes.Count; i++)
            {
                NodeDesigner node = m_Debugger.Nodes[i];

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

            if (m_Debugger == null)
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
            for (int i = 0; i < m_Debugger.Nodes.Count; i++)
            {
                NodeDesigner node = m_Debugger.Nodes[i];
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
            if (m_Debugger == null)
                return;

            Vector3 center = Vector3.zero;
            if (m_Debugger.Nodes.Count > 0)
            {
                for (int i = 0; i < m_Debugger.Nodes.Count; i++)
                {
                    NodeDesigner node = m_Debugger.Nodes[i];
                    center += new Vector3(node.Rect.center.x - scaledCanvasSize.width * 0.5f, node.Rect.center.y - scaledCanvasSize.height * 0.5f);
                }
                center /= m_Debugger.Nodes.Count;
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
