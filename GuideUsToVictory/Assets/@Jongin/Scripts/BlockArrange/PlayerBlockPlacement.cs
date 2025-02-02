using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockPlacement : MonoBehaviour
{
    BlockCell[,] grid;
    public GameObject target;

    public Material defaultMaterial;
    public Material overlapMaterial;

    private List<Vector3> existingBlocks = new List<Vector3>();
    public GameObject StartingCube;

    private void Start()
    {
        grid = Managers.SummonGround.grid;

        foreach (Transform child in StartingCube.transform)
        {
            existingBlocks.Add(child.position);
        }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 hitPoint = hit.point;
                BlockCell nearestNode = null;
                float tempDist = float.MaxValue;

                foreach (BlockCell node in grid)
                {
                    float dist = Vector3.Distance(node.worldPosition, hitPoint);
                    if (dist < tempDist)
                    {
                        tempDist = dist;
                        nearestNode = node;
                    }
                }

                if (nearestNode != null)
                {
                    target.transform.position = nearestNode.worldPosition;
                    UpdateOverlapState(nearestNode.worldPosition);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 targetPosition = target.transform.position;


                if (CheckAdjacent(targetPosition) && !IsOverlapping(targetPosition))
                {
                    existingBlocks.Add(targetPosition);
                    ResetMaterials();
                    target = null;
                    Debug.Log("Block placed successfully!");
                }
                else
                {
                    Debug.LogWarning("Cannot place block: Overlapping or not adjacent!");
                }
            }
        }
    }

    private void UpdateOverlapState(Vector3 position)
    {
        ResetMaterials();
        bool isOverlapping = false;

        foreach (Vector3 blockPos in existingBlocks)
        {
            float distance = Vector3.Distance(blockPos, position);
            if (distance < 0.1f)
            {
                isOverlapping = true;

                Collider[] colliders = Physics.OverlapSphere(blockPos, 0.1f);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                    {
                        renderer.material = overlapMaterial;
                    }
                }
            }
        }

        if (!isOverlapping)
        {
            Debug.Log("Valid position for block placement.");
        }
    }

    private bool CheckAdjacent(Vector3 position)
    {
        foreach (Vector3 blockPos in existingBlocks)
        {
            if (Mathf.Approximately(Vector3.Distance(blockPos, position), 1f))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOverlapping(Vector3 position)
    {
        foreach (Vector3 blockPos in existingBlocks)
        {
            if (Vector3.Distance(blockPos, position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetMaterials()
    {
        foreach (Vector3 blockPos in existingBlocks)
        {
            Collider[] colliders = Physics.OverlapSphere(blockPos, 0.1f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                {
                    renderer.material = defaultMaterial;
                }
            }
        }
    }


}
