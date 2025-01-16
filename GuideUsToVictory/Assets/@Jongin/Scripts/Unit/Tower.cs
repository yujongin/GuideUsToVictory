using System.Collections;
using UnityEngine;

public class Tower : UnitBase
{
    LineRenderer lineRenderer;

    void Start()
    {
        base.Init();
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2; // 두 점을 그리기 위한 포인트 수
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        StartCoroutine(AttackTarget());
    }

    IEnumerator AttackTarget()
    {
        while (true)
        {
            target = DetectTarget();

            // 타겟이 있으면 라인 업데이트

            yield return new WaitForSeconds(1 / attackSpeed.Value);
        }
    }

    void UpdateLine()
    {
        if (target != null)
        {
            lineRenderer.enabled = true;

            // 라인 시작점과 끝점 설정
            lineRenderer.SetPosition(0, transform.position + Vector3.up * 20f); // 타워 위치
            lineRenderer.SetPosition(1, target.transform.position + Vector3.down * 0.5f); // 타겟 위치
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
