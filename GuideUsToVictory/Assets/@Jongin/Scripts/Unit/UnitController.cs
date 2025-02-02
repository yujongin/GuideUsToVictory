using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitController : UnitBase
{
    public EUnitState unitState;

    Node startNode;
    Node destNode;
    List<Vector2> path;

    EUnitState lastState;
    private void OnEnable()
    {
        base.Init();
        spriteRenderer.color = Color.white;
        transform.position = Managers.UnitSpawn.GetRandomSpawnPos(MyTeam);
        SetState(EUnitState.Idle);
    }

    private void Update()
    {
        switch (unitState)
        {
            case EUnitState.Idle:
                UpdateIdle();
                break;
            case EUnitState.Move:
                UpdateMove();
                break;
            case EUnitState.Attack:
                UpdateAttack();
                break;
            case EUnitState.Skill:
                break;
            case EUnitState.Dead:
                break;
        }
    }

    void UpdateIdle()
    {
        if (Managers.Game.GameState == EGameState.End) return;

        Target = DetectTarget();
        if (Target == null)
        {
            Target = Managers.Map.GetTower(EnemyTeam);
        }

        if (Vector3.Distance(transform.position, Target.transform.position) > attackRange.Value + Target.UnitRadius)
        {
            IsLerpCellPosCompleted = true;
            UnitAnimator.SetTrigger("Move");
            SetState(EUnitState.Move);
        }
        else
        {
            isAttack = false;
            SetState(EUnitState.Attack);
        }
    }
    bool IsLerpCellPosCompleted = true;
    Node next;
    void UpdateMove()
    {
        if (IsLerpCellPosCompleted)
        {
            startNode = Managers.Map.GetNodeFromWorldPosition(transform.position);
            Target = DetectTarget();
            if (Target == null)
            {
                Target = Managers.Map.GetTower(EnemyTeam);

                float z = Mathf.Clamp(transform.position.z, -10, 10);
                destNode = Managers.Map.GetNodeFromWorldPosition(new Vector3(Target.transform.position.x, Target.transform.position.y, z));
            }
            else
            {
                destNode = Managers.Map.GetNodeFromWorldPosition(Target.transform.position);
            }
            LookAtTarget(Target);

            path = Managers.Map.FindPath(startNode.cellPos, destNode.cellPos, 10);

            if (path.Count >= 2)
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
                if (Vector3.Distance(transform.position, Target.transform.position) <= attackRange.Value + Target.UnitRadius)
                {
                    SetState(EUnitState.Idle);
                    UnitAnimator.SetTrigger("Idle");
                    return;
                }
            }
            Vector3 dirVec = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z) - transform.position;
            if (dirVec.magnitude < 0.01f)
            {
                transform.position = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z);
                IsLerpCellPosCompleted = true;
            }

            float moveDist = Mathf.Min(dirVec.magnitude, speed.Value * Time.deltaTime);
            transform.position += dirVec.normalized * moveDist;
        }
    }


    float attackDelay = 0;
    bool isAttack = false;
    void UpdateAttack()
    {
        if (skills.CurrentSkill.skillData.SkillType == ESkillType.ActiveSkill)
        {
            float animationLength = 0;
            AnimationClip[] clips = UnitAnimator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Contains(skills.CurrentSkill.skillData.AnimParam))
                {
                    animationLength = clip.length;
                }
            }
            if (!isAttack)
            {
                skills.CurrentSkill.DoSkill();
                isAttack = true;
            }
             
            if (attackDelay > animationLength + 1 / attackSpeed.Value)
            {
                attackDelay = 0;
                SetState(EUnitState.Idle);
                return;
            }
            attackDelay += Time.deltaTime;
        }
        else if(skills.CurrentSkill.skillData.SkillType == ESkillType.ContinuousSkill)
        {
            if (!isAttack)
            {
                SetState(EUnitState.Skill);
                skills.CurrentSkill.DoSkill();
                isAttack = true;
            }
        }
    }


    public void SetState(EUnitState state, float stunTime = 0)
    {
        lastState = unitState;
        unitState = state;
    }
    //Coroutine stun;
    //IEnumerator UpdateStun(float stunTime)
    //{
    //    if (isDead) yield break;
    //    UnitAnimator.SetTrigger("Idle");
    //    GameObject prefab = Managers.Resource.effects["StunEffect"];
    //    GameObject go = Managers.Resource.Instantiate(prefab, null, true);
    //    go.transform.SetParent(transform);
    //    go.transform.localPosition = Vector3.zero;
    //    yield return new WaitForSeconds(stunTime);
    //    Managers.Resource.Destroy(go);
    //    if (!isDead)
    //    {
    //        SetState(EUnitState.Move);
    //    }
    //}
    IEnumerator DamageEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void DamageToEnemy()
    {
        if (Target != null)
            Target.OnDamage(this, 1);
    }

    private Coroutine damageEffectCoroutine;
    public override void OnDamage(UnitBase attacker, float damageMultiplier)
    {
        if (isDead) return;
        if (spriteRenderer != null)
        {
            if (damageEffectCoroutine != null)
            {
                StopCoroutine(damageEffectCoroutine);
            }
            damageEffectCoroutine = StartCoroutine(DamageEffect());
        }
        base.OnDamage(attacker, damageMultiplier);
    }


    public override void OnDead()
    {
        UnitAnimator.SetTrigger("Dead");
        SetState(EUnitState.Dead);
        base.OnDead();
        if (next != null)
        {
            next.walkable = true;
        }
        Managers.Game.AddFaith(EnemyTeam, baseStat.PriceFaith * 0.1f);
        Managers.UnitSpawn.DespawnUnit(this);
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
