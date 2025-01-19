using System.Collections;
using UnityEngine;

public class Tower : UnitBase
{
    LineRenderer lineRenderer;

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
    void Update()
    {
        //UpdateLine();
    }
}
