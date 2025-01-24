using System.Collections.Generic;
using UnityEngine;

public class EffectComponent : MonoBehaviour
{
    public List<EffectBase> ActiveEffects = new List<EffectBase>();
    private UnitBase owner;

    public List<EffectBase> GenerateEffects(EffectBase[] effects, SkillBase skill)
    {
        List<EffectBase> generatedEffects = new List<EffectBase>();

        foreach(EffectBase effect in effects)
        {
            ActiveEffects.Add(effect);
            generatedEffects.Add(effect);

            effect.SetInfo(owner, skill);
            effect.ApplyEffect();
        }
        return generatedEffects;
    }

    public void ClearEffect()
    {

    }
}
