using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : UnitBase
{

    string myTeam;
    string enemyTeam;

    [SerializeField]
    UnitBase target;
    Animator animator;
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

    float detectRange = 50f;

    public LayerMask enemyLayer;
    void Start()
    {
        animator = GetComponent<Animator>();
        myTeam = LayerMask.LayerToName(gameObject.layer);
        enemyTeam = myTeam == "Blue" ? "Red" : "Blue";
        enemyLayer = LayerMask.GetMask(enemyTeam);

        unitState = UnitState.Idle;
        SetState(UnitState.Move);
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
                StartCoroutine(UpdateMove(baseStat.Speed));
                break;
            case UnitState.Attack:
                StartCoroutine(UpdateAttack(baseStat.AttackSpeed));
                break;
            case UnitState.Skill:
                break;
            case UnitState.Dead:
                break;
        }

    }

    bool IsLerpCellPosCompleted = true;
    Node next;
    IEnumerator UpdateAttack(float attackSpeed)
    {

        while (unitState == UnitState.Attack)
        {
            animator.SetTrigger("Attack");
            AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(0);

            foreach (var anim in info)
            {
                Debug.Log(anim.clip.name);
                //if (anim.clip.name == "Attack")
                //{

                //}
            }

            float animationLength = 0.5f;
            yield return new WaitForSeconds(animationLength + 1 / attackSpeed);
        }
    }

    IEnumerator UpdateMove(float moveSpeed)
    {
        while (unitState == UnitState.Move)
        {
            if (IsLerpCellPosCompleted)
            {
                startNode = Managers.Map.GetNodeFromWorldPosition(transform.position);

                DetectTarget();
                if (target != null)
                {
                    destNode = Managers.Map.GetNodeFromWorldPosition(target.transform.position);
                }
                else
                {
                    destNode = Managers.Map.GetNodeFromWorldPosition(Managers.Map.GetTowerPos(enemyTeam));
                }

                List<Vector2> path = Managers.Map.FindPath(startNode.cellPos, destNode.cellPos);

                if (path.Count < 2) yield break;

                if (next != null) { next.walkable = true; }
                next = Managers.Map.GetNodeFromCellPosition(path[1]);
                next.walkable = false;
                IsLerpCellPosCompleted = false;
            }
            else
            {
                if(target != null)
                {
                    if(Vector3.Distance(transform.position, target.transform.position) < baseStat.AttackRange)
                    {
                        animator.SetTrigger("Idle");
                        SetState(UnitState.Attack);
                        yield break;
                    }
                }
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

    Collider[] detectedEnemy = new Collider[1];
    public void DetectTarget()
    {
        Physics.OverlapSphereNonAlloc(transform.position, detectRange, detectedEnemy, enemyLayer);
        if (detectedEnemy[0] != null)
        {
            target = detectedEnemy[0].GetComponent<UnitBase>();
        }
    }
    public void OnDamage()
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
