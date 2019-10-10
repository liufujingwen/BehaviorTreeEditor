public enum EUnitState
{
    None,
    Enter = 1 << 0,//进场
    Idle = 1 << 1,//Idle
    Walk = 1 << 2,//移动
    Run = 1 << 3,//跑
    Skill = 1 << 4,//释放技能
    KnockBack = 1 << 5,//击退
    KnockOut = 1 << 6,//击飞
    AirBeHit = 1 << 7,//空中受击
    StandBeHit = 1 << 8,//站立受击
    StandUp = 1 << 9,//站起来
    PreStandDead = 1 << 10,//站立预死亡
    PreAirDead = 1 << 11,//空中预死亡
    StandDead = 1 << 12,//站立死亡（躺尸）
    AirDead = 1 << 13,//空中死亡（躺尸）
}
