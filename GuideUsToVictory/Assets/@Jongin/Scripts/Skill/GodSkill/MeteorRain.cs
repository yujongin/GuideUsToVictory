using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class MeteorRain : MonoBehaviour
{
    bool isUse = false;
    bool isAIUse = false;
    public VisualEffect myEffect;
    public VisualEffect aiEffect;
    public Button meteorBtn;
    UnitBase enemyTower;
    private void Start()
    {
        myEffect.Stop();
        aiEffect.Stop();
        enemyTower = Managers.Map.GetTower(Managers.Game.enemyTeamData.Team);
    }
    private void Update()
    {
        if (enemyTower.curHp < enemyTower.maxHp.Value / 3 && !isAIUse)
        {
            StartCoroutine(EffectPlay(aiEffect, LayerMask.GetMask(Managers.Game.myTeamData.Team.ToString())));
            isAIUse = true;
        }
    }
    public void DoSkill()
    {
        if (isUse) return;
        meteorBtn.interactable = false;
        StartCoroutine(EffectPlay(myEffect, Managers.Game.enemyLayer));
        isUse = true;
    }

    IEnumerator EffectPlay(VisualEffect effect, LayerMask mask)
    {
        effect.Play();
        yield return new WaitForSeconds(3);
        Collider[] colliders = Physics.OverlapSphere(Vector3.zero, 250f, mask);
        foreach (var collider in colliders)
        {
            UnitController unit;
            if (collider.TryGetComponent<UnitController>(out unit))
            {
                unit.curHp = 0;
                unit.OnDead();
            }
        }
        yield return new WaitForSeconds(3);
        effect.Stop();
    }


}
