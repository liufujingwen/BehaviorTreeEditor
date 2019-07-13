using BehaviorTreeViewer;
using BT.Editor;
using System;
using System.IO;
using UnityEngine;

namespace BT
{
    public static class BehaviorTreeUtility
    {
        public static string BehaviourTreeFile = "D:\\test.xml";
        public static string SkillSavePath = "";

        static BehaviorTreeUtility()
        {
            BehaviourTreeFile = Application.dataPath + "/Skills.xml";
            SkillSavePath = Application.dataPath + "../../../../cbin/resources/skill/";
        }

        //加载行为树
        public static TreeData Load()
        {
            TreeData data = null;
            if (File.Exists(BehaviourTreeFile))
            {
                try
                {
                    data = XmlUtility.Read<TreeData>(BehaviourTreeFile);

                    //处理父节点、子节点关系
                    for (int i = 0; i < data.Agents.Count; i++)
                    {
                        AgentDesigner agent = data.Agents[i];
                        if (agent != null)
                        {
                            for (int j = 0; j < agent.Nodes.Count; j++)
                            {
                                NodeDesigner node = agent.Nodes[j];
                                if (node.Transitions.Count > 0)
                                {
                                    for (int k = 0; k < node.Transitions.Count; k++)
                                    {
                                        Transition transition = node.Transitions[k];
                                        NodeDesigner fromNode = agent.FindNodeByID(transition.FromNodeID);
                                        NodeDesigner toNode = agent.FindNodeByID(transition.ToNodeID);
                                        transition.Set(toNode, fromNode);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
            }
           
            return data;
        }
    }
}