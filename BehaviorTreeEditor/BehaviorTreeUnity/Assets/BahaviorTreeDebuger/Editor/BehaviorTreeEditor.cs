using BehaviorTreeViewer;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace BT.Editor
{
    public class BehaviorTreeEditor : NodeEditor
    {
        public enum EditMode
        {
            Skill,
            Property,
            SkillDetail,
            Debug,
        }

        public static TreeData TreeData;
        public static BehaviorTreeEditor Instance;

        private AgentDesigner Agent;

        public AgentDesigner CurrentAgentData
        {
            get { return Agent; }
            set
            {
                Agent = value;

                if (Agent != null)
                {
                    if (Agent.Nodes.Count > 0)
                    {
                        for (int i = 0; i < Agent.Nodes.Count; i++)
                        {
                            NodeDesigner node = Agent.Nodes[i];
                            if (node.Transitions.Count > 0)
                            {
                                for (int j = 0; j < node.Transitions.Count; j++)
                                {
                                    BehaviorTreeViewer.Transition transition = node.Transitions[j];
                                    NodeDesigner fromNode = Agent.FindNodeByID(transition.FromNodeID);
                                    NodeDesigner toNode = Agent.FindNodeByID(transition.ToNodeID);
                                    transition.Set(toNode, fromNode);
                                }
                            }
                        }
                        CenterView();
                    }
                }
            }
        }

        private bool centerView;
        private NodeDesigner fromNode;
        private Rect propertyRect;
        private Rect preferencesRect;
        private Rect shortcutRect;
        private MainToolbar mainToolbar;
        private ShortcutEditor shortcutEditor;

        public static BehaviorTreeEditor ShowWindow()
        {
            BehaviorTreeEditor window = EditorWindow.GetWindow<BehaviorTreeEditor>("BehaviorTree");

            LoadBehaviorTreeData();
            window.CurrentAgentData = TreeData.Agents[0];
            return window;
        }

        public static void LoadBehaviorTreeData()
        {
            string path = @"C:\Users\Administrator\Desktop\Guide\Guide.xml";
            TreeData = XmlUtility.Read<TreeData>(path);
        }

        private void OnDestroy()
        {
            Selection.activeObject = null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            BehaviorTreeEditor.Instance = this;
            if (mainToolbar == null)
                mainToolbar = new MainToolbar();
            mainToolbar.OnEnable();

            if (shortcutEditor == null)
                shortcutEditor = new ShortcutEditor();

            centerView = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
        }

        private void Update()
        {
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
                    EventType eventType = FsmEditorUtility.ReserveEvent(preferencesRect, propertyRect);

                    ZoomableArea.Begin(new Rect(propertyRect.width, 0f, scaledCanvasSize.width, scaledCanvasSize.height + 21), scale, IsDocked);
                    Begin();

                    shortcutEditor.HandleKeyEvents();

                    if (Agent != null)
                    {
                        DoNodes();
                    }
                    else
                    {
                        ZoomableArea.End();
                    }
                    End();

                    FsmEditorUtility.ReleaseEvent(eventType);

                    preferencesRect.x -= propertyRect.width;
                    PreferencesEditor.DoGUI(preferencesRect);
                    shortcutEditor.DoGUI(shortcutRect);
                    if (centerView)
                    {
                        CenterView();
                        centerView = false;
                    }

                    GUI.Label(new Rect(5, 20, 300, 200), "Right click to create a node.", BehaviorTreeEditorStyles.instructionLabel);
                    Event ev = Event.current;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
        }

        protected override Rect GetCanvasSize()
        {
            preferencesRect = PreferencesEditor.GetBool(Preference.ShowPreference) ? new Rect(position.width - 202f, 18f, 200f, PreferencesEditor.GetHeight()) : new Rect();
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
            GUIStyle style = BehaviorTreeEditorStyles.GetNodeStyle(node, on);

            GUI.Box(node.Rect, node.Title, style);

            if (PreferencesEditor.GetBool(Preference.ShowNodeDescription))
            {
                GUILayout.BeginArea(new Rect(node.Rect.x, node.Rect.y + node.Rect.height, node.Rect.width, 500));
                GUILayout.Label(node.Title, BehaviorTreeEditorStyles.wrappedLabel);
                GUILayout.EndArea();
            }
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
                Color color = Color.white;
                DrawConnection(fromNode.Rect.center, toNode.Rect.center, color, 1, false);
            }
        }

        private void DoChildIndex()
        {
            if (Agent == null)
                return;

            if (PreferencesEditor.GetBool(Preference.ShowChildIndex))
            {
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

            Vector2 center = Vector2.zero;
            if (Agent.Nodes.Count > 0)
            {
                for (int i = 0; i < Agent.Nodes.Count; i++)
                {
                    NodeDesigner node = Agent.Nodes[i];
                    center += new Vector2(node.Rect.center.x - scaledCanvasSize.width * 0.5f, node.Rect.center.y - scaledCanvasSize.height * 0.5f);
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
