using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;       // ť�� ������
    public Transform spawnCenter;       // ���� �߽� ��ġ
    public float cubeSize = 1f;         // ť�� ���� (ũ��)

    private List<Vector3[]> possibleShapes = new List<Vector3[]>(); // ������ ť�� ����

    void Start()
    {
        // ������ ���� ����
        DefineShapes();
    }

    public void SpawnCubes()
    {
        // ���� ť�� ����
        foreach (Transform child in spawnCenter)
        {
            Destroy(child.gameObject);
        }

        // �������� �ϳ��� ���� ����
        Vector3[] selectedShape = possibleShapes[Random.Range(0, possibleShapes.Count)];

        // ���õ� ���տ� ���� ť�� ����
        foreach (Vector3 localPosition in selectedShape)
        {
            InstantiateCube(localPosition);
        }
    }

    private void InstantiateCube(Vector3 localPosition)
    {
        // ���� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPosition = spawnCenter.TransformPoint(localPosition * cubeSize);

        // Y ��ǥ�� ���� (SpawnCenter�� Y�� ���)
        worldPosition.y = spawnCenter.position.y;

        // ť�� ����
        GameObject newCube = Instantiate(cubePrefab, worldPosition, Quaternion.identity);
        newCube.transform.SetParent(spawnCenter);
    }

    private void DefineShapes()
    {
        // 3�� ���� (��, ��, ��)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up });       // ����
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.forward }); // ����
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.left });    // ����

        // 4�� ���� (��, ��, ��)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.up + Vector3.right }); // ����
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.up + Vector3.forward }); // ���� ����
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.up, Vector3.right, Vector3.left }); // ����

        // 5�� ���� (������ ����)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.forward, Vector3.left }); // ���� ���
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.right + Vector3.up, Vector3.up, Vector3.forward }); // ���� ����
    }
}
