using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BT.Editor
{
    [System.Serializable]
    public class MainToolbar
    {
        [SerializeField]
        private bool showPreferences;

        public MainToolbar()
        {
        }

        public void OnEnable()
        {
            showPreferences = PreferencesEditor.GetBool(Preference.ShowPreference);
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar); 
           
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("工具", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
            {
                GenericMenu menu = new GenericMenu();

                menu.AddItem(new GUIContent("快捷键设置"), false, delegate ()
                {
                    SetupShortcutsEditor.ShowWindow();
                });

                menu.ShowAsContext();
            }

            if (GUILayout.Button(FsmContent.centerButtonStr, EditorStyles.toolbarButton,GUILayout.Width(60)))
            {
                BehaviorTreeEditor.Instance.CenterView();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(FsmContent.saveStr, EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                BehaviorTreeEditor.Instance.Save();
            }

            if (GUILayout.Button(BehaviorTreeEditorStyles.popupIcon, (showPreferences ? (GUIStyle)"TE toolbarbutton" : EditorStyles.toolbarButton)))
            {
                showPreferences = !showPreferences;
                PreferencesEditor.SetBool(Preference.ShowPreference, showPreferences);
            }

            GUILayout.EndHorizontal();
        }

    }
}