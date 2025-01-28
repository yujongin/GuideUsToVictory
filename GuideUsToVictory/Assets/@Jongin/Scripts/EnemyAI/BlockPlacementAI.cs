using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
public class BlockPlacementAI : MonoBehaviour
{
    public GameObject block;
    public BlockCell[,] grid;
    ETeam aiTeam;

    List<BlockCell> myBlocks = new List<BlockCell>();
    List<Vector2[]> rotatedBlockNodes = new List<Vector2[]>();
    int minX = int.MaxValue;
    int maxX = 0;
    int minZ = int.MaxValue;
    int maxZ = 0;
    private void Start()
    {

        //Temp
        aiTeam = ETeam.Red;
        grid = Managers.SummonGround.grid;

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].team == aiTeam)
                {
                    myBlocks.Add(grid[i, j]);
                }
            }
        }
        //aiTeam = Managers.Game.enemyTeamData.Team;
    }
    public List<Vector2[]> GetRotateVectorArray(Vector3[] positions)
    {
        rotatedBlockNodes.Clear();
        Transform[] transforms = block.GetComponentsInChildren<Transform>()
        .Where(child => child != this.transform)
        .ToArray();

        for (int i = 0; i < positions.Length; i++)
        {
            block.transform.GetChild(i).position = positions[i];
        }

        for (int i = 0; i < 4; i++)
        {
            if (i > 0)
            {
                block.transform.rotation = Quaternion.Euler(Vector3.up * 90 * (i));
            }
            Vector2[] pos = new Vector2[transforms.Length - 1];

            for (int j = 0; j < transforms.Length; j++)
            {
                pos[j] = new Vector2(transforms[j].position.x - block.transform.position.x, transforms[j].position.z - block.transform.position.z);
            }
            rotatedBlockNodes.Add(pos);
        }
        block.transform.rotation = Quaternion.identity;
        return rotatedBlockNodes;
    }

    List<Vector3[]> blockPositions = new List<Vector3[]>();
    public List<Vector3[]> GetBlockPositions()
    {
        blockPositions.Clear();
        Transform[] transforms = block.GetComponentsInChildren<Transform>()
        .Where(child => child != this.transform)
        .ToArray();

        Vector3[] positions = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            Vector3 addPos = transforms[0].position - transforms[i].position;
            for (int j = 0; j < transforms.Length; j++)
            {
                positions[j] = transforms[j].position + addPos;
            }
            blockPositions.Add(positions);
        }

        return blockPositions;
    }
    public void FindBestPosition()
    {
        SetMinMaxPos();
        GetNeighborNodes();
        int bestSize = int.MaxValue;
        BlockCell bestNode = null;
        float bestRot = 0;
        int bestPosIndex = -1;
        GetBlockPositions();
        for (int l = 0; l < blockPositions.Count; l++)
        {
            GetRotateVectorArray(blockPositions[l]);
            List<BlockCell> blockPlace = new List<BlockCell>();
            for (int i = 0; i < rotatedBlockNodes.Count; i++)
            {
                Vector2[] pos = rotatedBlockNodes[i];
                for (int k = 0; k < neighborNodes.Count; k++)
                {
                    blockPlace.Clear();
                    for (int j = 0; j < pos.Length; j++)
                    {
                        Vector3 nodePos = neighborNodes[k].worldPosition + new Vector3(pos[j].x, 0, pos[j].y);
                        BlockCell cell = Managers.SummonGround.GetNodeFromWorldPosition(nodePos);
                        if (cell == null) break;
                        blockPlace.Add(cell);
                    }

                    if (blockPlace.Count == pos.Length)
                    {
                        int size = GetSizeMyCells(blockPlace);
                        if (size < bestSize)
                        {
                            bestSize = size;
                            bestNode = neighborNodes[k];
                            bestRot = 90 * i;
                            bestPosIndex = l;
                        }
                    }
                }
            }
        }

        //실제 배치
    }
    int GetSizeMyCells(List<BlockCell> blockPlace)
    {
        Vector2 maxPos = new Vector2(maxX, maxZ);
        Vector2 minPos = new Vector2(minX, minZ);
        for (int i = 0; i < blockPlace.Count; i++)
        {
            if (blockPlace[i].cellPos.x > maxPos.x)
                maxPos.x = blockPlace[i].cellPos.x;
            if (blockPlace[i].cellPos.x < minPos.x)
                minPos.x = blockPlace[i].cellPos.x;
            if (blockPlace[i].cellPos.y > maxPos.y)
                maxPos.y = blockPlace[i].cellPos.y;
            if (blockPlace[i].cellPos.y < minPos.y)
                minPos.y = blockPlace[i].cellPos.y;
        }
        return (int)((maxPos.x - minPos.x + 1) * (maxPos.y - minPos.y + 1));
    }
    void SetMinMaxPos()
    {
        for (int i = 0; i < myBlocks.Count; i++)
        {
            if (myBlocks[i].cellPos.x > maxX)
                maxX = (int)myBlocks[i].cellPos.x;

            if (myBlocks[i].cellPos.x < minX)
                minX = (int)myBlocks[i].cellPos.x;

            if (myBlocks[i].cellPos.y > maxZ)
                maxZ = (int)myBlocks[i].cellPos.y;

            if (myBlocks[i].cellPos.y < minZ)
                minZ = (int)myBlocks[i].cellPos.y;
        }
    }





    Vector2[] dir = new Vector2[4]
    {
        new Vector2(0,1), new Vector2(1,0), new Vector2(-1,0), new Vector2(0,-1)
    };

    List<BlockCell> neighborNodes = new List<BlockCell>();
    public List<BlockCell> GetNeighborNodes()
    {
        neighborNodes.Clear();
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




}
