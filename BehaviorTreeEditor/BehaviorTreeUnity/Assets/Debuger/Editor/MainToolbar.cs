using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BT.Debuger
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
        }

        public void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar); 
           
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(FsmContent.centerButtonStr, EditorStyles.toolbarButton,GUILayout.Width(60)))
            {
                BehaviorTreeEditor.Instance.CenterView();
            }
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }
    }
}