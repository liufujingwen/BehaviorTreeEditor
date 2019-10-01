public enum EUnitState
{
    None,
    Idle = 1 << 1,
    Walk = 2 << 1,//移动
    Run = 3 << 1,//跑
    Skill = 4 << 1,//释放技能
    BeHit = 5 << 1,//受击
    PreDead = 6 << 1,//预死亡
    Dead = 7 << 1,//死亡（躺尸）
}
