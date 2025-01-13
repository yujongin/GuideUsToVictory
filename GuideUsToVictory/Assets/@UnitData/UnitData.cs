using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public float Hp;
    public float Speed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Armor;
    public float AbilityPower;
    public float MagicRegistance;
    public float AttackRange;
}
