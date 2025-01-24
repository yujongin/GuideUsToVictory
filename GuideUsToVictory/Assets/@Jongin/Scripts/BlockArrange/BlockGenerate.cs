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

        // ���� ť���� ����� ��ġ�� �ϳ� ����
        Vector2 currentPos = posQueue.Dequeue();

        // ����� �����ϰ� ��ġ ����
        GameObject newBlock = Instantiate(block, transform);
        newBlock.transform.localPosition = new Vector3(0, 0 ,0);
        blockCount++;

        // ���� ��ġ�� blockMap�� ���
        int x = (int)currentPos.x;
        int y = (int)currentPos.y;
        blockMap[x, y] = true;

        // �ֺ� �� ���� �� �����ϰ� ���� ����
        List<Vector2> availableDirs = new List<Vector2>();
        for (int i = 0; i < dir.Length; i++)
        {
            Vector2 nextPos = currentPos + dir[i];
            int nextX = (int)nextPos.x;
            int nextY = (int)nextPos.y;

            // ���� ���� �ְ�, ���� ����� ���� ��ġ��� �߰�
            if (nextX >= 0 && nextX < blockMap.GetLength(0) && nextY >= 0 && nextY < blockMap.GetLength(1) && !blockMap[nextX, nextY])
            {
                availableDirs.Add(nextPos);
            }
        }

        // ������ �������� ����� Ȯ��
        while (availableDirs.Count > 0 && blockCount < maxCount)
        {
            int randIndex = Random.Range(0, availableDirs.Count);
            Vector2 nextBlockPos = availableDirs[randIndex];
            availableDirs.RemoveAt(randIndex);

            posQueue.Enqueue(nextBlockPos); // ���ο� ��� ��ġ�� ť�� �߰�
            blockMap[(int)nextBlockPos.x, (int)nextBlockPos.y] = true;
        }

        // ��� ȣ��� ���� ��� ����
        GenerateRandomBlock(maxCount);
    }
}

