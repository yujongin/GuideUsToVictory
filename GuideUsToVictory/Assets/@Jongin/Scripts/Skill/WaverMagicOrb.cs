using UnityEngine;

public class WaverMagicOrb : MonoBehaviour
{
    public float detectRange = 6f;
    public void AreaOfEffect(SkillData skillData, UnitBase owner,UnitBase target)
    {
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, detectRange, owner.EnemyLayer);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<UnitBase>().OnDamage(owner);
        }
        //despawn
        Managers.Resource.Destroy(gameObject, 1f);
    }

    //private void OnDrawGizmos()
    //{
        
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, detectRange);
    //}
}
