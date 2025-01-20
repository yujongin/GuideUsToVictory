using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitSpawnManager : MonoBehaviour
{
    public HashSet<UnitBase> myTeamsSpawned = new HashSet<UnitBase>();
    public HashSet<UnitBase> enemiesSpawned = new HashSet<UnitBase>();

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

    public Vector3 GetRandomSpawnPos(ETeam team)
    {
        Transform baseSpawnPos = team == ETeam.Blue ? BlueSpawnPos : RedSpawnPos;
        float x = Random.Range(-15, 15);
        float z = Random.Range(-15, 15);

        Vector3 spawnPos = baseSpawnPos.position + new Vector3(x, 0, z);
        return spawnPos;
    }

    public void SpawnUnits(TeamData teamData)
    {
        foreach (var unit in teamData.UnitCountDict)
        {
            GameObject prefab = Managers.Resource.GetUnitPrefab(teamData.Team, unit.Key);
            for (int i = 0; i < unit.Value; i++)
            {
                if (teamData.Team == Managers.Game.myTeam.Team)
                {
                    GameObject go = Managers.Resource.Instantiate(prefab, myTeamsRoot, true);
                    myTeamsSpawned.Add(go.GetComponent<UnitBase>());
                }
                else
                {
                    GameObject go = Managers.Resource.Instantiate(prefab, enemiesRoot, true);
                    enemiesSpawned.Add(go.GetComponent<UnitBase>());
                }
            }
        }
    }

    public void DespawnUnit(UnitBase unit)
    {
        if(unit.MyTeam == Managers.Game.myTeam.Team)
        {
            myTeamsSpawned.Remove(unit);
        }
        else
        {
            enemiesSpawned.Remove(unit);
        }
        Managers.Resource.Destroy(unit.gameObject, 2f);
    }
}
