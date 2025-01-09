using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{

    public Node[,] map;
    void Start()
    {
        map = FindFirstObjectByType<GridGenerator>().GenerateGrid();
    }

    List<Vector2Int> _delta = new List<Vector2Int>()
    {
        new Vector2Int(0, 1), // U
		new Vector2Int(1, 1), // UR
		new Vector2Int(1, 0), // R
		new Vector2Int(1, -1), // DR
		new Vector2Int(0, -1), // D
		new Vector2Int(-1, -1), // LD
		new Vector2Int(-1, 0), // L
		new Vector2Int(-1, 1), // LU
	};
    public List<Vector2Int> FindPath(UnitBase self, Vector2Int startCellPos, Vector2Int destCellPos, int maxDepth = 10)
    {
        //가장 좋은 후보 기록
        Dictionary<Vector2Int, int> best = new Dictionary<Vector2Int, int>();
        
        //경로 추적 용
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();

        



        return new List<Vector2Int>() { };
    }
    void Update()
    {
        
    }
}
