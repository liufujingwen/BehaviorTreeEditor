using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BT.Editor
{
    [System.Serializable]
    public class ShortcutEditor
    {
        public void DoGUI(Rect position)
        {
            if (PreferencesEditor.GetBool(Preference.ShowShortcuts))
            {
                GUILayout.BeginArea(position);
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical((GUIStyle)"U2D.createRect");
                ShortcutGUI("保存", "saveAll", KeyCode.F1);
                ShortcutGUI("显示帮助", "showHelp", KeyCode.F2);
                ShortcutGUI("选择全部", "selectAll", KeyCode.F3);
                ShortcutGUI("刷新", "fresh", KeyCode.F4);
                ShortcutGUI("居中", "centerView", KeyCode.Tab);
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        private void ShortcutGUI(string title, string key, KeyCode defaultValue)
        {
            FirstKey first = (FirstKey)EditorPrefs.GetInt(key + "1", (int)FirstKey.None);
            string shortcut = first != FirstKey.None ? first.ToString() + "+" : "";

            shortcut = shortcut + ((KeyCode)EditorPrefs.GetInt(key + "2", (int)defaultValue)).ToString();
            GUILayout.BeginHorizontal();
            GUILayout.Label(title, BehaviorTreeEditorStyles.shortcutLabel, GUILayout.Width(130));
            GUILayout.Label(shortcut, BehaviorTreeEditorStyles.shortcutLabel);
            GUILayout.EndHorizontal();
        }

        public void HandleKeyEvents()
        {
            if (BehaviorTreeEditor.Instance == null || !PreferencesEditor.GetBool(Preference.EnableShortcuts))
            {
                return;
            }
            Event ev = Event.current;
            switch (ev.type)
            {
                case EventType.KeyUp:
                    DoEvents(ev, false);
                    break;
                case EventType.MouseUp:
                    DoEvents(ev, true);
                    break;
            }
        }

        private void DoEvents(Event ev, bool isMouse)
        {
            if (Validate("saveAll", KeyCode.F1, isMouse))
            {
                BehaviorTreeEditor.Instance.Save();
                ev.Use();
            }

            if (Validate("showHelp", KeyCode.F2, isMouse))
            {
                PreferencesEditor.ToggleBool(Preference.ShowShortcuts);
                ev.Use();
            }

            if (Validate("selectAll", KeyCode.F3, isMouse))
            {
                BehaviorTreeEditor.Instance.ToggleSelection();
                ev.Use();
            }

            if (Validate("centerView", KeyCode.Tab, isMouse))
            {
                BehaviorTreeEditor.Instance.CenterView(); 
                ev.Use();
            }

            //保存并且刷新
            if (Validate("fresh", KeyCode.F4, isMouse))
            {
                if (BehaviorTreeEditor.Instance.Save())
                {
                    BehaviorTreeEditor.Instance.Fresh();
                }
                ev.Use();
            }
        }

        private bool Validate(string key, KeyCode defaultKey, bool isMouse)
        {
            return ControlPressed(key) && KeyPressed(key, defaultKey, isMouse);
        }

        private bool KeyPressed(string key, KeyCode defaultKey, bool isMouse)
        {
            return (Event.current.keyCode == (KeyCode)EditorPrefs.GetInt(key + "2", (int)defaultKey)) || isMouse && (KeyCode)EditorPrefs.GetInt(key + "2", (int)defaultKey) == KeyCode.Mouse0;
        }

        private bool ControlPressed(string key)
        {
            FirstKey firstKey = (FirstKey)EditorPrefs.GetInt(key + "1", (int)FirstKey.None);
            switch (firstKey)
            {
                case FirstKey.Alt:
                    return Event.current.alt;
                case FirstKey.Control:
                    return Event.current.control;
                case FirstKey.Shift:
                    return Event.current.shift;
            }
            return true;
        }

    }
}