using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public UnitBase Owner { get; private set; }

    public ProjectileData projectileData;
    private ProjectileMotionBase projectileMotion;

    public void SetSpawnInfo(UnitBase owner)
    {
        Owner = owner;
        UnitBase target = Owner.Target;
        projectileMotion = GetComponent<ProjectileMotionBase>();

        ParabolaMotion parabolaMotion = projectileMotion as ParabolaMotion;
        if (parabolaMotion != null)
            parabolaMotion.SetInfo(projectileData, owner.projectileLauncher.position, target, () =>
            {
                EndCallback(target);
            });

        StraightMotion straightMotion = projectileMotion as StraightMotion;
        if (straightMotion != null)
            straightMotion.SetInfo(projectileData, owner.projectileLauncher.position, target, () =>
            {
                EndCallback(target);
            });
    }

    public virtual void EndCallback(UnitBase target)
    {
        if (target != null)
        {
            target.OnDamage(Owner, 1);
        }
        //despawn
        Managers.Resource.Destroy(gameObject);
    }
}
