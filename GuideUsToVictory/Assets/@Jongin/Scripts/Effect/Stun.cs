using UnityEngine;
using static Define;

public class Stun : EffectBase
{
    private void Awake()
    {
        effectType = EEffectType.CrowdControl;
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();

        //    lastState = Owner.CreatureState;
        //    if (lastState == ECreatureState.OnDamaged)
        //        return;

        //    Owner.CreatureState = ECreatureState.OnDamaged;
    }

    //public override bool ClearEffect(EEffectClearType clearType)
    //{
    //    if (base.ClearEffect(clearType) == true)
    //        Owner.CreatureState = lastState;

    //    return true;
    //}
}
