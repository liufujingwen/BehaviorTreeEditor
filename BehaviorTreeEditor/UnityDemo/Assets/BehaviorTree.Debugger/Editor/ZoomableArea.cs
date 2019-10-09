using UnityEngine;

namespace R7BehaviorTreeDebugger
{
    public class ZoomableArea
    {
        private static Matrix4x4 prevGuiMatrix;
        private static float kEditorWindowTabHeight;

        static ZoomableArea()
        {
            ZoomableArea.kEditorWindowTabHeight = 22f;
        }

        public ZoomableArea()
        {
        }

        public static void Begin(Rect screenCoordsArea, float zoomScale, bool docked)
        {
            GUI.EndGroup();
            ZoomableArea.kEditorWindowTabHeight = (docked ? 19f : 22f);
            Rect rect = screenCoordsArea.ScaleSizeBy(1f / zoomScale, screenCoordsArea.TopLeft());
            rect.y = rect.y + ZoomableArea.kEditorWindowTabHeight;
            GUI.BeginGroup(rect);
            ZoomableArea.prevGuiMatrix = GUI.matrix;
            Matrix4x4 matrix4x4 = Matrix4x4.TRS(rect.TopLeft(), Quaternion.identity, Vector3.one);
            Vector3 vector3 = Vector3.one;
            float single = zoomScale;
            float single1 = single;
            vector3.y = single;
            vector3.x = single1;
            Matrix4x4 matrix4x41 = Matrix4x4.Scale(vector3);
            GUI.matrix = ((matrix4x4 * matrix4x41) * matrix4x4.inverse) * GUI.matrix;
        }

        public static void End()
        {
            GUI.matrix = ZoomableArea.prevGuiMatrix;
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0f, ZoomableArea.kEditorWindowTabHeight, (float)Screen.width, (float)Screen.height));
        }
    }
}