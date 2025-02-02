using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SummonGroundManager : MonoBehaviour
{
    public BlockCell[,] grid;
    int nodeSize = 1;

    BlockGenerator blockGenerator;
    BlockPlacementAI placementAI;

    public Dictionary<ETeam, List<BlockCell>> teamBlocks;
    public Transform redBlockParent;
    public Transform blueBlockParent;

    void Awake()
    {
        teamBlocks = new Dictionary<ETeam, List<BlockCell>>();
        teamBlocks.Add(ETeam.Blue, new List<BlockCell>());
        teamBlocks.Add(ETeam.Red, new List<BlockCell>());
        grid = FindFirstObjectByType<BlockGridGenerator>().GenerateGrid(nodeSize);
        blockGenerator = FindFirstObjectByType<BlockGenerator>();
        placementAI = FindFirstObjectByType<BlockPlacementAI>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = blockGenerator.GetRandomBlock();

            placementAI.block = go;
            placementAI.FindBestPosition();
        }
    }

    public void AIBlockPlacement(GameObject block)
    {
        placementAI.block = block;
        placementAI.FindBestPosition();
    }
    public BlockCell GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        Vector3 relativePosition = worldPosition - grid[0, 0].worldPosition;
        int x = Mathf.RoundToInt(relativePosition.x / nodeSize);
        int z = Mathf.RoundToInt(relativePosition.z / nodeSize);
        //int x = (int)relativePosition.x;
        //int z = (int)relativePosition.z;

        if (x < 0 || x > grid.GetLength(0) - 1 || z < 0 || z > grid.GetLength(1) - 1) return null;
        if (grid[x, z].placeable == false) return null;
        return grid[x, z];
    }



    Vector2[] dir = new Vector2[4]
    {
        new Vector2(0,1), new Vector2(1,0), new Vector2(-1,0), new Vector2(0,-1)
    };

    public List<BlockCell> GetNeighborNodes(ETeam team)
    {
        List<BlockCell> neighborNodes = new List<BlockCell>();
        neighborNodes.Clear();
        for (int i = 0; i < teamBlocks[team].Count; i++)
        {
            for (int j = 0; j < dir.Length; j++)
            {
                int cellX = (int)(teamBlocks[team][i].cellPos.x + dir[j].x);
                int cellY = (int)(teamBlocks[team][i].cellPos.y + dir[j].y);

                if (cellX < 0 || cellX > grid.GetLength(0) - 1 || cellY < 0 || cellY > grid.GetLength(0) - 1 ||
                    grid[cellX, cellY].placeable == false)
                {
                    continue;
                }

                if (!neighborNodes.Contains(grid[cellX, cellY]))
                {
                    neighborNodes.Add(grid[cellX, cellY]);
                }
            }
        }
        return neighborNodes;
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (BlockCell node in grid)
            {
                if (node.team == ETeam.Blue)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }
                else if (node.team == ETeam.Red)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }
                else
                {
                    Gizmos.color = node.placeable ? Color.green : Color.magenta;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }
            }
        }
    }
}
