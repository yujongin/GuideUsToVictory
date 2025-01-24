using UnityEngine;

public class BlockGrid : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class BlockNode 
{
    public Vector3 worldPosition;
    public bool placeable;

    public BlockNode(Vector3 worldPosition, bool placeable)
    {
        this.worldPosition = worldPosition;
        this.placeable = placeable;
    }
}