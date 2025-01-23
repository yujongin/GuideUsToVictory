using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class RandomTetrisSpawner : MonoBehaviour
{
    public GameObject cubePrefab;       // 큐브 프리팹
    public Vector3 spawnPosition = new Vector3(55.7f, 28.1f, -160.7f); // 초기 생성 위치
    public float cubeSize = 1f;         // 큐브 간격 (크기)

    public AuctionTimer auctionTimer;
    public Vector3 blockRotation = new Vector3(0f, 0f, 0f); // 블록 회전 값

    private HashSet<Vector3> usedPositions = new HashSet<Vector3>();
    private List<GameObject> spawnedBlocks = new List<GameObject>();

    public void SpawnTetrisBlock()
    {
        RemoveTetrisBlock();
        usedPositions.Clear();
        spawnedBlocks.Clear();

        int cubeCount = Random.Range(3, 6);
        Vector3 currentPosition = spawnPosition;
        Quaternion rotation = Quaternion.Euler(blockRotation);

        GameObject firstCube = InstantiateCube(currentPosition, rotation);
        spawnedBlocks.Add(firstCube);
        usedPositions.Add(currentPosition);

        for (int i = 1; i < cubeCount; i++)
        {
            Vector3 newPosition = GetRandomAdjacentPosition(currentPosition, rotation);
            GameObject newCube = InstantiateCube(newPosition, rotation);
            spawnedBlocks.Add(newCube);
            usedPositions.Add(newPosition);
            currentPosition = newPosition;
        }
    }

    public void MoveBlockToStore()
    {
        if (spawnedBlocks.Count > 0)
        {
            foreach (GameObject block in spawnedBlocks)
            {
                block.transform.position = new Vector3(118f, 4f, -81.9f);
            }
            spawnedBlocks.Clear();
        }
    }

    public void RemoveTetrisBlock()
    {
        foreach (GameObject block in spawnedBlocks)
        {
            Destroy(block);
        }
        spawnedBlocks.Clear();
    }

    private Vector3 GetRandomAdjacentPosition(Vector3 basePosition, Quaternion rotation)
    {
        Vector3[] localAdjacentPositions = new Vector3[]
        {
            new Vector3(cubeSize, 0, 0),
            new Vector3(-cubeSize, 0, 0),
            new Vector3(0, 0, cubeSize),
            new Vector3(0, 0, -cubeSize)
        };

        List<Vector3> validPositions = new List<Vector3>();

        foreach (Vector3 localPosition in localAdjacentPositions)
        {
            Vector3 worldPosition = localPosition + basePosition;

            if (!usedPositions.Contains(worldPosition))
            {
                validPositions.Add(worldPosition);
            }
        }

        if (validPositions.Count == 0)
        {
            return basePosition;
        }

        return validPositions[Random.Range(0, validPositions.Count)];
    }

    private GameObject InstantiateCube(Vector3 position, Quaternion rotation)
    {
        return Instantiate(cubePrefab, position, rotation);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTetrisBlock();
        }

    }
}