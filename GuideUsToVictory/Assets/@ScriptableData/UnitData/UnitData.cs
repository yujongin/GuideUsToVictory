using UnityEngine;
using static Define;
[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/UnitData")]
public class UnitData : ScriptableObject
{
    public int cost;
    public string Name;
    [TextArea]
    public string Description;
    public float Hp;
    public float Speed;
    public float AttackSpeed;
    public float AttackDamage;
    public float Armor;
    public float AbilityPower;
    public float MagicRegistance;
    public float AttackRange;
    public int Capacity;
    public float PriceFaith;
    public ERace Race;
    public Sprite BlueUnitIcon;
    public Sprite RedUnitIcon;
    public Sprite UnitLockedIcon;
}
