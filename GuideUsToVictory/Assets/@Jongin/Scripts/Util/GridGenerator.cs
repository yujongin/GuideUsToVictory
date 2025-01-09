using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public float nodeSize = 2.0f;

    public LayerMask obstacleMask;
    public LayerMask walkableMask;

    Vector3 planeOrigin;
    Vector2 planeSize;
    Node[,] grid;

    public Node[,] GenerateGrid()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Vector3 groundScale = transform.localScale;

        planeSize = new Vector2(renderer.bounds.size.x, renderer.bounds.size.z);
        planeOrigin = new Vector3(-planeSize.x / 2, 0, -planeSize.y / 2);

        int gridX = Mathf.RoundToInt(planeSize.x / nodeSize);
        int gridZ = Mathf.RoundToInt(planeSize.y / nodeSize);

        grid = new Node[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 worldPoint = planeOrigin + new Vector3(x * nodeSize, 0, z * nodeSize);
                bool walkable = Physics.CheckBox(worldPoint, Vector3.one, Quaternion.identity, walkableMask)
                                && !Physics.CheckBox(worldPoint, Vector3.one, Quaternion.identity, obstacleMask);

                grid[x, z] = new Node(walkable, worldPoint, x, z);
            }
        }

        return grid;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach(Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeSize * 0.1f));
            }
        }
    }
#endif
}

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridZ;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridZ)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
}
