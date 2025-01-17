using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MapManager : MonoBehaviour
{
    public float nodeSize = 1.0f;

    public Node[,] map;

    public GameObject[] towers;

    void Awake()
    {
        map = FindFirstObjectByType<GridGenerator>().GenerateGrid(nodeSize);
    }

    public UnitBase GetTower(string team)
    {
        for(int i = 0; i<towers.Length; i++)
        {
            if ((1 << towers[i].layer) == LayerMask.GetMask(team))
            {
                return towers[i].GetComponent<UnitBase>();   
            }
        }
        return null;
    }

    #region PathFinding
    List<Vector2> _delta = new List<Vector2>()
    {
        new Vector2(0, 1), // U
		new Vector2(1, 1), // UR
		new Vector2(1, 0), // R
		new Vector2(1, -1), // DR
		new Vector2(0, -1), // D
		new Vector2(-1, -1), // LD
		new Vector2(-1, 0), // L
		new Vector2(-1, 1), // LU
	};

    public struct PQNode : IComparable<PQNode>
    {
        public float h; // Heuristic
        public Vector2 cellPos;
        public int depth;

        public int CompareTo(PQNode other)
        {
            if (h == other.h)
                return 0;
            return h < other.h ? 1 : -1;
        }
    }
    public List<Vector2> FindPath(Vector2 startCellPos, Vector2 destCellPos, int maxDepth = 10)
    {
        // 가장 좋은 후보 기록
        Dictionary<Vector2, float> best = new Dictionary<Vector2, float>();

        // 경로 추적용
        Dictionary<Vector2, Vector2> parent = new Dictionary<Vector2, Vector2>();

        // 발견된 후보 중 가장 좋은 후보를 빠르게 뽑기 위한 우선순위 큐
        PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

        Vector2 pos = startCellPos;
        Vector2 dest = destCellPos;

        // 시작 지점 초기화
        float heuristic = (dest - pos).sqrMagnitude;
        pq.Push(new PQNode() { h = heuristic, cellPos = pos, depth = 0 });
        parent[pos] = pos;
        best[pos] = heuristic;

        Vector2 closestCellPos = startCellPos;
        float closestH = heuristic;

        while (pq.Count > 0)
        {
            // 제일 좋은 후보 가져오기
            PQNode node = pq.Pop();
            pos = node.cellPos;

            // 목적지에 도달했으면 종료
            if (pos == dest)
                break;

            // 최대 탐색 깊이를 초과하면 종료
            if (node.depth >= maxDepth)
                continue;

            foreach (Vector2 delta in _delta)
            {
                Vector2 next = pos + delta;

                // 이동 가능 여부 확인
                if (!IsWithinBounds(next) || !CanGo(next))
                    continue;

                // 휴리스틱 계산
                float newHeuristic = (dest - next).sqrMagnitude;

                // 더 나은 경로가 발견되지 않은 경우 건너뛰기
                if (best.ContainsKey(next) && best[next] <= newHeuristic)
                    continue;

                // 더 나은 경로로 갱신
                best[next] = newHeuristic;             
                parent[next] = pos;

                // 후보 추가
                pq.Push(new PQNode() { h = newHeuristic, cellPos = next, depth = node.depth + 1 });

                // 가장 가까운 노드 갱신
                if (closestH > newHeuristic)
                {
                    closestH = newHeuristic;
                    closestCellPos = next;
                }
            }
        }

        // 목적지로 가는 경로가 없으면 가장 가까운 경로 반환
        if (!parent.ContainsKey(dest))
            return CalcCellPathFromParent(parent, closestCellPos);

        // 목적지 경로 반환
        return CalcCellPathFromParent(parent, dest);
    }

    private bool CanGo(Vector2 next)
    {
        int x = (int)next.x;
        int z = (int)next.y;

        if (!IsWithinBounds(next))
            return false;

        return map[x, z].walkable;
    }
    private bool IsWithinBounds(Vector2 cellPos)
    {
        int x = (int)cellPos.x;
        int z = (int)cellPos.y;

        return x >= 0 && x < map.GetLength(0) && z >= 0 && z < map.GetLength(1);
    }
    private List<Vector2> CalcCellPathFromParent(Dictionary<Vector2, Vector2> parent, Vector2 dest)
    {
        List<Vector2> cells = new List<Vector2>();

        if(parent.ContainsKey(dest) == false) return cells;

        Vector2 now = dest;

        while (parent[now] != now)
        {
            cells.Add(now);
            now = parent[now];
        }

        cells.Add(now);
        cells.Reverse();

        return cells;
    }


    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        // Calculate the relative position of the point on the grid
        Vector3 relativePosition = worldPosition - map[0,0].worldPosition;

        // Calculate the node index
        int x = Mathf.FloorToInt(relativePosition.x / nodeSize);
        int z = Mathf.FloorToInt(relativePosition.z / nodeSize);

        // Ensure indices are within grid bounds
        x = Mathf.Clamp(x, 0, map.GetLength(0) - 1);
        z = Mathf.Clamp(z, 0, map.GetLength(1) - 1);

        // Return the corresponding node
        return map[x, z];
    }

    // 셀 좌표를 노드로 변환
    public Node GetNodeFromCellPosition(Vector2 cellPos)
    {
        int x = (int)cellPos.x;
        int z = (int)cellPos.y;

        return map[x, z];
    }


    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    map[146,18].walkable = false;
        //    // 테스트 시작 및 도착 지점
        //    Vector2 start = new Vector2(0, 0);
        //    Vector2 dest = new Vector2(182, 18);

        //    // 경로 계산
        //    debugPath = FindPath(start, dest);

        //    // 콘솔에 경로 출력
        //    Debug.Log("Path:");
        //    foreach (Vector2 pos in debugPath)
        //    {
        //        Debug.Log(pos);
        //    }
        //}
    }


    #endregion


}
