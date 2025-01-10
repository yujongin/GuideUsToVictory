using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab; 
    public Transform spawnCenter; 
    public int minCubes = 3;      
    public int maxCubes = 4;      
    public float spawnRange = 1.5f; 

    private List<GameObject> spawnedCubes = new List<GameObject>(); // 积己等 钮宏 府胶飘

    public void GenerateCubes()
    {
        
        ClearCubes();

        
        int cubeCount = Random.Range(minCubes, maxCubes + 1);

        for (int i = 0; i < cubeCount; i++)
        {
            
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange), 
                Random.Range(-spawnRange, spawnRange),
                0                                     
            );

            
            GameObject cube = Instantiate(cubePrefab, spawnCenter.position + randomPosition, Quaternion.identity);

           
            cube.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);

            
            cube.transform.SetParent(spawnCenter);

            
            spawnedCubes.Add(cube);
        }
    }

    public void ClearCubes()
    {
        
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }

        spawnedCubes.Clear();
    }
}
