using UnityEngine;

public static class UnitFactory
{
    public static Unit CreateUnit(string res, EUnitType unitType)
    {
        GameObject unitGo = Object.Instantiate(Resources.Load(res)) as GameObject;
        Unit unit = unitGo.GetComponent<Unit>();
        if (unit == null)
            unit = unitGo.AddComponent<Unit>();
        unit.ID = IdGenerater.GenerateId();
        unit.UnitType = unitType;
        return unit;
    }

    public static Unit CreateUnit(string res, int ID, EUnitType unitType)
    {
        GameObject unitGo = Object.Instantiate(Resources.Load(res)) as GameObject;
        Unit unit = unitGo.GetComponent<Unit>();
        if (unit == null) 
            unit = unitGo.AddComponent<Unit>();
        unit.ID = ID;
        unit.UnitType = unitType;
        return unit;
    }
}
