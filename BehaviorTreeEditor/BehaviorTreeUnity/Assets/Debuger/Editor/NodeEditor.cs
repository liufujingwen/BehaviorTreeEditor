using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT.Debuger
{
    public class NodeEditor : EditorWindow
    {
        public const float MaxCanvasSize = 50000f;
        private const float GridMinorSize = 12f;
        private const float GridMajorSize = 120f;
        public static Vector2 Center
        {
            get
            {
                return new Vector2(NodeEditor.MaxCanvasSize * 0.5f, NodeEditor.MaxCanvasSize * 0.5f);
            }
        }
        [SerializeField]
        protected Vector2 scrollPosition;
        [SerializeField]
        protected Rect canvasSize;
        [SerializeField]
        protected float scale = 1.0f;
        protected Rect scaledCanvasSize
        {
            get
            {
                return new Rect(canvasSize.x * (1f / scale), canvasSize.y * (1f / scale), canvasSize.width * (1f / scale), canvasSize.height * (1f / scale));
            }
        }

        [SerializeField]
        private Rect worldViewRect;
        [SerializeField]
        private Vector2 offset;
        protected Vector2 mousePosition;
        protected Event currentEvent;
        private Rect scrollView;
        private Material material;

        protected virtual void OnEnable()
        {
            this.scrollView = new Rect(0, 0, MaxCanvasSize, MaxCanvasSize);
            this.UpdateScrollPosition(NodeEditor.Center);
        }


        protected void Begin()
        {
            currentEvent = Event.current;
            this.canvasSize = GetCanvasSize();
            if (currentEvent.type == EventType.ScrollWheel)
            {
                Vector2 offset = (scaledCanvasSize.size - canvasSize.size) * 0.5f;
                scale -= currentEvent.delta.y / 100f;
                scale = Mathf.Clamp(scale, 0.3f, 1.0f);

                UpdateScrollPosition(scrollPosition - (scaledCanvasSize.size - canvasSize.size) * 0.5f + offset);
                Event.current.Use();
            }


            if (currentEvent.type == EventType.Repaint)
            {
                BehaviorTreeEditorStyles.canvasBackground.Draw(scaledCanvasSize, false, false, false, false);
                DrawGrid();
            }

            Vector2 curScroll = GUI.BeginScrollView(scaledCanvasSize, scrollPosition, scrollView, GUIStyle.none, GUIStyle.none);
            UpdateScrollPosition(curScroll);
            mousePosition = Event.current.mousePosition;
        }

        protected void End()
        {
            CanvasContextMenu();
            DragCanvas();
            GUI.EndScrollView();
        }

        protected virtual void OnGUI()
        {
        }

        protected virtual void CanvasContextMenu()
        {
        }

        protected virtual void TransitionContextMenu()
        {
        }

        protected virtual Rect GetCanvasSize()
        {
            return new Rect(0, 0, position.width, position.height);
        }

        protected void UpdateScrollPosition(Vector2 position)
        {
            offset = offset + (scrollPosition - position);
            scrollPosition = position;
            worldViewRect = new Rect(this.scaledCanvasSize);
            worldViewRect.y += scrollPosition.y;
            worldViewRect.x += scrollPosition.x;
        }

        private void DragCanvas()
        {
            if (currentEvent.button != 2)
            {
                return;
            }

            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (currentEvent.rawType)
            {
                case EventType.MouseDown:
                    GUIUtility.hotControl = controlID;
                    currentEvent.Use();
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID)
                    {
                        UpdateScrollPosition(scrollPosition - currentEvent.delta * (1f / scale));
                        currentEvent.Use();
                    }
                    break;
            }
        }

        private void DrawGrid()
        {
            GL.PushMatrix();
            GL.Begin(1);
            this.DrawGridLines(scaledCanvasSize, NodeEditor.GridMinorSize, offset, BehaviorTreeEditorStyles.gridMinorColor);
            this.DrawGridLines(scaledCanvasSize, NodeEditor.GridMajorSize, offset, BehaviorTreeEditorStyles.gridMajorColor);
            GL.End();
            GL.PopMatrix();
        }

        private void DrawGridLines(Rect rect, float gridSize, Vector2 offset, Color gridColor)
        {
            GL.Color(gridColor);
            for (float i = rect.x + (offset.x < 0f ? gridSize : 0f) + offset.x % gridSize; i < rect.x + rect.width; i = i + gridSize)
            {
                this.DrawLine(new Vector2(i, rect.y), new Vector2(i, rect.y + rect.height));
            }
            for (float j = rect.y + (offset.y < 0f ? gridSize : 0f) + offset.y % gridSize; j < rect.y + rect.height; j = j + gridSize)
            {
                this.DrawLine(new Vector2(rect.x, j), new Vector2(rect.x + rect.width, j));
            }
        }

        private void DrawLine(Vector2 p1, Vector2 p2)
        {
            GL.Vertex(p1);
            GL.Vertex(p2);
        }

        protected void DrawConnection(Vector3 start, Vector3 end, Color color, int arrows, bool offset)
        {
            if (currentEvent.type != EventType.Repaint)
            {
                return;
            }

            Vector3 cross = Vector3.Cross((start - end).normalized, Vector3.forward);
            if (offset)
            {
                start = start + cross * 6;
                end = end + cross * 6;
            }

            Texture2D tex = (Texture2D)UnityEditor.Graphs.Styles.connectionTexture.image;
            Handles.color = color;
            Handles.DrawAAPolyLine(tex, 5.0f, new Vector3[] { start, end });

            Vector3 vector3 = end - start;
            Vector3 vector31 = vector3.normalized;
            Vector3 vector32 = (vector3 * 0.5f) + start;
            vector32 = vector32 - (cross * 0.5f);
            Vector3 vector33 = vector32 + vector31;

            for (int i = 0; i < arrows; i++)
            {
                Vector3 center = vector33 + vector31 * 10.0f * i + vector31 * 5.0f - vector31 * arrows * 5.0f;
                DrawArrow(color, cross, vector31, center, 6.0f);
            }
        }

        private void DrawArrow(Color color, Vector3 cross, Vector3 direction, Vector3 center, float size)
        {
            Rect rect = new Rect(worldViewRect);
            rect.y -= canvasSize.y - size;
            if (!rect.Contains(center))
            {
                return;
            }
            Vector3[] vector3Array = new Vector3[] {
                center + (direction * size),
                (center - (direction * size)) + (cross * size),
                (center - (direction * size)) - (cross * size),
                center + (direction * size)
            };

            Color color1 = color;
            color1.r *= 0.8f;
            color1.g *= 0.8f;
            color1.b *= 0.8f;

            CreateMaterial();
            material.SetPass(0);

            GL.Begin(GL.TRIANGLES);
            GL.Color(color1);
            GL.Vertex(vector3Array[0]);
            GL.Vertex(vector3Array[1]);
            GL.Vertex(vector3Array[2]);
            GL.End();
        }

        private void CreateMaterial()
        {
            if (material != null)
                return;
            material = new Material(Resources.Load<Shader>("GridLine"));
            material.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}