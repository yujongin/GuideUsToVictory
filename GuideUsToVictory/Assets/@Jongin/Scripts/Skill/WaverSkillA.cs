using System.Collections;
using UnityEngine;

public class WaverSkillA : SkillBase
{
    public GameObject magicOrb;
    private void OnEnable()
    {
        if (!Owner.skills.ActiveSkills.Contains(this))
        {
            Owner.skills.ActiveSkills.Add(this);
        }
    }
    public override void SetInfo(UnitBase owner)
    {
        base.SetInfo(owner);
    }

    public override void DoSkill()
    {
        base.DoSkill();
        StartCoroutine(SummonMagicOrb());
    }

    IEnumerator SummonMagicOrb()
    {
        yield return new WaitForSeconds(0.4f);
        GameObject go = Managers.Resource.Instantiate(magicOrb, null, true);
        go.transform.position = Owner.Target.transform.position - Vector3.up * 6;
        yield return new WaitForSeconds(3f);
        go.GetComponent<WaverMagicOrb>().AreaOfEffect(skillData,Owner,Owner.Target);
    }
}
