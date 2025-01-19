using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitController : UnitBase
{
    EUnitState unitState;

    Node startNode;
    Node destNode;
    List<Vector2> path;

    private void OnEnable()
    {
        base.Init();
        AddPos = Vector3.up * -4f;
        SetState(EUnitState.Move);
    }


    public void SetState(EUnitState state)
    {
        unitState = state;
        switch (unitState)
        {
            case EUnitState.Idle:
                animator.SetTrigger("Idle");
                break;
            case EUnitState.Move:
                animator.SetTrigger("Move");
                StartCoroutine(UpdateMove(speed.Value));
                break;
            case EUnitState.Attack:
                StartCoroutine(UpdateAttack(attackSpeed.Value));
                break;
            case EUnitState.Dead:
                break;
        }
    }

    bool IsLerpCellPosCompleted = true;
    Node next;
    IEnumerator UpdateAttack(float attackSpeed)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        float animationLength = 0;
        foreach (var clip in clips)
        {
            if (clip.name.Contains(skills.CurrentSkill.skillData.AnimParam))
            {
                animationLength = clip.length;
            }
        }
        while (unitState == EUnitState.Attack)
        {
            if (Target != null)
            {
                if (Target.isDead)
                {
                    SetState(EUnitState.Move);
                    yield break;
                }
                //Debug.Log(skills.CurrentSkill.skillData.name);

                //animator.SetTrigger("Attack");

                skills.CurrentSkill.DoSkill();
                yield return new WaitForSeconds(animationLength + 1 / attackSpeed);
            }
            else
            {
                SetState(EUnitState.Move);
            }
        }
        yield break;
    }

    IEnumerator UpdateMove(float moveSpeed)
    {
        IsLerpCellPosCompleted = true;
        while (unitState == EUnitState.Move)
        {
            if (IsLerpCellPosCompleted)
            {
                startNode = Managers.Map.GetNodeFromWorldPosition(transform.position);

                Target = DetectTarget();
                if (Target == null)
                {
                    Target = Managers.Map.GetTower(enemyTeam);
                }

                destNode = Managers.Map.GetNodeFromWorldPosition(Target.transform.position);
                LookAtTarget(Target);

                List<Vector2> path = Managers.Map.FindPath(startNode.cellPos, destNode.cellPos, 10);

                if (path.Count < 2)
                {
                    //Debug.Log("NoPath");
                }
                else
                {
                    if (next != null) { next.walkable = true; }
                    next = Managers.Map.GetNodeFromCellPosition(path[1]);
                    next.walkable = false;
                    IsLerpCellPosCompleted = false;
                }
            }
            else
            {
                if (Target != null)
                {
                    if (Vector3.Distance(transform.position, Target.transform.position) < attackRange.Value + Target.unitRadius)
                    {
                        animator.SetTrigger("Idle");
                        SetState(EUnitState.Attack);
                        yield break;
                    }
                }
                Vector3 dirVec = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z) - transform.position;
                if (dirVec.magnitude < 0.01f)
                {
                    transform.position = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z);
                    IsLerpCellPosCompleted = true;
                }

                float moveDist = Mathf.Min(dirVec.magnitude, moveSpeed * Time.deltaTime);
                transform.position += dirVec.normalized * moveDist;
            }

            yield return null;
        }
        yield break;
    }

    public void DamageToEnemy()
    {
        Target.OnDamage(this);
    }
    public override void OnDead()
    {
        base.OnDead();
        animator.SetTrigger("Dead");
        SetState(EUnitState.Dead);
        if (next != null)
        {
            next.walkable = true;
        }
        Managers.Resource.Destroy(gameObject, 2f);
    }

    //private List<Vector2> debugPath = new List<Vector2>();

    //void OnDrawGizmos()
    //{
    //    // 경로를 시각적으로 확인
    //    Gizmos.color = Color.yellow;

    //    if (debugPath != null && debugPath.Count > 1)
    //    {
    //        for (int i = 0; i < debugPath.Count - 1; i++)
    //        {
    //            // 각 경로 지점을 선으로 연결
    //            Vector3 from = Managers.Map.GetNodeFromCellPosition(debugPath[i]).worldPosition;
    //            Vector3 to = Managers.Map.GetNodeFromCellPosition(debugPath[i + 1]).worldPosition;

    //            Gizmos.DrawLine(from, to);
    //        }
    //    }

    //    Gizmos.color = Color.magenta;
    //    Gizmos.DrawWireSphere(transform.position, detectRange);
    //}
}
