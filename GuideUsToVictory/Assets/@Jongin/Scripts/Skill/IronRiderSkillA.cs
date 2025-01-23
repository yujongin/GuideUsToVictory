using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IronRiderSkillA : SkillBase
{
    public GameObject dustEffect;
    public bool isDoing = false;
    UnitController controll;
    StatModifier addAR;
    StatModifier addSpeed;

    Coroutine coroutine;
    private void OnEnable()
    {
        if (!Owner.skills.ActiveSkills.Contains(this))
        {
            SetInfo(GetComponent<UnitBase>());
        }
    }
    public override void SetInfo(UnitBase owner)
    {
        base.SetInfo(owner);
        if (owner.skills != null)
        {
            Owner.skills.ActiveSkills.Add(this);
        }
        float lastDetectRange = owner.detectRange;
        owner.detectRange = 500f;
        addAR = new StatModifier(500f, Define.EStatModType.Add, 0, gameObject);
        addSpeed = new StatModifier(5f, Define.EStatModType.Add, 0, gameObject);
        owner.attackRange.AddModifier(addAR);
        owner.speed.AddModifier(addSpeed);
    }
    public override void DoSkill()
    {
        if (isDoing) return;
        Owner.skills.ActiveSkills.Remove(this);
        isDoing = true;
        dustEffect.SetActive(true);
        controll = Owner.GetComponent<UnitController>();

        controll.SetState(Define.EUnitState.Skill);
        Owner.UnitAnimator.SetTrigger(skillData.AnimParam);
        coroutine = StartCoroutine(RunToTarget());
    }

    Node startNode;
    Node destNode;
    bool IsLerpCellPosCompleted = true;
    Node next;
    IEnumerator RunToTarget()
    {
        IsLerpCellPosCompleted = true;
        while (controll.unitState == Define.EUnitState.Skill)
        {
            if (IsLerpCellPosCompleted)
            {
                Owner.Target = Owner.DetectTarget();
                startNode = Managers.Map.GetNodeFromWorldPosition(transform.position);
                destNode = destNode = Managers.Map.GetNodeFromWorldPosition(Owner.Target.transform.position);

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
                Vector3 dirVec = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z) - transform.position;
                if (dirVec.magnitude < 0.01f)
                {
                    transform.position = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z);
                    IsLerpCellPosCompleted = true;
                }

                float moveDist = Mathf.Min(dirVec.magnitude, Owner.speed.Value * Time.deltaTime);
                transform.position += dirVec.normalized * moveDist;

                if (!Owner.Target.LookLeft)
                {
                    dustEffect.transform.localRotation = Quaternion.Euler(0, 180, 90);
                }
                else
                {
                    dustEffect.transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnitBase>() != null)
        {
            if (other.GetComponent<UnitBase>().MyTeam == Owner.MyTeam
                || other.GetComponent<UnitBase>().isDead) return;

            if (isDoing)
            {
                isDoing = false;
                //stop coroutine 
                StopCoroutine(coroutine);
                // remove stat modifiy
                Owner.detectRange = 30f;
                Owner.attackRange.RemoveModifier(addAR);
                Owner.speed.RemoveModifier(addSpeed);

                // effect off
                dustEffect.SetActive(false);

                //damage to target
                Owner.Target.OnDamage(Owner);

                if (controll.unitState != Define.EUnitState.Stun)
                {
                    controll.SetState(Define.EUnitState.Move);
                }

                if (Owner.Target.GetComponent<UnitController>() != null)
                {
                    Vector3 dir = Owner.Target.transform.position - transform.position;
                    float x = dir.normalized.x * 10f;
                    Owner.Target.transform.DOMoveX(Owner.Target.transform.position.x + x, 0.1f).SetEase(Ease.OutSine);
                    //if (Owner.Target.isDead) return;
                    //Owner.Target.GetComponent<UnitController>().SetState(Define.EUnitState.Stun, 1f);
                }
            }
        }
    }
}
