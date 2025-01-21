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
}
