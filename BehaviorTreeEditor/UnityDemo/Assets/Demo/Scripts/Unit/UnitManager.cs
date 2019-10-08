using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    private static int Id;

    public Dictionary<GameObject, Unit> UnitDic = new Dictionary<GameObject, Unit>();
    public List<Unit> Units = new List<Unit>();

    public void Add(Unit unit)
    {
        if (unit == null)
            return;

        if (Find(unit.ID) != null)
        {
            Debug.LogError($"已存在Unit id：{unit.ID}.");
            return;
        }

        Units.Add(unit);
        UnitDic.Add(unit.gameObject, unit);
    }

    public void Add(long id)
    {
    }

    public void Remove(int id)
    {
    }

    public void Remove(Unit unit)
    {
    }

    public Unit Find(int id)
    {
        for (int i = 0; i < Units.Count; i++)
        {
            Unit unit = Units[i];
            if (unit != null && unit.ID == id)
                return unit;
        }
        return null;
    }

    public void NotifyAnimationStateChange(Animator animator, AnimatorStateInfo stateInfo)
    {
        if (!animator)
            return;

        Unit unit = null;

        if (!UnitDic.TryGetValue(animator.gameObject, out unit))
            return;

        if (unit == null)
            return;

        unit.NotifyAnimationStateChange(stateInfo);
    }
}
