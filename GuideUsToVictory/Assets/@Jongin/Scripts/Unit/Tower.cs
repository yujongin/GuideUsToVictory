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
    void Update()
    {
        //UpdateLine();
    }
}
