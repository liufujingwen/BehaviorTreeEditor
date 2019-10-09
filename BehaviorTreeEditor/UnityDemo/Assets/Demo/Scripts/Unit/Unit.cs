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
    public EUnitState UnitState = EUnitState.Idle;

    //属性
    public Dictionary<AttrType, int> AttrDic = new Dictionary<AttrType, int>();

    //保存动画状态机数据
    private Dictionary<int, string> StateDic = new Dictionary<int, string>();

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
        { "Dead",EUnitState.Dead},
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

    public Animator Animator;

    //碰撞体
    public Collider Collider;

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

    public void PlayAnimation(string stateName, float bleendTime)
    {
        Animator.CrossFade(stateName, bleendTime);
    }

    public void CastSkill(string skill)
    {
        UnitState = EUnitState.Skill;
        BehaviorTree behaviorTree = BehaviorTreeManager.Instance.CreateBehaviorTree((int)EBehaviorTreeType.SKILL, skill, this);
        BehaviorTreeManager.Instance.RunBehaviorTree(behaviorTree);
    }

    void Start()
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
        }
    }

    //AnimatorController通知状态改变
    public void NotifyAnimationStateChange(AnimatorStateInfo stateInfo)
    {
        string StateName = string.Empty;
        if (StateDic.TryGetValue(stateInfo.shortNameHash, out StateName))
            return;

        EUnitState unitState = EUnitState.None;
        NameToStateDic.TryGetValue(StateName, out unitState);
        if (unitState != EUnitState.None)
            UnitState = unitState;
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
