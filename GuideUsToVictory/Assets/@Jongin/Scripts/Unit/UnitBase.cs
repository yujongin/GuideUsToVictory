using System.Collections;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    UnitStat stat;

    protected UnitBase target;
    protected Animator animator;

    public void Attack(UnitBase unit)
    {
        target = unit;
        StartCoroutine(AttackSequence());
    }

    IEnumerator AttackSequence()
    {
        while (target != null)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(1 / stat.AttackSpeed);
        }
        yield return null;
    }

    public void Run()
    {
        animator.SetTrigger("Run");
    }
    public virtual void Skill() { }

    public void OnDamage()
    {
        target.stat.Hp -= (stat.AttackDamage >= stat.AbilityPower ? stat.AttackDamage : stat.AbilityPower);

        if (target.stat.Hp < 0)
        {
            stat.Hp = 0;
            target.OnDead();
        }
    }

    public void OnDead()
    {
        animator.SetTrigger("Dead");
    }
}
