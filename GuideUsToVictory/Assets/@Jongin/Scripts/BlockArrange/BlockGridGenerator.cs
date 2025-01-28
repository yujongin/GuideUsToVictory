using System.Collections.Generic;
using UnityEngine;
using static Define;
public class BlockGridGenerator : MonoBehaviour
{
    Vector3 planeOrigin;
    Vector2 planeSize;
    BlockCell[,] grid;


    public BlockCell[,] GenerateGrid(float nodeSize)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Vector3 groundScale = transform.localScale;

        planeSize = new Vector2(renderer.bounds.size.x, renderer.bounds.size.z);
        planeOrigin = new Vector3((-planeSize.x / 2) + 0.5f, 0, (-planeSize.y / 2) + 0.5f);

        int gridX = Mathf.RoundToInt(planeSize.x / nodeSize);
        int gridZ = Mathf.RoundToInt(planeSize.y / nodeSize);

        grid = new BlockCell[gridX, gridZ];

        for (int x = 0; x < gridX; x++)
        {
            for (int z = 0; z < gridZ; z++)
            {
                Vector3 worldPosition = transform.position + planeOrigin + new Vector3(x * nodeSize, 0, z * nodeSize);
                if (x < 2 && z < 2)
                {
                    grid[x, z] = new BlockCell(new Vector2(x, z), worldPosition, false, ETeam.Blue);
                }
                else if(x >= grid.GetLength(0) - 2 && z >= grid.GetLength(1) - 2)
                {
                    grid[x, z] = new BlockCell(new Vector2(x, z), worldPosition, false, ETeam.Red);
                }
                else
                {
                    grid[x, z] = new BlockCell(new Vector2(x, z), worldPosition, true, ETeam.None);
                }

            }
        }

        return grid;
    }


}


public class BlockCell
{
    public Vector2 cellPos;
    public Vector3 worldPosition;
    public bool placeable;
    public ETeam team;

    public BlockCell(Vector2 cellPos, Vector3 localPosition, bool placeable, ETeam team)
    {
        this.cellPos = cellPos;
        this.worldPosition = localPosition;
        this.placeable = placeable;
        this.team = team;
    }
}