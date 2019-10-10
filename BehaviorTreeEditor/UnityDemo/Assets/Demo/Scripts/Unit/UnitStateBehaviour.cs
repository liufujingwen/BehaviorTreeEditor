using System;
using UnityEngine;

public class UnitStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnitManager.Instance.NotifyAnimationStateEnter(animator, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnitManager.Instance.NotifyAnimationStateExit(animator, stateInfo);
    }
}
