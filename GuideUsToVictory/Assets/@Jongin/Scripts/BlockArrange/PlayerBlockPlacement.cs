using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBlockPlacement : MonoBehaviour
{
    BlockCell[,] grid;
    public GameObject target;

    [HideInInspector]
    public Material originalMaterial;
    public Material canPlaceMaterial;
    public Material overlapMaterial;

    private List<Vector3> existingBlocks = new List<Vector3>();

    private void Start()
    {
        grid = Managers.SummonGround.grid;

    }
    BlockCell nearestNode = null;
    public List<BlockCell> neighbors;
    bool canPlacement;
    Transform[] blocks;
    private void Update()
    {
        if (target != null)
        {
            if (blocks == null)
            {
                blocks = target.transform.GetComponentsInChildren<Transform>()
                    .Where(child => child != target.transform)
                    .ToArray();
            }

            Vector3 screenPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);


            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;
                BlockCell node = Managers.SummonGround.GetNodeFromWorldPosition(hitPoint);


                if (node != null && nearestNode != node)
                {
                    nearestNode = node;
                    target.transform.position = nearestNode.worldPosition;

                    UpdateOverlapState();
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float angle = target.transform.eulerAngles.y + 90f;
                    if (angle >= 360)
                    {
                        angle = 0;
                    }
                    target.transform.rotation = Quaternion.Euler(0, angle, 0);
                    UpdateOverlapState();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!canPlacement)
                    {
                        Managers.Game.CallNoticeTextFade("현재 자리는 블록을 놓을 수 없습니다.", Color.red);
                        return;
                    }
                    target.transform.parent = Managers.SummonGround.blueBlockParent;
                    Managers.SummonGround.AddBlock(Define.ETeam.Blue, target);
                    ChangeAllBlockMat(blocks, originalMaterial);
                    blocks = null;
                    target = null;
                    Managers.Auction.isPlacement = true;
                }
            }
        }
    }

    private void UpdateOverlapState()
    {
        canPlacement = false;
        bool isNotOverap = true;
        bool isNeighbor = false;
        for (int i = 0; i < blocks.Length; i++)
        {
            Material[] newMaterials = blocks[i].GetComponent<Renderer>().materials;
            newMaterials[1] = canPlaceMaterial;


            BlockCell child = Managers.SummonGround.GetNodeFromWorldPosition(blocks[i].transform.position);
            if (child == null)
            {
                isNotOverap = false;
                newMaterials[1] = overlapMaterial;
            }
            if (neighbors.Contains(child))
            {
                isNeighbor = true;
            }
            blocks[i].GetComponent<Renderer>().materials = newMaterials;
        }
        if (!isNeighbor)
        {
            ChangeAllBlockMat(blocks, overlapMaterial);
        }
        if (isNotOverap && isNeighbor)
        {
            canPlacement = true;
            ChangeAllBlockMat(blocks, canPlaceMaterial);
        }
    }

    public void AutoBlockPlacement()
    {
        ChangeAllBlockMat(blocks, originalMaterial);
        blocks = null;
        target = null;
    }
    public void ChangeAllBlockMat(Transform[] blocks, Material mat)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            Material[] newMaterials = blocks[i].GetComponent<Renderer>().materials;
            newMaterials[1] = mat;
            blocks[i].GetComponent<Renderer>().materials = newMaterials;
        }
    }


}
