using UnityEngine;

public class TowerProjectile : Projectile
{
    public GameObject orb;
    public GameObject explosion;
    public float detectRange;

    private void OnEnable()
    {
        explosion.SetActive(false);
        orb.SetActive(true);
    }

    Vector3 subValue = Vector3.zero;
    public override void EndCallback(UnitBase target)
    {
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, detectRange, Owner.EnemyLayer);

        for(int i = 0; i< enemies.Length; i++)
        {
            enemies[i].GetComponent<UnitBase>().OnDamage(Owner,1);
        }
        orb.SetActive(false);
        subValue.Set(0, 6f, 0.1f);
        explosion.transform.position = target.transform.position - subValue;
        explosion.SetActive(true);
        //despawn
        Managers.Resource.Destroy(gameObject, 1f);
    }
}
