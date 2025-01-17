using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class ResourceManager : MonoBehaviour
{
    public List<GameObject> blueUnitPrefabs;
    public List<GameObject> redUnitPrefabs;

    public Dictionary<ETeam, List<GameObject>> Units = new Dictionary<ETeam, List<GameObject>>();

    void Start()
    {
        Units.Add(ETeam.Blue, blueUnitPrefabs);
        Units.Add(ETeam.Red, redUnitPrefabs);
    }
    public GameObject Instantiate(GameObject prefab, Transform parent = null, bool pooling = false)
    {
        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }

    public GameObject GetUnitPrefab(ETeam team, string unitName)
    {
        for (int i = 0; i < Units[team].Count; i++)
        {
            if (Units[team][i].name.Contains(unitName))
            {
                return Units[team][i];
            }
        }
        return null;
    }
    public void Destroy(GameObject go, float delay = 0)
    {
        if (go == null)
            return;

        StartCoroutine(DelayedPush(go, delay));

    }
    private IEnumerator DelayedPush(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (Managers.Pool.Push(go))
            yield break;

        Object.Destroy(go);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    GameObject go = Instantiate(GetUnitPrefab("Blue", "JuniorKnight"), null, true);
            //    // go.transform.position = new Vector3(200, 6, 0);
            //    // go.GetComponent<UnitController>().SetInfo();
            //    Instantiate(GetUnitPrefab("Red", "JuniorKnight"), null, true);
            //}
            //for (int i = 0; i < 100; i++)
            //{
            //    Instantiate(GetUnitPrefab("Blue", "EliteArcher"), null, true);
            //    // Instantiate(GetUnitPrefab("Red", "EliteArcher"), null, true);
            //}
        }
    }
}
