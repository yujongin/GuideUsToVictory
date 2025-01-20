using UnityEngine;
using static Define;
[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public string Name;
    public float Hp;
    public float Speed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Armor;
    public float AbilityPower;
    public float MagicRegistance;
    public float AttackRange;
    public int Capacity;
    public ERace Race;
}
