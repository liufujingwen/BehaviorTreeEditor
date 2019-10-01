using System;
using UnityEngine;

public class UnitStateBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private EUnitState UnitState;
    public Action<Animator, EUnitState> StateChangeHanlder;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
