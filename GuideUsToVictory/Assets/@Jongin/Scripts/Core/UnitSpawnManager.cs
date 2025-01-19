using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitSpawnManager : MonoBehaviour
{
    public HashSet<UnitBase> myTeamsSpawned = new HashSet<UnitBase>();
    public HashSet<UnitBase> enemiesSpawned = new HashSet<UnitBase>();

    public void Init()
    {

    }
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
        {
            root = new GameObject { name = name };
        }
        return root.transform;
    }

    public Transform myTeamsRoot { get { return GetRootTransform("@MyTeams"); } }
    public Transform enemiesRoot { get { return GetRootTransform("@Enemies"); } }

    public Transform BlueSpawnPos;
    public Transform RedSpawnPos;


    Vector3 GetRandomSpawnPos(ETeam team)
    {
        Transform baseSpawnPos = team == ETeam.Blue ? BlueSpawnPos : RedSpawnPos;
        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        Vector3 spawnPos = baseSpawnPos.position + new Vector3(x, 0, z);
        return spawnPos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Resource.GetUnitPrefab(ETeam.Red, "JuniorKnight"), null, true);
            go.transform.position = GetRandomSpawnPos(ETeam.Red);
        }
    }
}
