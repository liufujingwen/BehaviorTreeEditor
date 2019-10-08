using System;
using UnityEngine;

public class UnitStateBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnitManager.Instance.NotifyAnimationStateChange(animator, stateInfo);
    }
}
