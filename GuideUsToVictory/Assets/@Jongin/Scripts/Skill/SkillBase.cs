using System.Collections;
using UnityEngine;

public class SkillBase : MonoBehaviour
{
    public UnitBase Owner { get; protected set; }
    public float RemainCoolTime { get; set; }

    public SkillData skillData;

    public Projectile projectile;

    public virtual void SetInfo(UnitBase owner)
    {
        Owner = owner;
    }

    private IEnumerator CountdownCoolTime()
    {
        RemainCoolTime = skillData.CoolTime;
        yield return new WaitForSeconds(skillData.CoolTime);
        RemainCoolTime = 0;

        if (Owner.skills != null)
        {
            Owner.skills.ActiveSkills.Add(this);
        }
    }

    public virtual void DoSkill()
    {
        if (Owner.skills != null)
        {
            Owner.skills.ActiveSkills.Remove(this);
        }

        Owner.animator.SetTrigger(skillData.AnimParam);
    }

    protected virtual void GenerateProjectile(UnitBase owner, Vector3 spawnPos)
    {
        Projectile go = Instantiate(projectile, spawnPos, Quaternion.identity);
        go.SetSpawnInfo(owner);
    }
}