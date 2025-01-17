using System.Collections;
using UnityEngine;

public class Tower : UnitBase
{
    LineRenderer lineRenderer;

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        unitCollider = GetComponent<Collider>();
        skills = GetComponent<SkillComponent>();
        skills.SetInfo(this);


        curHp = baseStat.Hp;
        maxHp = new UnitStat(baseStat.Hp);
        speed = new UnitStat(baseStat.Speed);
        attackSpeed = new UnitStat(baseStat.AttackSpeed);
        attackDamage = new UnitStat(baseStat.AttackDamage);
        armor = new UnitStat(baseStat.Armor);
        abilityPower = new UnitStat(baseStat.AbilityPower);
        magicRegistance = new UnitStat(baseStat.MagicRegistance);
        attackRange = new UnitStat(baseStat.AttackRange);
        unitRadius = unitCollider.bounds.size.x / 2;

        myTeam = LayerMask.LayerToName(gameObject.layer);
        enemyTeam = myTeam == "Blue" ? "Red" : "Blue";
        enemyLayer = LayerMask.GetMask(enemyTeam);

        AddPos = Vector3.up * 5f;
        StartCoroutine(AttackTarget());
    }

    IEnumerator AttackTarget()
    {
        while (true)
        {
            Target = DetectTarget();

            // Ÿ���� ������ ���� ������Ʈ
            if (Target != null)
            {
                if (Target.isDead)
                {
                    yield return null;
                }
                //Debug.Log(skills.CurrentSkill.skillData.name);

                //animator.SetTrigger("Attack");

                if (Vector3.Distance(Target.transform.position, transform.position) < attackRange.Value)
                {
                    skills.CurrentSkill.DoSkill();
                    yield return new WaitForSeconds(1 / attackSpeed.Value);
                }
            }

            yield return null;
        }
    }

    void UpdateLine()
    {
        if (Target != null)
        {
            lineRenderer.enabled = true;

            // ���� �������� ���� ����
            lineRenderer.SetPosition(0, transform.position + Vector3.up * 20f); // Ÿ�� ��ġ
            lineRenderer.SetPosition(1, Target.transform.position + Vector3.down * 0.5f); // Ÿ�� ��ġ
        }
        else
        {
            lineRenderer.enabled = false; // Ÿ���� ������ ���� ��Ȱ��ȭ
        }
    }
    void Update()
    {
        //UpdateLine();
    }
}
