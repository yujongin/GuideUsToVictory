using System.Collections;
using UnityEngine;

public class WaverMagicOrb : MonoBehaviour
{
    public float detectRange = 6f;
    public void AreaOfEffect(SkillData skillData, UnitBase owner,UnitBase target)
    {
        StartCoroutine(DelayDamage(skillData, owner, target)); 
    }
    IEnumerator DelayDamage(SkillData skillData, UnitBase owner, UnitBase target)
    {
        yield return new WaitForSeconds(3f);
        Collider[] enemies = Physics.OverlapSphere(target.transform.position, detectRange, owner.EnemyLayer);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<UnitBase>().OnDamage(owner, skillData.DamageMultiplier);
        }
        yield return new WaitForSeconds(1f);
        //despawn
        Managers.Resource.Destroy(gameObject);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
