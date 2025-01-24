using System.Collections.Generic;
using UnityEngine;

public class BlockGenerate : MonoBehaviour
{
    public GameObject block;

    
    void Start()
    {
    }
    int blockCount = 0;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            int num = Random.Range(3, 6);
            blockCount = 0;
            posQueue.Enqueue(startPoint);
            GenerateRandomBlock(num);
            Debug.Log(blockCount);
        }
    }
    Vector2[] dir = new Vector2[4]
    {
        new Vector2(0,1), new Vector2(1,0), new Vector2(-1,0), new Vector2(0,-1)
    };
    Vector2 startPoint = new Vector2(4.5f, 4.5f);
    bool[,] blockMap = new bool[9,9];
    Queue<Vector2> posQueue = new Queue<Vector2>();

    void GenerateRandomBlock(int maxCount)
    {
        if (blockCount >= maxCount || posQueue.Count == 0) return;

        // 현재 큐에서 블록의 위치를 하나 꺼냄
        Vector2 currentPos = posQueue.Dequeue();

        // 블록을 생성하고 위치 설정
        GameObject newBlock = Instantiate(block, transform);
        newBlock.transform.localPosition = new Vector3(0, 0 ,0);
        blockCount++;

        // 현재 위치를 blockMap에 기록
        int x = (int)currentPos.x;
        int y = (int)currentPos.y;
        blockMap[x, y] = true;

        // 주변 네 방향 중 랜덤하게 방향 선택
        List<Vector2> availableDirs = new List<Vector2>();
        for (int i = 0; i < dir.Length; i++)
        {
            Vector2 nextPos = currentPos + dir[i];
            int nextX = (int)nextPos.x;
            int nextY = (int)nextPos.y;

            // 범위 내에 있고, 아직 블록이 없는 위치라면 추가
            if (nextX >= 0 && nextX < blockMap.GetLength(0) && nextY >= 0 && nextY < blockMap.GetLength(1) && !blockMap[nextX, nextY])
            {
                availableDirs.Add(nextPos);
            }
        }

        // 랜덤한 방향으로 블록을 확장
        while (availableDirs.Count > 0 && blockCount < maxCount)
        {
            int randIndex = Random.Range(0, availableDirs.Count);
            Vector2 nextBlockPos = availableDirs[randIndex];
            availableDirs.RemoveAt(randIndex);

            posQueue.Enqueue(nextBlockPos); // 새로운 블록 위치를 큐에 추가
            blockMap[(int)nextBlockPos.x, (int)nextBlockPos.y] = true;
        }

        // 재귀 호출로 다음 블록 생성
        GenerateRandomBlock(maxCount);
    }
}

