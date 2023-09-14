using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public enum EMoveState { NONE = -1, NORMAL, ATTACK, PATROL, CHASE, FOLLOW, FOLLOW_ENEMY }
    public ESelectableObjectType ObjectType => objectType;
    public Vector3 GetPos => transform.position;

    public void Init(int _nodeIdx = -1, NodeUpdateDelegate _updateNodeCallback = null)
    {
        nodeIdx = _nodeIdx;
        updateNodeCallback = _updateNodeCallback;
        stateMachine = GetComponent<StateMachine>();

        if (stateMachine != null)
        {
            stateMachine.Init(GetCurState);

            if (objectType.Equals(ESelectableObjectType.UNIT))
                StateIdle();
            else if (objectType.Equals(ESelectableObjectType.TURRET))
                StateHold();
        }
    }

    public void AttackDmg(int _dmg)
    {
        //Debug.LogFormat("Hit Dmg {0}", _dmg);
    }

    public void SetMyTr(Transform _myTr)
    {
        stateMachine.SetMyTr(_myTr);
    }


    public void FollowTarget(Transform _targetTr)
    {
        stateMachine.TargetTr = _targetTr;
        targetTr = _targetTr;

        if(targetTr.CompareTag("EnemyUnit"))
            curMoveCondition = EMoveState.FOLLOW_ENEMY;
        else
            curMoveCondition = EMoveState.FOLLOW;

        if (isMovable)
            StateMove();
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        curMoveCondition = EMoveState.NORMAL;

        if (isMovable)
            StateMove();
    }

    public void MoveAttack(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        curMoveCondition = EMoveState.ATTACK;

        if (isMovable)
            StateMove();
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        targetPos = _wayPointTo;
        wayPointStart = transform.position;
        curMoveCondition = EMoveState.PATROL;

        if (isMovable)
            StateMove();
    }

    public void Stop()
    {
        if (isMovable)
            StateStop();
    }

    public void Hold()
    {
        if (isMovable)
            StateHold();
    }


    #region StateIdleCondition
    private void StateIdle()
    {
        stateMachine.TargetTr = null;
        updateNodeCallback?.Invoke(transform.position, nodeIdx);
        stateMachine.ChangeStateEnum(EState.IDLE);
        StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
    }
    #endregion

    private IEnumerator CheckIsEnemyInChaseStartRangeCoroutine()
    {
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(transform.position, chaseStartRange, 1 << LayerMask.NameToLayer("SelectableObject"));

            if (arrCollider.Length > 1)
            {
                foreach (Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        stateMachine.TargetTr = c.transform;
                        targetTr = c.transform;
                        prevMoveCondition = curMoveCondition;
                        curMoveCondition = EMoveState.CHASE;
                        StateMove();
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator CheckIsTargetInChaseFinishRangeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (targetTr != null)
            {
                if (!isTargetInRangeFromMyPos(targetTr.position, chaseFinishRange))
                {
                    stateMachine.TargetTr = null;
                    targetTr = null;
                    FinishState();
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator CheckIsTargetInAttackRangeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (targetTr != null)
            {
                if (isTargetInRangeFromMyPos(targetTr.position, attackRange))
                {
                    StateAttack();
                    yield break;
                }
            }
            yield return null;
        }
    }

    #region StateMoveConditions
    private void StateMove()
    {
        StopAllCoroutines();
        curWayNode = null;

        switch (curMoveCondition)
        {
            case EMoveState.NORMAL:
                StartCoroutine("CheckNormalMoveCoroutine");
                break;
            case EMoveState.ATTACK:
                StartCoroutine("CheckNormalMoveCoroutine");
                StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
                break;
            case EMoveState.PATROL:
                StartCoroutine("CheckPatrolMoveCoroutine");
                StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
                break;
            case EMoveState.CHASE:
                if (targetTr == null)
                {
                    curMoveCondition = prevMoveCondition;
                    prevMoveCondition = EMoveState.NONE;
                    StateMove();
                    break;
                }
                else
                {
                    StartCoroutine("CheckFollowMoveCoroutine");
                    StartCoroutine("CheckIsTargetInChaseFinishRangeCoroutine");
                    StartCoroutine("CheckIsTargetInAttackRangeCoroutine");
                }
                break;
            case EMoveState.FOLLOW:
                if (targetTr == null)
                    ResetState();
                else
                    StartCoroutine("CheckFollowMoveCoroutine");

                break;
            case EMoveState.FOLLOW_ENEMY:
                if (targetTr == null)
                    ResetState();
                else
                {
                    StartCoroutine("CheckFollowMoveCoroutine");
                    StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator CheckNormalMoveCoroutine()
    {
        stateMachine.TargetTr = null;

        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeStateEnum(EState.MOVE);

        while (true)
        {
            if (!curWayNode.walkable)
            {
                stateMachine.TargetPos = transform.position;
                curWayNode = null;

                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);

                while (curWayNode == null)
                    yield return null;

                stateMachine.TargetPos = curWayNode.worldPos;
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                yield return new WaitForSeconds(0.3f);


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(stateMachine.TargetPos, 0.1f))
            {
                ++targetIdx;

                updateNodeCallback?.Invoke(transform.position, nodeIdx);
                // 목적지에 도착시 
                if (targetIdx >= arrPath.Length)
                {
                    //curWayNode = null;
                    ResetState();
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
        Vector3 wayPointFrom = wayPointStart;
        Vector3 wayPointTo = targetPos;

        PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeStateEnum(EState.MOVE);

        while (true)
        {
            if (!curWayNode.walkable)
            {
                curWayNode = null;
                PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
                while (curWayNode == null)
                    yield return null;

                stateMachine.TargetPos = curWayNode.worldPos;
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
            {
                yield return new WaitForSeconds(0.3f);
            }


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
            {
                ++targetIdx;
                updateNodeCallback?.Invoke(transform.position, nodeIdx);
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
                curWayNode = arrPath[targetIdx];
                stateMachine.TargetPos = curWayNode.worldPos;
            }

            yield return null;
        }
    }

    private IEnumerator CheckFollowMoveCoroutine()
    {
        targetTr = stateMachine.TargetTr;
        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        float elapsedTime = 0f;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeStateEnum(EState.MOVE);

        while (true)
        {
            if (targetTr == null)
            {
                ResetState();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            if (curWayNode != null)
            {
                if (!curWayNode.walkable)
                {
                    curWayNode = null;
                    PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);

                    while (curWayNode == null)
                        yield return null;

                    stateMachine.TargetPos = curWayNode.worldPos;
                }

                if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                    yield return new WaitForSeconds(0.1f);


                if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
                {
                    ++targetIdx;
                    updateNodeCallback?.Invoke(transform.position, nodeIdx);

                    if (targetIdx >= arrPath.Length)
                    {
                        curWayNode = null;
                        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                        while (curWayNode == null)
                            yield return null;

                        stateMachine.TargetPos = curWayNode.worldPos;
                    }
                    else
                    {
                        curWayNode = arrPath[targetIdx];
                        stateMachine.TargetPos = curWayNode.worldPos;
                    }
                }
            }

            // 일정 주기로 경로 재탐색
            if (elapsedTime > resetPathDelay)
            {
                elapsedTime = 0f;

                curWayNode = null;
                PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                while (curWayNode == null)
                    yield return null;

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
    #endregion

    #region StateStopCondition
    private void StateStop()
    {
        StopAllCoroutines();
        stateMachine.ChangeStateEnum(EState.STOP);
        StartCoroutine("CheckStopCoroutine");
        
    }

    private IEnumerator CheckStopCoroutine()
    {
        yield return new WaitForSeconds(stopDelay);

        ResetState();
    }
    #endregion

    #region StateAttackCondition
    private void StateAttack()
    {
        StopAllCoroutines();
        stateMachine.ChangeStateEnum(EState.ATTACK);
        StartCoroutine("AttackCoroutine");
        //StartCoroutine("CheckIsEnemyInAttackRangeCoroutine");
    }

    private IEnumerator AttackCoroutine()
    {
        yield return null;
        targetTr = stateMachine.TargetTr;
        while (true)
        {
            // 적이 없음
            if (targetTr == null)
            {
                // 추격, 정찰, 대기, 홀드 등 뭐든간에 이전으로 돌아감.
                FinishState();
                yield break;
            }
            else // 적이 공격 사거리를 벗어남
            {
                if (!isTargetInRangeFromMyPos(targetTr.position, attackRange))
                {
                    FinishState();
                    yield break;
                }
            }

            yield return null;
        }
    }
    #endregion

    #region StateHoldCondition
    private void StateHold()
    {
        StopAllCoroutines();
        stateMachine.ChangeStateEnum(EState.HOLD);
        StartCoroutine("CheckHoldCoroutine");
    }

    private IEnumerator CheckHoldCoroutine()
    {
        yield return null;
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(transform.position, attackRange, 1 << LayerMask.NameToLayer("SelectableObject"));

            if (arrCollider.Length > 0)
            {
                foreach (Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        stateMachine.TargetTr = c.transform;
                        StateAttack();
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    private void FinishState()
    {
        StopAllCoroutines();
        stateMachine.FinishStateEnum();
    }

    private void ResetState()
    {
        StopAllCoroutines();
        stateMachine.ResetStateEnum();
    }

    private bool isTargetInRangeFromMyPos(Vector3 _targetPos, float _range)
    {
        return Vector3.SqrMagnitude(transform.position - _targetPos) < Mathf.Pow(_range, 2);
    }


    public void OnDrawGizmos()
    {
        if (arrPath != null)
        {
            for (int i = targetIdx; i < arrPath.Length; ++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(arrPath[i].worldPos, Vector3.one * 0.4f);

                if (i == targetIdx)
                    Gizmos.DrawLine(transform.position, arrPath[i].worldPos);
                else
                    Gizmos.DrawLine(arrPath[i - 1].worldPos, arrPath[i].worldPos);
            }
        }
    }

    private void GetCurState(EState _curStateEnum)
    {
        switch (_curStateEnum)
        {
            case EState.IDLE:
                StateIdle();
                break;
            case EState.MOVE:
                StateMove();
                break;
            case EState.STOP:
                StateStop();
                break;
            case EState.HOLD:
                StateHold();
                break;
            case EState.ATTACK:
                StateAttack();
                break;
            default:
                break;
        }
    }

    [Header("-Unit Attribute")]
    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isMovable = false;

    [Header("-Unit Control Values")]
    [SerializeField]
    private float chaseStartRange = 0f;
    [SerializeField]
    private float chaseFinishRange = 0f;
    [SerializeField]
    private float attackRange = 0f;
    [SerializeField]
    private float resetPathDelay = 0.5f;
    [SerializeField]
    private float stopDelay = 2f;

    private EMoveState curMoveCondition = EMoveState.NONE;
    private EMoveState prevMoveCondition = EMoveState.NONE;
    private StateMachine stateMachine = null;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 wayPointStart = Vector3.zero;

    private Transform targetTr = null;

    private int targetIdx = 0;
    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;

    private NodeUpdateDelegate updateNodeCallback = null;

    private int nodeIdx = 0;
    private int testIdx = 0;
}