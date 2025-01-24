using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class Tower : UnitBase
{
    LineRenderer lineRenderer;
    public GameObject destroyEffect;
    public Image hpDisplayer;
    Tween damageCoroutine;
    void Start()
    {
        Init();
        AddPos = Vector3.up * 5f;
        StartCoroutine(AttackTarget());
    }
    public override void OnDamage(UnitBase attacker, float damageMultiplier)
    {
        if (isDead) return;
        if(damageCoroutine != null)
        {
            damageCoroutine.Kill();
        }

        damageCoroutine = transform.DOPunchPosition(transform.right, 1, 10, 0, false);
        base.OnDamage(attacker, damageMultiplier);

        hpDisplayer.fillAmount = curHp / maxHp.Value;
    }


    IEnumerator AttackTarget()
    {
        while (true)
        {
            if (isDead)
            {
                Managers.Game.GameEnd(MyTeam);
                TowerCollapseSequence();
                yield break;
            }
            Target = DetectTarget();

            // 타겟이 있으면 라인 업데이트
            if (Target != null)
            {
                if (Target.isDead)
                {
                    yield return null;
                }
                else
                {
                    if (Vector3.Distance(Target.transform.position, transform.position) < attackRange.Value)
                    {
                        skills.CurrentSkill.DoSkill();
                        yield return new WaitForSeconds(1 / attackSpeed.Value);
                    }
                }
            }
            yield return null;
        }
    }

    void UpdateLine()
    {
        if (Target != null)
        {
            lineRenderer.enabled = true;

            // 라인 시작점과 끝점 설정
            lineRenderer.SetPosition(0, transform.position + Vector3.up * 20f); // 타워 위치
            lineRenderer.SetPosition(1, Target.transform.position + Vector3.down * 0.5f); // 타겟 위치
        }
        else
        {
            lineRenderer.enabled = false; // 타겟이 없으면 라인 비활성화
        }
    }

    public void TowerCollapseSequence()
    {
        destroyEffect.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(-46f, 5f).SetEase(Ease.Linear));
        sequence.Join(transform.parent.DOShakePosition(5f,new Vector3(0.5f,0,0.5f),20,90,false,false,ShakeRandomnessMode.Full));
        sequence.Append(destroyEffect.transform.DOScale(Vector3.zero, 1f));
    }
}
