using UnityEngine;
using System.Collections.Generic;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;       // 큐브 프리팹
    public Transform spawnCenter;       // 생성 중심 위치
    public float cubeSize = 1f;         // 큐브 간격 (크기)

    private List<Vector3[]> possibleShapes = new List<Vector3[]>(); // 가능한 큐브 조합

    void Start()
    {
        // 가능한 조합 정의
        DefineShapes();
    }

    public void SpawnCubes()
    {
        // 기존 큐브 삭제
        foreach (Transform child in spawnCenter)
        {
            Destroy(child.gameObject);
        }

        // 랜덤으로 하나의 조합 선택
        Vector3[] selectedShape = possibleShapes[Random.Range(0, possibleShapes.Count)];

        // 선택된 조합에 따라 큐브 생성
        foreach (Vector3 localPosition in selectedShape)
        {
            InstantiateCube(localPosition);
        }
    }

    private void InstantiateCube(Vector3 localPosition)
    {
        // 로컬 좌표를 월드 좌표로 변환
        Vector3 worldPosition = spawnCenter.TransformPoint(localPosition * cubeSize);

        // Y 좌표를 고정 (SpawnCenter의 Y값 사용)
        worldPosition.y = spawnCenter.position.y;

        // 큐브 생성
        GameObject newCube = Instantiate(cubePrefab, worldPosition, Quaternion.identity);
        newCube.transform.SetParent(spawnCenter);
    }

    private void DefineShapes()
    {
        // 3개 조합 (ㄱ, ㄴ, ㅡ)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up });       // ㄱ자
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.forward }); // ㄴ자
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.left });    // ㅡ자

        // 4개 조합 (ㅁ, ㄱ, ㅗ)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.up + Vector3.right }); // ㅁ자
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.up + Vector3.forward }); // ㄱ자 변형
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.up, Vector3.right, Vector3.left }); // ㅗ자

        // 5개 조합 (복잡한 구조)
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.forward, Vector3.left }); // 십자 모양
        possibleShapes.Add(new Vector3[] { Vector3.zero, Vector3.right, Vector3.right + Vector3.up, Vector3.up, Vector3.forward }); // ㅁ자 변형
    }
}
