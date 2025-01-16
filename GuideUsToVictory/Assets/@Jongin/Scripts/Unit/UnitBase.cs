using UnityEngine;
public class UnitBase : MonoBehaviour
{
    public UnitBase Target { get; protected set; }
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
    public float unitRadius { get; private set; }
    public bool isDead = false;

    public Animator animator { get; private set; }
    protected SpriteRenderer spriteRenderer;
    protected Collider unitCollider;

    protected string myTeam;
    protected string enemyTeam;
    public LayerMask enemyLayer;

    public Vector3 CenterPosition { get { return transform.position + Vector3.down * 4f; } }
    public Transform projectileLauncher;


    bool lookLeft = false;
    public bool LookLeft
    {
        get { return lookLeft; }
        set
        {
            lookLeft = value;
            Flip(value);
        }
    }

    public void Init()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    }

    public void OnDamage(UnitBase attacker)
    {
        if(isDead) return;

        UnitStat damage = attacker.attackDamage.Value >= attacker.abilityPower.Value ? attacker.attackDamage : attacker.abilityPower;
        Debug.Log(damage.Value);
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
        detectedEnemies = Physics.OverlapSphere(transform.position, detectRange, enemyLayer);

        if (detectedEnemies != null && detectedEnemies.Length > 0)
        {
            float minDist = float.MaxValue;
            int minIndex = 0;
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
            return detectedEnemies[minIndex].GetComponent<UnitBase>();
        }
        return null;
    }
}
