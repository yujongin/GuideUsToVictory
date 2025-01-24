using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private List<GameObject> storedBlocks = new List<GameObject>(); // ����� ��� ����Ʈ

    public Transform[] blockPositions; // �� ����� ��ġ�� ��ġ �迭

    private int currentBlockIndex = 0; // ���� ��� ��ġ �ε���

    public void StoreBlock(GameObject block)
    {
        if (currentBlockIndex < blockPositions.Length)
        {
            // ����� ��� ��ġ�� �̵�
            block.transform.position = blockPositions[currentBlockIndex].position;
            block.transform.rotation = blockPositions[currentBlockIndex].rotation;
            storedBlocks.Add(block); // ��� ����
            currentBlockIndex++; // ���� ��ġ�� �̵�
        }
        else
        {
            Debug.LogWarning("�� �̻� ������ ��ġ�� ����.");
        }
    }

    public void ClearStoredBlocks()
    {
        foreach (var block in storedBlocks)
        {
            Destroy(block); // ����� ��� ����
        }
        storedBlocks.Clear();
        currentBlockIndex = 0; // �ε��� �ʱ�ȭ
    }
}
