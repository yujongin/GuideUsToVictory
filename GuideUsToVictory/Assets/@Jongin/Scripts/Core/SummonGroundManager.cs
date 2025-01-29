using UnityEngine;
using static Define;

public class SummonGroundManager : MonoBehaviour
{
    public BlockCell[,] grid;
    int nodeSize = 1;

    BlockGenerator blockGenerator;
    BlockPlacementAI placementAI;

    public Transform blockSummonPos;
    void Awake()
    {
        grid = FindFirstObjectByType<BlockGridGenerator>().GenerateGrid(nodeSize);
        blockGenerator = FindFirstObjectByType<BlockGenerator>(); 
        placementAI = FindFirstObjectByType<BlockPlacementAI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject go = blockGenerator.GetRandomBlock();
            go.transform.position = blockSummonPos.position;

            placementAI.block = go;
            placementAI.FindBestPosition();
        }
    }

    public BlockCell GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        Vector3 relativePosition = worldPosition - grid[0, 0].worldPosition;
        int x = Mathf.FloorToInt(relativePosition.x / nodeSize);
        int z = Mathf.FloorToInt(relativePosition.z / nodeSize);

        if (x < 0 || x > grid.GetLength(0) - 1 || z < 0 || z > grid.GetLength(1) - 1) return null;
        if (grid[x,z].placeable == false) return null;
        return grid[x, z];
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
