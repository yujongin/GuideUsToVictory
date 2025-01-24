using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private List<GameObject> storedBlocks = new List<GameObject>(); // 저장된 블록 리스트

    public Transform[] blockPositions; // 각 블록이 배치될 위치 배열

    private int currentBlockIndex = 0; // 현재 블록 배치 인덱스

    public void StoreBlock(GameObject block)
    {
        if (currentBlockIndex < blockPositions.Length)
        {
            // 저장된 블록 위치로 이동
            block.transform.position = blockPositions[currentBlockIndex].position;
            block.transform.rotation = blockPositions[currentBlockIndex].rotation;
            storedBlocks.Add(block); // 블록 저장
            currentBlockIndex++; // 다음 위치로 이동
        }
        else
        {
            Debug.LogWarning("더 이상 저장할 위치가 없음.");
        }
    }

    public void ClearStoredBlocks()
    {
        foreach (var block in storedBlocks)
        {
            Destroy(block); // 저장된 블록 삭제
        }
        storedBlocks.Clear();
        currentBlockIndex = 0; // 인덱스 초기화
    }
}
