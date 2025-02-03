using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Define;
public class BlockPlacementAI : MonoBehaviour
{
    public GameObject block;
    public BlockCell[,] grid;

    List<BlockCell> myBlocks;
    List<Vector2[]> rotatedBlockNodes = new List<Vector2[]>();
    int minX = int.MaxValue;
    int maxX = 0;
    int minZ = int.MaxValue;
    int maxZ = 0;

    List<BlockCell> neighborNodes = new List<BlockCell>();
    private void Start()
    {
        grid = Managers.SummonGround.grid;
    }
    public List<Vector2[]> GetRotateVectorArray(Vector3[] positions)
    {
        rotatedBlockNodes.Clear();
        Transform[] transforms = block.GetComponentsInChildren<Transform>()
        .Where(child => child != block.transform)
        .ToArray();

        for (int i = 0; i < positions.Length; i++)
        {
            transforms[i].localPosition = positions[i];
        }

        for (int i = 0; i < 4; i++)
        {
            block.transform.rotation = Quaternion.Euler(Vector3.up * 90 * (i));

            Vector2[] pos = new Vector2[transforms.Length];

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
        .Where(child => child != block.transform)
        .ToArray();

        Vector3[] positions = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            Vector3 addPos = transforms[0].localPosition - transforms[i].localPosition;
            for (int j = 0; j < transforms.Length; j++)
            {
                positions[j] = transforms[j].localPosition + addPos;
            }
            blockPositions.Add(positions);
        }

        return blockPositions;
    }
    public void FindBestPosition(ETeam team)
    {
        SetMinMaxPos(team);
        neighborNodes = Managers.SummonGround.GetNeighborNodes(team);
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
                        //Debug.Log("pos: " + new Vector3(pos[j].x, 0, pos[j].y));
                        //Debug.Log("nodePos: " +  nodePos);
                        if (cell == null)
                        {
                            break;
                        }
                        //Debug.Log("cellPos: " + cell.cellPos);
                        blockPlace.Add(cell);
                    }


                    if (blockPlace.Count == pos.Length)
                    {
                        //for (int z = 0; z < blockPlace.Count; z++)
                        //{
                        //    Debug.Log("cellPos: " + blockPlace[z].cellPos);
                        //}
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
        block.transform.position = bestNode.worldPosition;
        block.transform.rotation = Quaternion.Euler(0, bestRot, 0);
        for (int i = 0; i < block.transform.childCount; i++)
        {
            block.transform.GetChild(i).localPosition = blockPositions[bestPosIndex][i];
            BlockCell cell = Managers.SummonGround.GetNodeFromWorldPosition(block.transform.GetChild(i).position);
            Managers.SummonGround.teamBlocks[team].Add(cell);

            for (int j = 0; j < 4; j++)
            {
                if (cell.cellPos == Managers.SummonGround.unitUnlockNode[j])
                {
                    Managers.Game.UnlockUnit(team);
                }
            }
            cell.team = team;
            cell.placeable = false;
        }
        block.transform.parent = team == ETeam.Blue ? Managers.SummonGround.blueBlockParent : Managers.SummonGround.redBlockParent;
        Managers.Game.IncreaseMaxBlock(team, block.transform.childCount);
    }
    int GetSizeMyCells(List<BlockCell> blockPlace)
    {
        Vector2Int maxPos = new Vector2Int(maxX, maxZ);
        Vector2Int minPos = new Vector2Int(minX, minZ);
        //Debug.Log("maxPos: " + maxPos + ", minPos : " + minPos);

        for (int i = 0; i < blockPlace.Count; i++)
        {
            int x = Mathf.RoundToInt(blockPlace[i].cellPos.x);
            int y = Mathf.RoundToInt(blockPlace[i].cellPos.y);
            //Debug.Log("cellposX: " + x + ", cellposY : " + y);
            if (x > maxPos.x) maxPos.x = x;
            if (x < minPos.x) minPos.x = x;
            if (y > maxPos.y) maxPos.y = y;
            if (y < minPos.y) minPos.y = y;

            //Debug.Log("maxPos: " + maxPos + ", minPos : " + minPos);
        }

        //Debug.Log("Result : " + (maxPos.x - minPos.x + 1) * (maxPos.y - minPos.y + 1));
        return (maxPos.x - minPos.x + 1) * (maxPos.y - minPos.y + 1);
    }
    void SetMinMaxPos(ETeam team)
    {
        myBlocks = Managers.SummonGround.teamBlocks[team];
        maxX = Mathf.RoundToInt(myBlocks[0].cellPos.x);
        minX = Mathf.RoundToInt(myBlocks[0].cellPos.x);
        maxZ = Mathf.RoundToInt(myBlocks[0].cellPos.y);
        minZ = Mathf.RoundToInt(myBlocks[0].cellPos.y);
        for (int i = 0; i < myBlocks.Count; i++)
        {
            if (Mathf.RoundToInt(myBlocks[i].cellPos.x) > maxX)
                maxX = Mathf.RoundToInt(myBlocks[i].cellPos.x);

            if (Mathf.RoundToInt(myBlocks[i].cellPos.x) < minX)
                minX = Mathf.RoundToInt(myBlocks[i].cellPos.x);

            if (Mathf.RoundToInt(myBlocks[i].cellPos.y) > maxZ)
                maxZ = Mathf.RoundToInt(myBlocks[i].cellPos.y);

            if (Mathf.RoundToInt(myBlocks[i].cellPos.y) < minZ)
                minZ = Mathf.RoundToInt(myBlocks[i].cellPos.y);
        }
    }


}
