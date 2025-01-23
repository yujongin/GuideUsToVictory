using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;
public class EffectBase : MonoBehaviour
{
    public UnitBase owner;
    public SkillBase skill;
    public EffectData effectData;
    public EEffectType effectType;

    public virtual void SetInfo(UnitBase owner,SkillBase skill)
    {
        this.skill = skill;
        this.owner = owner;
        effectType = effectData.EffectType;
    }
    public virtual void ApplyEffect()
    {
        ShowEffect();
    }

    protected virtual void ShowEffect()
    {

    }
    protected void AddModifier(UnitStat stat, object source, int order = 0)
    {
        if (effectData.Amount != 0)
        {
            StatModifier add = new StatModifier(effectData.Amount, EStatModType.Add, order, source);
            stat.AddModifier(add);
        }

        if (effectData.PercentAdd != 0)
        {
            StatModifier percentAdd = new StatModifier(effectData.PercentAdd, EStatModType.PercentAdd, order, source);
            stat.AddModifier(percentAdd);
        }

        if (effectData.PercentMult != 0)
        {
            StatModifier percentMult = new StatModifier(effectData.PercentMult, EStatModType.PercentMult, order, source);
            stat.AddModifier(percentMult);
        }
    }

    protected void RemoveModifier(UnitStat stat, object source)
    {
        stat.ClearModifiersFromSource(source);
    }
}
