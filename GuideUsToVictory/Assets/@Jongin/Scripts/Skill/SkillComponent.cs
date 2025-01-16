using System.Collections.Generic;
using UnityEngine;
using static Define;
public class SkillComponent : MonoBehaviour
{
    public List<SkillBase> SkillList { get; } = new List<SkillBase>();
    public List<SkillBase> ActiveSkills { get; set; } = new List<SkillBase>();

    public SkillBase DefaultSkill { get; set; }
    public SkillBase ASkill { get; set; }

    UnitBase owner;
    public SkillBase CurrentSkill
    {
        get
        {
            if(ActiveSkills.Count == 0) 
                return DefaultSkill;

            int randomIndex = Random.Range(0,ActiveSkills.Count);
            return ActiveSkills[randomIndex];
        }
    }

    public void SetInfo(UnitBase owner)
    {
        this.owner = owner;
        SkillBase[] skills = GetComponents<SkillBase>();
        
        for(int i = 0; i<skills.Length; i++)
        {
            if (skills[i].skillData.SkillSlot == ESkillSlot.DefaultSkill)
            {
                DefaultSkill = skills[i];
                DefaultSkill.SetInfo(owner);
                SkillList.Add(DefaultSkill);
            }
            else if(skills[i].skillData.SkillSlot == ESkillSlot.ASkill)
            {
                ASkill = skills[i];
                ASkill.SetInfo(owner);
                SkillList.Add(ASkill);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(CurrentSkill.skillData.name);
        }
    }
}
