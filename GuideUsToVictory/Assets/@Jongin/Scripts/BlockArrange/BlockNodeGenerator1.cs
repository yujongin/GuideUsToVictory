using System.Collections.Generic;
using UnityEngine;
using static Define;
public class BlockNodeGenerator1 : MonoBehaviour
{
    Vector3 planeOrigin;
    Vector2 planeSize;
    BlockNode1[,] grid;

    private void Start()
    {
        GenerateGrid(1f);
    }
    public BlockNode1[,] GenerateGrid(float nodeSize)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Vector3 groundScale = transform.localScale;

        planeSize = new Vector2(renderer.bounds.size.x, renderer.bounds.size.z);
        planeOrigin = new Vector3((-planeSize.x / 2) + 0.5f, 0, (-planeSize.y / 2) + 0.5f);

        int gridX = Mathf.RoundToInt(planeSize.x / nodeSize);
        int gridZ = Mathf.RoundToInt(planeSize.y / nodeSize);

        grid = new BlockNode1[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 worldPoint = planeOrigin + new Vector3(x * nodeSize, 0, z * nodeSize);
                if (x < 2 && z < 2)
                {
                    grid[x, z] = new BlockNode1(new Vector2(x, z), worldPoint, false, ETeam.Blue);
                    myBlocks.Add(grid[x, z]);
                }
                else
                {
                    grid[x, z] = new BlockNode1(new Vector2(x, z), worldPoint, true, ETeam.None);
                }

            }
        }

        return grid;
    }

    List<BlockNode1> myBlocks = new List<BlockNode1>();
    Vector2[] dir = new Vector2[4]
    {
        new Vector2(0,1), new Vector2(1,0), new Vector2(-1,0), new Vector2(0,-1)
    };
    public List<BlockNode1> GetNeighborNodes(ETeam team)
    {
        List<BlockNode1> neighborNodes = new List<BlockNode1>();
        for (int i = 0; i < myBlocks.Count; i++)
        {
            for (int j = 0; j < dir.Length; j++)
            {
                int cellX = (int)(myBlocks[i].cellPos.x + dir[j].x);
                int cellY = (int)(myBlocks[i].cellPos.y + dir[j].y);

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
        List<BlockNode1> nodes = GetNeighborNodes(ETeam.Blue);
        if (grid != null)
        {
            foreach (BlockNode1 node in grid)
            {
                if (node.team == ETeam.Blue)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }
                else
                {
                    Gizmos.color = node.placeable ? Color.green : Color.red;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }

                if (nodes.Contains(node))
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
                }
            }
        }
    }
}


public class BlockNode1
{
    public Vector2 cellPos;
    public Vector3 worldPosition;
    public bool placeable;
    public ETeam team;

    public BlockNode1(Vector2 cellPos, Vector3 worldPosition, bool placeable, ETeam team)
    {
        this.cellPos = cellPos;
        this.worldPosition = worldPosition;
        this.placeable = placeable;
        this.team = team;
    }
}