using UnityEngine;

public class UnitController : UnitBase
{
    public enum UnitState
    {
        Idle,
        Run,
        Attack,
        Skill,
        Dead
    }

    UnitState unitState;
    void Start()
    {
        unitState = UnitState.Idle;
    }

    void Update()
    {

    }

    public void SetState(UnitState state)
    {
        unitState = state;
        switch (unitState)
        {
            case UnitState.Idle:
                animator.SetTrigger("Idle");
                break;
            case UnitState.Run:
                animator.SetTrigger("Run");
                break;
            case UnitState.Attack:
                base.Attack(target);
                break;
            case UnitState.Skill:
                break;
            case UnitState.Dead:
                OnDead();
                break;
        }
    }


    void PathFind()
    {

    }
}
