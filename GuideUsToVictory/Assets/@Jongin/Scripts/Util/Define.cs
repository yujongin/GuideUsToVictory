using UnityEngine;

public static class Define
{
    public enum EStatModType
    {
        Add,
        PercentAdd,
        PercentMult,
    }

    public enum EUnitState
    {
        Idle,
        Move,
        Attack,
        Skill,
        Dead
    }
}
