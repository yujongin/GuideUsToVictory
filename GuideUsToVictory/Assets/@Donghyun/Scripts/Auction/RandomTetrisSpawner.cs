using UnityEngine;
using System.Collections.Generic;

public class RandomTetrisSpawner : MonoBehaviour
{
    public GameObject cubePrefab;       // ť�� ������
    public Vector3 spawnPosition = new Vector3(-6.5f, 45f, 428f); // �ʱ� ���� ��ġ
    public float cubeSize = 1f;         // ť�� ���� (ũ��)
    public Vector3 blockRotation = new Vector3(-46f, 46f, 0f); // ��� ȸ�� �� (X, Y, Z ��)

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>(); // ���� ��ġ�� ����
    private List<GameObject> spawnedCubes = new List<GameObject>();     // ������ ť�� ��ü�� ����

    private void Start()
    {
        SpawnTetrisBlock(); // ���� ���� �� �ڵ� ����
    }

    public void OnSpawnButtonClicked()
    {
        SpawnTetrisBlock(); // ť�� ����
    }

    public void SpawnTetrisBlock()
    {
        // ���� ��ġ �ʱ�ȭ
        spawnedPositions.Clear();

        // ���� ������ ť�� ���� (�ߺ� ����)
        RemoveTetrisBlock();

        // �������� ������ ť�� ���� (3~5��)
        int cubeCount = Random.Range(3, 6);

        // ȸ���� ����
        Quaternion rotation = Quaternion.Euler(blockRotation);

        // ù ��° ť��� ������ ��ġ�� ����
        Vector3 currentPosition = spawnPosition;
        GameObject firstCube = InstantiateCube(currentPosition, rotation);
        spawnedPositions.Add(currentPosition);
        spawnedCubes.Add(firstCube);

        // ������ ť�� ����
        for (int i = 1; i < cubeCount; i++)
        {
            Vector3 newPosition = GetRandomAdjacentPosition(currentPosition, rotation);
            GameObject cube = InstantiateCube(newPosition, rotation);
            spawnedPositions.Add(newPosition);
            spawnedCubes.Add(cube);
            currentPosition = newPosition;
        }
    }

    public void RemoveTetrisBlock()
    {
        // ������ ��� ť�� ����
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }

        // ����Ʈ �ʱ�ȭ
        spawnedCubes.Clear();
        spawnedPositions.Clear();

        Debug.Log("All Tetris Blocks Removed");
    }

    private Vector3 GetRandomAdjacentPosition(Vector3 basePosition, Quaternion rotation)
    {
        Vector3[] localAdjacentPositions = new Vector3[]
        {
            new Vector3(cubeSize, 0, 0),  // ������
            new Vector3(-cubeSize, 0, 0), // ����
            new Vector3(0, 0, cubeSize),  // ����
            new Vector3(0, 0, -cubeSize)  // ����
        };

        List<Vector3> validPositions = new List<Vector3>();

        foreach (Vector3 localPosition in localAdjacentPositions)
        {
            Vector3 worldPosition = rotation * localPosition + basePosition;

            // ���� ��ġ���� Ȯ��
            if (!spawnedPositions.Contains(worldPosition))
            {
                validPositions.Add(worldPosition);
            }
        }

        // ��ȿ�� ��ġ�� ������ �⺻ ��ġ ��ȯ
        return validPositions.Count == 0 ? basePosition : validPositions[Random.Range(0, validPositions.Count)];
    }

    private GameObject InstantiateCube(Vector3 position, Quaternion rotation)
    {
        // ť�� ����
        GameObject cube = Instantiate(cubePrefab, position, rotation);
        return cube;
    }
}
