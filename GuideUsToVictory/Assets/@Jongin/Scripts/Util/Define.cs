using UnityEngine;

public static class Define
{
    public enum EStatModType
    {
        Add,
        PercentAdd,
        PercentMult,
    }

    public enum ETeam
    {
        Blue,
        Red
    }
    public enum EUnitState
    {
        Idle,
        Move,
        Attack,
        Skill,
        Stun,
        Dead
    }

    public enum ESkillSlot
    {
        DefaultSkill,
        ASkill
    }

    public enum EGameState
    {
        Ready,
        Battle,
        End
    }

    public enum ERace
    {
        Human,
        Demon
    }

    public enum EEffectType
    {
        Buff,
        Debuff,
        CrowdControl,
    }
}
