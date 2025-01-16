using System.Collections;
using UnityEngine;

public class NormalAttack : SkillBase
{
    public float shootDelay = 0;

    Coroutine shootRoutine;
    public override void SetInfo(UnitBase owner)
    {
        base.SetInfo(owner);

    }

    public override void DoSkill()
    {
        base.DoSkill();
        if (projectile != null)
        {
            if(shootDelay > 0)
            {
                if(shootRoutine != null)
                {
                    StopCoroutine(shootRoutine);
                }

                shootRoutine = StartCoroutine(ShootRoutine());
            }
            else
            {
                GenerateProjectile(Owner, Owner.CenterPosition);
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(shootDelay);
        GenerateProjectile(Owner, Owner.CenterPosition);
    }
}
