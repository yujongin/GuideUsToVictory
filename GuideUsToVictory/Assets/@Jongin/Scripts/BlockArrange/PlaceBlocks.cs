using UnityEngine;

public class PlaceBlocks : MonoBehaviour
{
    BlockNode[,] blockNodes;
    public GameObject target;
    public BlockNodeGenerator blockNodeGenerator;
    private void Start()
    {
        blockNodes = blockNodeGenerator.GenerateGrid(1f);
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
                BlockNode nearestNode = null;
                float tempdist = float.MaxValue;
                foreach (BlockNode node in blockNodes)
                {
                    float dist = Vector3.Distance(node.worldPosition, hitPoint);
                    if (dist < tempdist)
                    {
                        tempdist = dist;
                        nearestNode = node;
                    }
                }

                if (nearestNode != null)
                    target.transform.position = nearestNode.worldPosition;
                //1. �� ���̶� ���� �پ��ִ��� Ȯ��
                //2. �ٸ� ���� �ִ��� ���࿡ ������ �� ���׸��� ��������� ��ġ�� �ָ�
                //3. ȸ�� ���� �� �ٽ� ���
            }

            if (Input.GetMouseButtonDown(0))
            {
                target = null;

                //gamemanager�� ���� 
            }
        }

        //BlockNode GetNodeFromWolrdPos(Vector3 worldPos)
        //{

            
        //}
    }


}
