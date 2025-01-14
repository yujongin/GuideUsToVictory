using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitController : UnitBase
{
    [SerializeField]
    UnitBase target;

    EUnitState unitState;

    Node startNode;
    Node destNode;
    List<Vector2> path;

    void Start()
    {
        base.Init();

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
                StartCoroutine(UpdateMove(baseStat.Speed));
                break;
            case EUnitState.Attack:
                StartCoroutine(UpdateAttack(baseStat.AttackSpeed));
                break;
            case EUnitState.Skill:
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
            if (clip.name.Contains("Attack"))
            {
                animationLength = clip.length;
            }
        }
        while (unitState == EUnitState.Attack)
        {
            if (target != null)
            {
                if (target.isDead)
                {
                    SetState(EUnitState.Move);
                    yield break;
                }
            }

            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(animationLength + 1 / attackSpeed);
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

                DetectTarget();
                if (target != null)
                {
                    destNode = Managers.Map.GetNodeFromWorldPosition(target.transform.position);
                    LookAtTarget(target);
                }

                List<Vector2> path = Managers.Map.FindPath(startNode.cellPos, destNode.cellPos);

                if (path.Count < 2) yield break;

                if (next != null) { next.walkable = true; }
                next = Managers.Map.GetNodeFromCellPosition(path[1]);
                next.walkable = false;
                IsLerpCellPosCompleted = false;
            }
            else
            {
                if (target != null)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < baseStat.AttackRange + target.unitRadius)
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

    Collider[] detectedEnemy = new Collider[1];
    public void DetectTarget()
    {
        Physics.OverlapSphereNonAlloc(transform.position, detectRange, detectedEnemy, enemyLayer);

        if (detectedEnemy[0] != null)
        {
            target = detectedEnemy[0].GetComponent<UnitBase>();
        }
        else
        {
            target = Managers.Map.GetTower(enemyTeam);
        }
    }

    public void DamageToEnemy()
    {
        target.OnDamage(this);
    }
    public override void OnDead()
    {
        base.OnDead();
        animator.SetTrigger("Dead");
        SetState(EUnitState.Dead);
        if(next != null)
        {
            next.walkable = true;
        }
        collider.enabled = false;
        Destroy(gameObject, 2f);
    }


    private List<Vector2> debugPath = new List<Vector2>();

    void OnDrawGizmos()
    {
        // 경로를 시각적으로 확인
        Gizmos.color = Color.yellow;

        if (debugPath != null && debugPath.Count > 1)
        {
            for (int i = 0; i < debugPath.Count - 1; i++)
            {
                // 각 경로 지점을 선으로 연결
                Vector3 from = Managers.Map.GetNodeFromCellPosition(debugPath[i]).worldPosition;
                Vector3 to = Managers.Map.GetNodeFromCellPosition(debugPath[i + 1]).worldPosition;

                Gizmos.DrawLine(from, to);
            }
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
