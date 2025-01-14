using UnityEngine;
public class UnitBase : MonoBehaviour
{
    [SerializeField]
    protected UnitData baseStat;

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
    public float unitRadius = 0;
    public bool isDead = false;


    protected Animator animator;
    protected SpriteRenderer renderer;
    protected Collider collider;

    protected string myTeam;
    protected string enemyTeam;
    public LayerMask enemyLayer;


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
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider>();

        curHp = baseStat.Hp;
        maxHp = new UnitStat(baseStat.Hp);
        speed = new UnitStat(baseStat.Speed);
        attackSpeed = new UnitStat(baseStat.AttackSpeed);
        attackDamage = new UnitStat(baseStat.AttackDamage);
        armor = new UnitStat(baseStat.Armor);
        abilityPower = new UnitStat(baseStat.AbilityPower);
        magicRegistance = new UnitStat(baseStat.MagicRegistance);
        attackRange = new UnitStat(baseStat.AttackRange);
        unitRadius = collider.bounds.size.x / 2;

        myTeam = LayerMask.LayerToName(gameObject.layer);
        enemyTeam = myTeam == "Blue" ? "Red" : "Blue";
        enemyLayer = LayerMask.GetMask(enemyTeam);
    }

    public void OnDamage(UnitBase attacker)
    {
        curHp -= attacker.attackDamage.Value;

        if(curHp < 0)
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
        renderer.flipX = flag;
    }
}
