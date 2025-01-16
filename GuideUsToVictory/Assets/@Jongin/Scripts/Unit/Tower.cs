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
        lineRenderer.positionCount = 2; // �� ���� �׸��� ���� ����Ʈ ��
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        StartCoroutine(AttackTarget());
    }

    IEnumerator AttackTarget()
    {
        while (true)
        {
            Target = DetectTarget();

            // Ÿ���� ������ ���� ������Ʈ

            yield return new WaitForSeconds(1 / attackSpeed.Value);
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
