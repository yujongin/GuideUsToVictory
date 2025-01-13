using System.Collections;
using UnityEngine;
using static Define;
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
    public float detectRange = 50f;

    public Animator animator;

    protected string myTeam;
    protected string enemyTeam;
    public LayerMask enemyLayer;

    public void Init()
    {
        animator = GetComponent<Animator>();

        curHp = baseStat.Hp;
        maxHp = new UnitStat(baseStat.Hp);
        speed = new UnitStat(baseStat.Speed);
        attackSpeed = new UnitStat(baseStat.AttackSpeed);
        attackDamage = new UnitStat(baseStat.AttackDamage);
        armor = new UnitStat(baseStat.Armor);
        abilityPower=new UnitStat(baseStat.AbilityPower);
        magicRegistance = new UnitStat(baseStat.MagicRegistance);
        attackRange = new UnitStat(baseStat.AttackRange);

        myTeam = LayerMask.LayerToName(gameObject.layer);
        enemyTeam = myTeam == "Blue" ? "Red" : "Blue";
        enemyLayer = LayerMask.GetMask(enemyTeam);
    }
}
