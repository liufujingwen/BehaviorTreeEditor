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

        private List<NodeDesigner> selection = new List<NodeDesigner>();

        public static List<NodeDesigner> SelectedNodes
        {
            get
            {
                if (BehaviorTreeEditor.Instance != null)
                {
                    return BehaviorTreeEditor.Instance.selection;
                }
                return null;
            }
        }

        private bool centerView;
        private Vector2 selectionStartPosition;
        private SelectionMode selectionMode;
        private NodeDesigner fromNode;
        private Rect propertyRect;
        private Rect preferencesRect;
        private Rect shortcutRect;
        private MainToolbar mainToolbar;
        private ShortcutEditor shortcutEditor;
        private Transition selectedTransition;

        public static BehaviorTreeEditor ShowWindow()
        {
            BehaviorTreeEditor window = EditorWindow.GetWindow<BehaviorTreeEditor>("BehaviorTree");

            if (TreeData == null)
                LoadBehaviorTreeData();
            return window;
        }

        public static void LoadBehaviorTreeData()
        {
            string path = @"C:\Users\Administrator\Desktop\Test\BehaviorTreeData.xml";
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

                    //if (SelectedNodes.Count == 1 && ev.rawType == EventType.KeyDown && ev.keyCode == KeyCode.Delete && BehaviorTreeEditor.SelectedTransition != null && EditorUtility.DisplayDialog("Delete selected transition?", BehaviorTreeEditor.SelectedTransition.FromNode.Name + " -> " + BehaviorTreeEditor.SelectedTransition.ToNode.Name + "\r\n\r\nYou cannot undo this action.", "Delete", "Cancel"))
                    //{
                    //BaseNodeDesigner node = SelectedTransition.fromNode;
                    //node.Transitions = ArrayUtility.Remove(node.Transitions, BehaviorTreeEditor.SelectedTransition);
                    //FsmEditorUtility.DestroyImmediate(BehaviorTreeEditor.SelectedTransition);
                    ////ErrorChecker.CheckForErrors();
                    //EditorUtility.SetDirty(node);
                    //}
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

        private void DoNodeEvents()
        {
            if (currentEvent.button != 0)
            {
                return;
            }
            SelectNodes();
        }

        private void SelectNodes()
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (currentEvent.rawType)
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlID;
                    selectionStartPosition = mousePosition;
                    NodeDesigner node = MouseOverNode();
                    if (node == null)
                    {
                        this.selection.Clear();
                        UpdateUnitySelection();
                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                        Event.current.Use();
                        return;
                    }
                    if (node != null)
                    {
                        if (fromNode != null)
                        {
                            fromNode = null;
                            GUIUtility.hotControl = 0;
                            GUIUtility.keyboardControl = 0;
                            return;
                        }


                        if (EditorGUI.actionKey || currentEvent.shift)
                        {
                            if (!this.selection.Contains(node))
                            {
                                this.selection.Add(node);
                            }
                            else
                            {
                                this.selection.Remove(node);
                            }
                        }
                        else if (!this.selection.Contains(node))
                        {
                            this.selection.Clear();
                            this.selection.Add(node);
                        }


                        GUIUtility.hotControl = 0;
                        GUIUtility.keyboardControl = 0;
                        UpdateUnitySelection();
                        return;
                    }
                    fromNode = null;
                    selectionMode = SelectionMode.Pick;
                    if (!EditorGUI.actionKey && !currentEvent.shift)
                    {
                        this.selection.Clear();
                        UpdateUnitySelection();
                    }
                    currentEvent.Use();
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        selectionMode = SelectionMode.None;
                        GUIUtility.hotControl = 0;
                        if (currentEvent != null)
                            currentEvent.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID && !EditorGUI.actionKey && !currentEvent.shift && (selectionMode == SelectionMode.Pick || selectionMode == SelectionMode.Rect))
                    {
                        selectionMode = SelectionMode.Rect;
                        SelectNodesInRect(FromToRect(selectionStartPosition, mousePosition));
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    if (GUIUtility.hotControl == controlID && selectionMode == SelectionMode.Rect)
                    {
                        BehaviorTreeEditorStyles.selectionRect.Draw(FromToRect(selectionStartPosition, mousePosition), false, false, false, false);
                    }
                    break;
            }
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

            if (selection.Count > 0)
            {
                for (int i = 0; i < selection.Count; i++)
                {
                    NodeDesigner node = selection[i];
                    DoNode(node, true);
                }
            }

            DoNodeEvents();
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

            if (SelectedNodes.Count == 1)
            {
            }

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

        private void SelectNodesInRect(Rect r)
        {
            for (int i = 0; i < Agent.Nodes.Count; i++)
            {
                NodeDesigner node = Agent.Nodes[i];
                Rect rect = node.Rect;
                if (rect.xMax < r.x || rect.x > r.xMax || rect.yMax < r.y || rect.y > r.yMax)
                {
                    selection.Remove(node);
                    continue;
                }
                if (!selection.Contains(node))
                {
                    selection.Add(node);
                }
            }
            UpdateUnitySelection();
        }

        private Rect FromToRect(Vector2 start, Vector2 end)
        {
            Rect rect = new Rect(start.x, start.y, end.x - start.x, end.y - start.y);
            if (rect.width < 0f)
            {
                rect.x = rect.x + rect.width;
                rect.width = -rect.width;
            }
            if (rect.height < 0f)
            {
                rect.y = rect.y + rect.height;
                rect.height = -rect.height;
            }
            return rect;
        }

        private void UpdateUnitySelection()
        {
            //Selection.objects = selection1.ToArray();
        }

        public void Fresh()
        {
        }

        public void ToggleSelection()
        {
            if (selection.Count == Agent.Nodes.Count)
            {
                selection.Clear();
            }
            else
            {
                selection.Clear();
                selection.AddRange(Agent.Nodes);
            }
            UpdateUnitySelection();
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

        public enum SelectionMode
        {
            None,
            Pick,
            Rect,
        }
    }
}
