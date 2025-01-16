using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public List<GameObject> blueUnitPrefabs;
    public List<GameObject> redUnitPrefabs;

    public Dictionary<string, List<GameObject>> Units = new Dictionary<string, List<GameObject>>();
    public GameObject Instantiate(GameObject prefab, Transform parent = null, bool pooling = false)
    {
        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }

    public GameObject GetUnitPrefab(string team, string unitName)
    {
        for(int i = 0; i< Units[team].Count; i++)
        {
            if (Units[team][i].name.Contains(unitName))
            {
                return Units[team][i];
            }
        }
        return null;
    }
    public void Destroy(GameObject go, float delay)
    {
        if (go == null)
            return;
        
        StartCoroutine(DelayedPush(go,delay));

    }
    private IEnumerator DelayedPush(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Managers.Pool.Push(go))
            yield break;

        Object.Destroy(go);
    }
    void Start()
    {
        Units.Add("Blue", blueUnitPrefabs);
        Units.Add("Red", redUnitPrefabs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(GetUnitPrefab("Blue", "JuniorKnight"),null,true);
            Instantiate(GetUnitPrefab("Red", "JuniorKnight"),null,true);
        }
    }
}
