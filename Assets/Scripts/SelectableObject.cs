using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public enum EMoveState { NONE = -1, NORMAL, ATTACK, PATROL, FOLLOW, CHASE, FOLLOW_ENEMY }
    public ESelectableObjectType ObjectType => objectType;
    public Vector3 GetPos => transform.position;

    public void Init(int _nodeIdx, NodeUpdateDelegate _updateNodeCallback = null)
    {
        updateNodeCallback = _updateNodeCallback;
        stateMachine = GetComponent<StateMachine>();

        if (stateMachine != null)
        {
            stateMachine.Init(_nodeIdx, _updateNodeCallback);

            //stateMachine.ChangeState(stateMachine.GetState((int)EState.IDLE));
            //StateIdle();
        }
    }

    public void AttackDmg(int _dmg)
    {
        Debug.LogFormat("Hit Dmg {0}", _dmg);
    }


    public void FollowTarget(Transform _targetTr)
    {
        stateMachine.SetTargetTr(_targetTr);

        if (isControllable)
            stateMachine.ChangeState(stateMachine.GetState((int)EState.FOLLOW));
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        //targetPos = _targetPos;
        targetPos = _targetPos;
        moveState = EMoveState.NORMAL;

        if (isControllable)
            StateMove();
    }

    public void MoveAttack(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        moveState = EMoveState.NORMAL;

        if (isControllable)
            StateMove();
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        targetPos = _wayPointTo;

        if (isControllable)
            stateMachine.ChangeState(stateMachine.GetState((int)EState.PATROL));
    }

    public void Stop()
    {
        if (isControllable)
            stateMachine.ChangeState(stateMachine.GetState((int)EState.STOP));
    }

    public void Hold()
    {
        if (isControllable)
            stateMachine.ChangeState(stateMachine.GetState((int)EState.HOLD));
    }


    private void StateIdle()
    {
        StartCoroutine("CheckEnemyInAttRangeCoroutine");
    }

    private IEnumerator CheckEnemyInAttRangeCoroutine()
    {
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(transform.position, traceStartRange);

            if (arrCollider.Length > 0)
            {
                foreach (Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        stateMachine.SetTargetTr(c.transform);
                        StateAttack();
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    private void StateMove()
    {
        StopAllCoroutines();

        switch (moveState)
        {
            case EMoveState.NORMAL:
                StartCoroutine("CheckNormalMoveCoroutine");
                break;
            case EMoveState.ATTACK:
                StartCoroutine("CheckNormalMoveCoroutine");
                StartCoroutine("CheckEnemyInAttRangeCoroutine");
                break;
            case EMoveState.PATROL:
                StartCoroutine("CheckPatrolMoveCoroutine");
                StartCoroutine("CheckEnemyInAttRangeCoroutine");
                break;
            case EMoveState.FOLLOW:
                StartCoroutine("CheckFollowMoveCoroutine");
                break;
            case EMoveState.CHASE:
                StartCoroutine("CheckFollowMoveCoroutine");
                break;
            case EMoveState.FOLLOW_ENEMY:
                StartCoroutine("CheckFollowMoveCoroutine");
                StartCoroutine("CheckEnemyInAttRangeCoroutine");
                break;
            default:
                break;
        }
    }

    private IEnumerator CheckNormalMoveCoroutine()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));

        while (true)
        {
            if (!curWayNode.walkable)
            {
                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                yield return new WaitForSeconds(0.3f);
                stateMachine.TargetPos = curWayNode.worldPos;
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                yield return new WaitForSeconds(0.3f);


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
               if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
            {
                ++targetIdx;

                updateNodeCallback?.Invoke(transform.position, stateMachine.GetNodeIdx());
                // 목적지에 도착시 Stop상태로 변경
                if (targetIdx >= arrPath.Length)
                {
                    FinishState();
                    curWayNode = null;
                    yield break;
                }
                curWayNode = arrPath[targetIdx];
                stateMachine.TargetPos = curWayNode.worldPos;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckPatrolMoveCoroutine()
    {
        Vector3 wayPointFrom = transform.position;
        Vector3 wayPointTo = targetPos;

        PF_PathRequestManager.RequestPath(wayPointFrom, wayPointTo, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNodePos;
        stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));

        while (true)
        {
            if (!curWayNode.walkable)
            {
                PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
                yield return new WaitForSeconds(0.1f);
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                yield return new WaitForSeconds(0.1f);


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
            {
                ++targetIdx;
                updateNodeCallback?.Invoke(transform.position, stateMachine.GetNodeIdx());
                // 목적지에 도착시 Stop상태로 변경
                if (targetIdx >= arrPath.Length)
                {
                    Vector3 tempWayPoint = wayPointFrom;
                    wayPointFrom = wayPointTo;
                    wayPointTo = tempWayPoint;

                    PF_PathRequestManager.RequestPath(wayPointFrom, wayPointTo, OnPathFound);
                    yield return null;

                    while (targetIdx != 0)
                        yield return null;

                    stateMachine.TargetPos = curWayNode.worldPos;
                    continue;
                }

                stateMachine.TargetPos = curWayNode.worldPos;
            }

            yield return null;
        }
    }

    private IEnumerator CheckFollowMoveCoroutine()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));

        while (true)
        {
            if (!curWayNode.walkable)
            {   
                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                yield return new WaitForSeconds(0.1f);
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                yield return new WaitForSeconds(0.1f);


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
            {
                ++targetIdx;
                updateNodeCallback?.Invoke(transform.position, stateMachine.GetNodeIdx());
                
                if (targetIdx >= arrPath.Length)
                {
                    
                }

                stateMachine.TargetPos = curWayNode.worldPos;
            }

            yield return null;
        }
    }

    private void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            arrPath = _newPath;
            targetIdx = 0;
            curWayNode = arrPath[targetIdx];
        }
    }


    private void StateStop()
    {
        StartCoroutine("CheckStopCoroutine");
    }

    private IEnumerator CheckStopCoroutine()
    {
        while (true)
        {
            yield return null;
        }
    }


    private void StateAttack()
    {
        StartCoroutine("CheckAttackCoroutine");
    }

    private IEnumerator CheckAttackCoroutine()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void StateHold()
    {
        StartCoroutine("CheckHoldCoroutine");
    }

    private IEnumerator CheckHoldCoroutine()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void FinishState()
    {
        StopAllCoroutines();
        stateMachine.FinishState();
    }



    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;

    private EMoveState moveState = EMoveState.NONE;

    private StateMachine stateMachine = null;

    private Vector3 targetPos = Vector3.zero;
    
    private Transform targetTr = null;

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;
    private Vector3 curWayNodePos = Vector3.zero;
    private int targetIdx = 0;

    private NodeUpdateDelegate updateNodeCallback = null;

    private float traceStartRange = 7f;
}