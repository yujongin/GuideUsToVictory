using System;
using UnityEngine;
using static Define;
public class UnitBase : MonoBehaviour
{
    public UnitBase Target;
    [HideInInspector]
    public SkillComponent skills;
    public UnitData baseStat;

    public float curHp;
    public UnitStat maxHp;
    public UnitStat speed;
    public UnitStat attackSpeed;
    public UnitStat attackDamage;
    public UnitStat armor;
    public UnitStat abilityPower;
    public UnitStat magicRegistance;
    public UnitStat attackRange;
    public float detectRange = 10f;
    public float UnitRadius { get; protected set; }
    public bool isDead = false;

    public Animator UnitAnimator { get; private set; }
    protected SpriteRenderer spriteRenderer;
    protected Collider unitCollider;

    public ETeam MyTeam { get; private set; }
    public ETeam EnemyTeam { get; private set; }
    public LayerMask EnemyLayer { get; private set; }

    public Vector3 CenterPosition
    {
        get
        {
            return transform.position + AddPos;
        }
    }
    protected Vector3 AddPos { get; set; } = Vector3.zero;
    public Transform projectileLauncher;


    bool lookLeft = false;
    public bool LookLeft
    {
        get { return lookLeft; }
        set
        {
            if (lookLeft != value)
            {
                lookLeft = value;
                Flip(value);
                if (projectileLauncher != null)
                    projectileLauncher.localPosition = Vector3.Scale(projectileLauncher.localPosition, new Vector3(-1, 1, 1));
            }
        }
    }

    bool isInit = false;
    public void Init()
    {
        StatReset();

        if (!isInit)
        {
            if (TryGetComponent<Animator>(out Animator animator))
            {
                this.UnitAnimator = animator;
            }
            if (TryGetComponent<SpriteRenderer>(out SpriteRenderer spriteRenderer))
            {
                this.spriteRenderer = spriteRenderer;
            }
            unitCollider = GetComponent<Collider>();
            UnitRadius = unitCollider.bounds.size.x / 2;

            skills = GetComponent<SkillComponent>();
            skills.SetInfo(this);

            MyTeam = (ETeam)Enum.Parse(typeof(ETeam), LayerMask.LayerToName(gameObject.layer));
            EnemyTeam = MyTeam == ETeam.Blue ? ETeam.Red : ETeam.Blue;
            EnemyLayer = LayerMask.GetMask(EnemyTeam.ToString());
            AddPos = Vector3.up * -4f;

            isInit = true;
        }
    }

    public void StatReset()
    {
        
        curHp = baseStat.Hp;
        maxHp = new UnitStat(baseStat.Hp);
        speed = new UnitStat(baseStat.Speed);
        attackSpeed = new UnitStat(baseStat.AttackSpeed);
        attackDamage = new UnitStat(baseStat.AttackDamage);
        armor = new UnitStat(baseStat.Armor);
        abilityPower = new UnitStat(baseStat.AbilityPower);
        magicRegistance = new UnitStat(baseStat.MagicRegistance);
        attackRange = new UnitStat(baseStat.AttackRange);

        isDead = false;
    }
    public virtual void OnDamage(UnitBase attacker)
    {
        if (isDead) return;

        UnitStat damage = attacker.attackDamage.Value >= attacker.abilityPower.Value ? attacker.attackDamage : attacker.abilityPower;
        UnitStat defense = damage == attacker.attackDamage ? armor : magicRegistance;

        float finalDamage = (100 / (100 + defense.Value)) * damage.Value;
        curHp -= finalDamage;

        if (curHp < 0)
        {
            curHp = 0;
            OnDead();
        }
    }

    public virtual void OnDead()
    {
        isDead = true;
    }

    public void LookAtTarget(UnitBase target)
    {
        Vector3 dir = target.transform.position - transform.position;
        if (dir.x < 0)
            LookLeft = true;
        else
            LookLeft = false;
    }

    public void Flip(bool flag)
    {
        spriteRenderer.flipX = flag;
    }

    Collider[] detectedEnemies;
    public UnitBase DetectTarget()
    {
        detectedEnemies = Physics.OverlapSphere(transform.position, detectRange, EnemyLayer);

        if (detectedEnemies != null && detectedEnemies.Length > 0)
        {
            float minDist = float.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < detectedEnemies.Length; i++)
            {
                float dist = Vector3.Distance(transform.position, detectedEnemies[i].transform.position);
                if (minDist > dist)
                {
                    if (detectedEnemies[i].GetComponent<UnitBase>().isDead) continue;
                    minDist = dist;
                    minIndex = i;
                }
            }
            if (minIndex != -1)
                return detectedEnemies[minIndex].GetComponent<UnitBase>();
        }
        return null;
    }
}
