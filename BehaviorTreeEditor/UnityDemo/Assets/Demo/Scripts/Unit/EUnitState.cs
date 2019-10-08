public enum EUnitState
{
    None,
    Enter = 1 << 0,//进场
    Idle = 1 << 1,//Idle
    Walk = 1 << 2,//移动
    Run = 1 << 3,//跑
    Skill = 1 << 4,//释放技能
    KnockBack = 1 << 6,//击退
    KnockOut = 1 << 6,//击飞
    AirBeHit = 1 << 5,//空中受击
    StandBeHit = 1 << 5,//站立受击
    StandUp = 1 << 6,//站起来
    PreStandDead = 1 << 7,//站立预死亡
    PreAirDead = 1 << 7,//空中预死亡
    Dead = 1 << 8,//死亡（躺尸）
}
