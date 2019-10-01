using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //单位类型
    public EUnitType UnitType;
    
    //单位状态
    public EUnitState UnitState = EUnitState.Idle;

    //当前播放的动画
    public string AnimationName;
    //当前动画时间
    public float AnimationTime;

    public void PlayAnimation()
    {
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
