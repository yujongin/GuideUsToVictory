using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [SerializeField]
    protected UnitData baseStat;

    public float curHp;
    public float Speed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Armor;
    public float AbilityPower;
    public float MagicRegistance;
    public float AttackRange;
}
