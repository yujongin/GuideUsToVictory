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
    public override void EndCallback(UnitBase target)
    {
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, detectRange, Owner.enemyLayer);

        for(int i = 0; i< enemies.Length; i++)
        {
            enemies[i].GetComponent<UnitBase>().OnDamage(Owner);
        }
        orb.SetActive(false);
        explosion.transform.position = target.transform.position - new Vector3(0,6f,0.1f);
        explosion.SetActive(true);
        //despawn
        Managers.Resource.Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
