using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitController : UnitBase
{
    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        Skill,
        Dead
    }

    UnitState unitState;

    Node startNode;
    Node destNode; 
    List<Vector2> path;

    float detectRange = 10f;

    void Start()
    {
        animator = GetComponent<Animator>();    
        unitState = UnitState.Idle;
        SetState(UnitState.Move);
    }

    void Update()
    {
    }

    public void SetState(UnitState state)
    {
        unitState = state;
        switch (unitState)
        {
            case UnitState.Idle:
                animator.SetTrigger("Idle");
                break;
            case UnitState.Move:
                animator.SetTrigger("Move");
                StartCoroutine(UpdateMove(stat.Speed));
                break;
            case UnitState.Attack:
                //base.Attack(target);
                break;
            case UnitState.Skill:
                break;
            case UnitState.Dead:
                OnDead();
                break;
        }

    }

    void UpdateIdle()
    {
        animator.SetTrigger("Idle");
    }
    void UpdateAttack()
    {

    }
    void UpdateSkill()
    {

    }

    void UpdateDead()
    {

    }

    bool IsLerpCellPosCompleted = true;
    Node next;

    IEnumerator UpdateMove(float moveSpeed)
    {
        while(unitState == UnitState.Move)
        {
            if (IsLerpCellPosCompleted)
            {
                startNode = Managers.Map.GetNodeFromWorldPosition(transform.position);

                destNode = Managers.Map.GetNodeFromWorldPosition(Managers.Map.GetTowerPos(tag));

                List<Vector2> path = Managers.Map.FindPath(startNode.cellPos, destNode.cellPos);

                if (path.Count < 2) yield break;

                if (next != null) { next.walkable = true; }
                next = Managers.Map.GetNodeFromCellPosition(path[1]);
                next.walkable = false;
                IsLerpCellPosCompleted = false;
            }
            else
            {
                Vector3 dirVec = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z) - transform.position;
                if (dirVec.magnitude < 0.01f)
                {
                    transform.position = new Vector3(next.worldPosition.x, transform.position.y, next.worldPosition.z);
                    IsLerpCellPosCompleted = true;
                }

                float moveDist = Mathf.Min(dirVec.magnitude, moveSpeed * Time.deltaTime);
                transform.position += dirVec.normalized * moveDist;
            }
            yield return null;
        }
        yield break;
    }


    public void DetectTarget()
    {

    }

    private List<Vector2> debugPath = new List<Vector2>();

    void OnDrawGizmos()
    {
        // 경로를 시각적으로 확인
        Gizmos.color = Color.yellow;

        if (debugPath != null && debugPath.Count > 1)
        {
            for (int i = 0; i < debugPath.Count - 1; i++)
            {
                // 각 경로 지점을 선으로 연결
                Vector3 from = Managers.Map.GetNodeFromCellPosition(debugPath[i]).worldPosition;
                Vector3 to = Managers.Map.GetNodeFromCellPosition(debugPath[i + 1]).worldPosition;

                Gizmos.DrawLine(from, to);
            }
        }
    }
}
