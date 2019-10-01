using System.Collections;
using System.Collections.Generic;
using R7BehaviorTree;
using UnityEngine;

public class Game : MonoBehaviour, IContext
{
    // Use this for initialization
    void Start()
    {
        //BehaviorTree behaviorTree = BehaviorTreeManager.Instance.CreateBehaviorTree((int)EBehaviorTreeType.Game, "AI1", this);
        //BehaviorTreeManager.Instance.RunBehaviorTree(behaviorTree);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
