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
            parabolaMotion.SetInfo(projectileData,owner.projectileLauncher.position, target, () =>
            {
                Owner.Target.OnDamage(Owner);
                //despawn
                Destroy(gameObject);
            });  
        
        StraightMotion straightMotion = projectileMotion as StraightMotion;
        if (straightMotion != null)
            straightMotion.SetInfo(projectileData,owner.projectileLauncher.position, target, () =>
            {
                Owner.Target.OnDamage(Owner);
                //despawn
                Destroy(gameObject);
            });
    }
}
