using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using R7BehaviorTree;

public class Unit : MonoBehaviour, IContext
{
    //Unit唯一识别
    public int ID;

    //单位名称
    public string Name;

    //单位类型
    public EUnitType UnitType;

    //单位状态
    public EUnitState UnitState { get; set; }

    //属性
    public Dictionary<AttrType, int> AttrDic = new Dictionary<AttrType, int>();

    //保存动画状态机数据
    private Dictionary<int, string> StateDic = new Dictionary<int, string>();

    //保存状态指向的状态
    private Dictionary<int, string> TransitionStateDic = new Dictionary<int, string>();

    public Animator Animator;

    //碰撞体
    public Collider Collider;

    //Unit状态改变通知
    public Action<EUnitState, EUnitState> UnitStateChanged;

    //动画State状态改变通知
    public Action<string, string> AnimatorStateChanged;

    //动画State结束通知
    public Action<string> AnimatorStateFinished;

    //当前播放的AnimatorState
    public string CurrentAnimatorState;

    //动画状态对应的状态
    private static Dictionary<string, EUnitState> NameToStateDic = new Dictionary<string, EUnitState>()
    {
        { "Enter_01",EUnitState.Enter},
        { "Enter_02",EUnitState.Enter},
        { "Enter_03",EUnitState.Enter},
        { "Idle",EUnitState.Idle},
        { "WalkFront",EUnitState.Walk},
        { "RunFront",EUnitState.Run},
        { "PreAirDead",EUnitState.PreAirDead},
        { "PreStandDead",EUnitState.PreStandDead},
        { "AirDead",EUnitState.AirDead},
        { "StandDead",EUnitState.StandDead},
        { "KnockBack",EUnitState.KnockBack},
        { "KnockOut",EUnitState.KnockOut},
        { "StandHit",EUnitState.StandBeHit},
        { "StandHit_01",EUnitState.StandBeHit},
        { "StandHit_02",EUnitState.StandBeHit},
        { "AirHit",EUnitState.AirBeHit},
        { "AirHit_01",EUnitState.AirBeHit},
        { "AirHit_02",EUnitState.AirBeHit},
        { "StandUp",EUnitState.StandUp},
    };

    //单位位置
    public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    //单位欧拉角
    public Vector3 EulerAngles
    {
        get { return transform.eulerAngles; }
        set { transform.eulerAngles = value; }
    }

    public void PlayAnimation(string stateName, float bleendTime = 0.1f)
    {
        Animator.CrossFade(stateName, bleendTime);
    }

    public void CastSkill(string skill)
    {
        UnitState = EUnitState.Skill;
        BehaviorTree behaviorTree = BehaviorTreeManager.Instance.CreateBehaviorTree((int)EBehaviorTreeType.SKILL, skill, this);
        BehaviorTreeManager.Instance.RunBehaviorTree(behaviorTree);
    }

    private void Start()
    {
        if (ID == 0)
            ID = IdGenerater.GenerateId();

        UnitManager.Instance.Add(this);

        Animator = GetComponent<Animator>();

        Collider = GetComponent<Collider>();

        AnimatorController animatorController = Animator.runtimeAnimatorController as AnimatorController;
        AnimatorStateMachine animatorStateMachine = animatorController.layers[0].stateMachine;
        for (int i = 0; i < animatorStateMachine.states.Length; i++)
        {
            ChildAnimatorState childAnimatorState = animatorStateMachine.states[i];
            string stateName = childAnimatorState.state.name;
            int nameHash = childAnimatorState.state.nameHash;
            StateDic.Add(nameHash, stateName);

            if (childAnimatorState.state.behaviours.Length == 0)
                childAnimatorState.state.AddStateMachineBehaviour<UnitStateBehaviour>();

            AnimatorStateTransition[] transitions = childAnimatorState.state.transitions;
            if (transitions.Length > 1)
            {
                throw new Exception($"State:{stateName},不能包含多个Tansition");
            }

            if (transitions.Length == 1)
            {
                AnimatorStateTransition transition = transitions[0];
                TransitionStateDic.Add(nameHash, transition.destinationState.name);
            }
        }
    }

    //AnimatorController通知状态改变
    public void OnNotifyAnimationStateEnter(AnimatorStateInfo stateInfo)
    {
        string preStateName = CurrentAnimatorState;

        string StateName = string.Empty;
        if (!StateDic.TryGetValue(stateInfo.shortNameHash, out StateName))
            return;

        CurrentAnimatorState = StateName;

        EUnitState unitState = EUnitState.None;
        NameToStateDic.TryGetValue(StateName, out unitState);
        if (unitState != EUnitState.None)
        {
            EUnitState preState = UnitState;
            UnitState = unitState;
            UnitStateChanged?.Invoke(preState, UnitState);
        }

        AnimatorStateChanged?.Invoke(preStateName, CurrentAnimatorState);
    }

    //AnimatorController通知状态改变
    public void OnNotifyAnimationStateExit(AnimatorStateInfo stateInfo)
    {
        string StateName = string.Empty;
        if (!StateDic.TryGetValue(stateInfo.shortNameHash, out StateName))
            return;

        AnimatorStateFinished?.Invoke(StateName);
    }

    //设置名称
    public void SetName(string name)
    {
        Name = name;
    }

    //设置属性
    public void SetAttr(AttrType attrType, int value)
    {
        AttrDic[attrType] = value;
    }

    public void SetState(EUnitState state)
    {
        if (UnitState == state)
            return;

        string StateName = string.Empty;

        //持续的状态才需要改变动作
        switch (state)
        {
            case EUnitState.Idle:
                StateName = "Idle";
                break;
            case EUnitState.Walk:
                StateName = "WalkFront";
                break;
            case EUnitState.Run:
                StateName = "RunFront";
                break;
            case EUnitState.StandDead:
                StateName = "StandDead";
                break;
            case EUnitState.AirDead:
                StateName = "StandDead";
                break;
        }

        if (!string.IsNullOrEmpty(StateName))
            PlayAnimation(StateName);
    }

    //获取属性
    public int GetAttr(AttrType attrType)
    {
        int value = 0;
        AttrDic.TryGetValue(attrType, out value);
        return value;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
