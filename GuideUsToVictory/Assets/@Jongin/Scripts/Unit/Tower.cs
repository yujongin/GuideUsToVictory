using System.Collections;
using UnityEngine;
using DG.Tweening;
public class Tower : UnitBase
{
    LineRenderer lineRenderer;
    public GameObject destroyEffect;
    void Start()
    {
        Init();
        AddPos = Vector3.up * 5f;
        StartCoroutine(AttackTarget());
    }



    IEnumerator AttackTarget()
    {
        while (true)
        {
            if (isDead)
            {
                TowerCollapseSequence();
                yield break;
            }
            Target = DetectTarget();

            // Ÿ���� ������ ���� ������Ʈ
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

            // ���� �������� ���� ����
            lineRenderer.SetPosition(0, transform.position + Vector3.up * 20f); // Ÿ�� ��ġ
            lineRenderer.SetPosition(1, Target.transform.position + Vector3.down * 0.5f); // Ÿ�� ��ġ
        }
        else
        {
            lineRenderer.enabled = false; // Ÿ���� ������ ���� ��Ȱ��ȭ
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
