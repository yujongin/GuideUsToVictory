using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public GameObject block;
    public GameObject blockRoot;
    int blockCount = 0;
    Vector2[] dir = new Vector2[4]
    {
        new Vector2(0,1), new Vector2(1,0), new Vector2(-1,0), new Vector2(0,-1)
    };
    Vector2 startPoint = new Vector2(0, 0);
    bool[,] blockMap = new bool[9, 9];
    Queue<Vector2> posQueue = new Queue<Vector2>();


    public GameObject GetRandomBlock()
    {
        for (int i = 0; i < blockMap.GetLength(0); i++)
        {
            for (int j = 0; j < blockMap.GetLength(1); j++)
            {
                blockMap[i, j] = false;
            }
        }

        blockRoot = new GameObject("Block");
        int num = Random.Range(0, 10);
        int maxb = 0;
        if (num >= 0) maxb = 3;
        if (num >= 2) maxb = 4;
        if (num >= 7) maxb = 5;
        blockCount = 0;
        posQueue.Clear();
        posQueue.Enqueue(startPoint);
        GenerateRandomBlock(maxb);
        return blockRoot;
    }

    void GenerateRandomBlock(int maxCount)
    {
        if (blockCount >= maxCount || posQueue.Count == 0) return;

        // 현재 큐에서 블록의 위치를 하나 꺼냄
        Vector2 currentPos = posQueue.Dequeue();

        // 블록을 생성하고 위치 설정
        GameObject newBlock = Instantiate(block, blockRoot.transform);
        newBlock.transform.localPosition = new Vector3(currentPos.x, 0, currentPos.y);
        blockCount++;

        // 현재 위치를 blockMap에 기록
        int x = (int)currentPos.x;
        int y = (int)currentPos.y;
        blockMap[x + 4, y + 4] = true;

        // 주변 네 방향 중 랜덤하게 방향 선택
        List<Vector2> availableDirs = new List<Vector2>();
        for (int i = 0; i < dir.Length; i++)
        {
            Vector2 nextPos = currentPos + dir[i];
            int nextX = (int)nextPos.x;
            int nextY = (int)nextPos.y;

            // 범위 내에 있고, 아직 블록이 없는 위치라면 추가
            if (nextX >= -4 && nextX <= 4 && nextY >= -4 && nextY <= 4 && !blockMap[nextX + 4, nextY + 4])
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
            if (availableDirs.Count != 0)
            {
                int isDoing = Random.Range(0, 10);

                if (isDoing < 8) continue;
            }
            posQueue.Enqueue(nextBlockPos); // 새로운 블록 위치를 큐에 추가
            blockMap[(int)nextBlockPos.x + 4, (int)nextBlockPos.y + 4] = true;
        }

        // 재귀 호출로 다음 블록 생성
        GenerateRandomBlock(maxCount);
    }
}

