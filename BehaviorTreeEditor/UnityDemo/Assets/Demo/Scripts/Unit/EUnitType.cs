public enum EUnitType
{
    None,
    Self = 1 << 0,//玩家自身
    Friend = 1 << 1,//友方单位
    Enemy = 1 << 2,//敌方单位
    All = Friend | Enemy,
}