using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BT.Editor
{
    public class SetupShortcutsEditor : EditorWindow
    {
        public static void ShowWindow()
        {
            SetupShortcutsEditor window = EditorWindow.GetWindow<SetupShortcutsEditor>("快捷键设置");
            Vector2 size = new Vector2(300f, 110);
            window.minSize = size;
            UnityEngine.Object.DontDestroyOnLoad(window);
        }

        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = 130;
            DoKeyGUI("保存", "saveAll", KeyCode.F1);
            DoKeyGUI("帮助", "showHelp", KeyCode.F2);
            DoKeyGUI("选择全部", "selectAll", KeyCode.F3);
            DoKeyGUI("刷新", "fresh", KeyCode.F4);
            DoKeyGUI("居中", "centerView", KeyCode.Tab);
        }

        private void DoKeyGUI(string label, string key, KeyCode defaultValue)
        {
            GUILayout.BeginHorizontal();

            FirstKey firstKey = (FirstKey)EditorPrefs.GetInt(key + "1", (int)FirstKey.None);
            FirstKey index1 = (FirstKey)EditorGUILayout.EnumPopup(label, (FirstKey)firstKey, GUILayout.Width(200));
            if (index1 != firstKey)
            {
                EditorPrefs.SetInt(key + "1", (int)index1);
            }

            GUILayout.Label("+", GUILayout.Width(17));

            KeyCode keyCode = (KeyCode)EditorPrefs.GetInt(key + "2", (int)defaultValue);
            KeyCode index2 = (KeyCode)EditorGUILayout.EnumPopup(GUIContent.none, (KeyCode)keyCode, GUILayout.Width(70));
            if (index2 != keyCode)
            {
                EditorPrefs.SetInt(key + "2", (int)index2);
            }
            GUILayout.EndHorizontal();
        }
    }

    public enum FirstKey
    {
        None,
        Control,
        Alt,
        Shift,
    }
}