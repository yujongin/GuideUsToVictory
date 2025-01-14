using UnityEngine;
using System.Collections.Generic;

public class RandomTetrisSpawner : MonoBehaviour
{
    public GameObject cubePrefab;       // 큐브 프리팹
    public Vector3 spawnPosition = new Vector3(-6.5f, 45f, 428f); // 초기 생성 위치
    public float cubeSize = 1f;         // 큐브 간격 (크기)
    public Vector3 blockRotation = new Vector3(-46f, 46f, 0f); // 블록 회전 값 (X, Y, Z 축)

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>(); // 사용된 위치를 추적
    private List<GameObject> spawnedCubes = new List<GameObject>();     // 생성된 큐브 객체를 추적

    private void Start()
    {
        SpawnTetrisBlock(); // 게임 시작 시 자동 생성
    }

    public void OnSpawnButtonClicked()
    {
        SpawnTetrisBlock(); // 큐브 생성
    }

    public void SpawnTetrisBlock()
    {
        // 기존 위치 초기화
        spawnedPositions.Clear();

        // 기존 생성된 큐브 제거 (중복 방지)
        RemoveTetrisBlock();

        // 랜덤으로 생성할 큐브 개수 (3~5개)
        int cubeCount = Random.Range(3, 6);

        // 회전값 생성
        Quaternion rotation = Quaternion.Euler(blockRotation);

        // 첫 번째 큐브는 지정된 위치에 생성
        Vector3 currentPosition = spawnPosition;
        GameObject firstCube = InstantiateCube(currentPosition, rotation);
        spawnedPositions.Add(currentPosition);
        spawnedCubes.Add(firstCube);

        // 나머지 큐브 생성
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
        // 생성된 모든 큐브 삭제
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }

        // 리스트 초기화
        spawnedCubes.Clear();
        spawnedPositions.Clear();

        Debug.Log("All Tetris Blocks Removed");
    }

    private Vector3 GetRandomAdjacentPosition(Vector3 basePosition, Quaternion rotation)
    {
        Vector3[] localAdjacentPositions = new Vector3[]
        {
            new Vector3(cubeSize, 0, 0),  // 오른쪽
            new Vector3(-cubeSize, 0, 0), // 왼쪽
            new Vector3(0, 0, cubeSize),  // 앞쪽
            new Vector3(0, 0, -cubeSize)  // 뒤쪽
        };

        List<Vector3> validPositions = new List<Vector3>();

        foreach (Vector3 localPosition in localAdjacentPositions)
        {
            Vector3 worldPosition = rotation * localPosition + basePosition;

            // 사용된 위치인지 확인
            if (!spawnedPositions.Contains(worldPosition))
            {
                validPositions.Add(worldPosition);
            }
        }

        // 유효한 위치가 없으면 기본 위치 반환
        return validPositions.Count == 0 ? basePosition : validPositions[Random.Range(0, validPositions.Count)];
    }

    private GameObject InstantiateCube(Vector3 position, Quaternion rotation)
    {
        // 큐브 생성
        GameObject cube = Instantiate(cubePrefab, position, rotation);
        return cube;
    }
}
