using UnityEngine;
using static Define;
public class BlockNodeGenerator : MonoBehaviour
{
    Vector3 planeOrigin;
    Vector2 planeSize;
    BlockNode[,] grid;

    private void Start()
    {
        GenerateGrid(1f);
    }
    public BlockNode[,] GenerateGrid(float nodeSize)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Vector3 groundScale = transform.localScale;

        planeSize = new Vector2(renderer.bounds.size.x, renderer.bounds.size.z);
        planeOrigin = new Vector3((-planeSize.x / 2) + 0.5f, 0, (-planeSize.y / 2) + 0.5f);

        int gridX = Mathf.RoundToInt(planeSize.x / nodeSize);
        int gridZ = Mathf.RoundToInt(planeSize.y / nodeSize);

        grid = new BlockNode[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 worldPoint = planeOrigin + new Vector3(x * nodeSize, 0, z * nodeSize);

                grid[x, z] = new BlockNode(worldPoint, true, ETeam.None);
            }
        }

        return grid;
    }
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (BlockNode node in grid)
            {
                Gizmos.color = node.placeable ? Color.green : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (2f * 0.1f));
            }
        }
    }
}


public class BlockNode
{
    public Vector3 worldPosition;
    public bool placeable;
    ETeam team;

    public BlockNode(Vector3 worldPosition, bool placeable, ETeam team)
    {
        this.worldPosition = worldPosition;
        this.placeable = placeable;
        this.team = team;
    }
}