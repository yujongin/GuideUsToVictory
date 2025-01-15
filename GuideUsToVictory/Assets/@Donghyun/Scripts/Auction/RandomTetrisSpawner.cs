using UnityEngine;
using System.Collections.Generic;

public class RandomTetrisSpawner : MonoBehaviour
{
    public GameObject cubePrefab;      
    public Vector3 spawnPosition = new Vector3(-6.5f, 45f, 428f); 
    public float cubeSize = 1f;        
    public Vector3 blockRotation = new Vector3(-46f, 46f, 0f); 

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>(); 
    private List<GameObject> spawnedCubes = new List<GameObject>();     

    private void Start()
    {
        SpawnTetrisBlock(); 
    }

    public void OnSpawnButtonClicked()
    {
        SpawnTetrisBlock(); 
    }

    public void SpawnTetrisBlock()
    {
        spawnedPositions.Clear();

        RemoveTetrisBlock();

        int cubeCount = Random.Range(3, 6);

        Quaternion rotation = Quaternion.Euler(blockRotation);

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
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTetrisBlock();
        }
        
    }
}
