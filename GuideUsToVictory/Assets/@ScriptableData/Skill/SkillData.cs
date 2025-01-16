using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string Name;
    public float CoolTime;
    public float Duration;
    public float SkillRange;
    public float DamageMultiplier;
    public string AnimParam;
    public ESkillSlot SkillSlot;
}
