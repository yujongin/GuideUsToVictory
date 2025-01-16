using UnityEngine;

public class Projectile : MonoBehaviour
{
    public UnitBase Owner { get; private set; }

    public ProjectileData projectileData;
    private ProjectileMotionBase projectileMotion;
    
    public void SetSpawnInfo(UnitBase owner)
    {
        Owner = owner;

        projectileMotion = GetComponent<ProjectileMotionBase>();    
        ParabolaMotion parabolaMotion = projectileMotion as ParabolaMotion;
        if (parabolaMotion != null)
            parabolaMotion.SetInfo(projectileData,owner.CenterPosition, owner.target.CenterPosition, () =>
            {
                Owner.target.OnDamage(Owner);
                //despawn
                Destroy(gameObject);
            });
    }
}
