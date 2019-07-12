using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BT.Editor;

[CustomEditor(typeof(Agent))]
public class AgentInspector : Editor
{
    private Agent Agent;

    private void OnEnable()
    {
        Agent = target as Agent;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("打开调试器"))
        {
            BehaviorTreeEditor.ShowWindow();
        }
    }
}
