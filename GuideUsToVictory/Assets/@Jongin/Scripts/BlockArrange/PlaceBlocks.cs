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
                //1. 내 팀이랑 블럭이 붙어있는지 확인
                //2. 다른 블럭이 있는지 만약에 있으면 블럭 머테리얼 보라색으로 겹치는 애만
                //3. 회전 했을 때 다시 계산
            }

            if (Input.GetMouseButtonDown(0))
            {
                target = null;

                //gamemanager랑 연결 
            }
        }

        //BlockNode GetNodeFromWolrdPos(Vector3 worldPos)
        //{

            
        //}
    }


}
