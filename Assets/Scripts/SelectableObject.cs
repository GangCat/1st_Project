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
            stateMachine.Init(_nodeIdx, _updateNodeCallback, GetCurState, GetCurState);
    }

    public void AttackDmg(int _dmg)
    {
        Debug.LogFormat("Hit Dmg {0}", _dmg);
    }


    public void FollowTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
        curMoveCondition = EMoveState.FOLLOW;

        if (isControllable)
            StateMove();
    }

    public void FollowEnemy(Transform _targetTr)
    {
        targetTr = _targetTr;
        curMoveCondition = EMoveState.FOLLOW_ENEMY;

        if (isControllable)
            StateMove();
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        //targetPos = _targetPos;
        targetPos = _targetPos;
        curMoveCondition = EMoveState.NORMAL;

        if (isControllable)
            StateMove();
    }

    public void MoveAttack(Vector3 _targetPos)
    {
        targetPos = _targetPos;
        curMoveCondition = EMoveState.ATTACK;

        if (isControllable)
            StateMove();
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        targetPos = _wayPointTo;
        curMoveCondition = EMoveState.PATROL;

        if (isControllable)
            StateMove();
    }

    public void Stop()
    {
        if (isControllable)
            //stateMachine.ChangeState(stateMachine.GetState((int)EState.STOP));
            stateMachine.ChangeStateEnum(EState.STOP);
    }

    public void Hold()
    {
        if (isControllable)
            StateHold();
    }


    #region StateIdleCondition
    private void StateIdle()
    {
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.IDLE));
        stateMachine.ChangeStateEnum(EState.IDLE);
        StartCoroutine("CheckEnemyInChaseStartRangeCoroutine");
    }

    private IEnumerator CheckEnemyInChaseStartRangeCoroutine()
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
    #endregion

    #region StateMoveConditions
    private void StateMove()
    {
        StopAllCoroutines();

        switch (curMoveCondition)
        {
            case EMoveState.NORMAL:
                StartCoroutine("CheckNormalMoveCoroutine");
                break;
            case EMoveState.ATTACK:
                StartCoroutine("CheckNormalMoveCoroutine");
                StartCoroutine("CheckEnemyInChaseStartRangeCoroutine");
                break;
            case EMoveState.PATROL:
                StartCoroutine("CheckPatrolMoveCoroutine");
                StartCoroutine("CheckEnemyInChaseStartRangeCoroutine");
                break;
            case EMoveState.FOLLOW:
                StartCoroutine("CheckFollowMoveCoroutine");
                break;
            case EMoveState.CHASE:
                StartCoroutine("CheckFollowMoveCoroutine");
                break;
            case EMoveState.FOLLOW_ENEMY:
                StartCoroutine("CheckFollowMoveCoroutine");
                StartCoroutine("CheckEnemyInChaseStartRangeCoroutine");
                break;
            default:
                break;
        }
    }

    private IEnumerator CheckNormalMoveCoroutine()
    {
        curWayNode = null;
        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));
        stateMachine.ChangeStateEnum(EState.MOVE);

        while (true)
        {
            if (!curWayNode.walkable)
            {
                curWayNode = null;
                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                while (curWayNode == null)
                    yield return null;

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

        stateMachine.TargetPos = curWayNode.worldPos;
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));
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
                curWayNode = arrPath[targetIdx];
                stateMachine.TargetPos = curWayNode.worldPos;
            }

            yield return null;
        }
    }

    private IEnumerator CheckFollowMoveCoroutine()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
        yield return null;

        while (curWayNode == null)
            yield return null;

        float elapsedTime = 0f;

        stateMachine.TargetPos = curWayNode.worldPos;
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.MOVE));
        stateMachine.ChangeStateEnum(EState.MOVE);

        while (true)
        {
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


                // 노드에 도착할 때마다 새로운 노드로 이동 갱신
                if (Vector3.SqrMagnitude(transform.position - curWayNode.worldPos) < 0.01f)
                {
                    ++targetIdx;
                    updateNodeCallback?.Invoke(transform.position, stateMachine.GetNodeIdx());

                    if (targetIdx >= arrPath.Length)
                    {
                        curWayNode = null;
                        continue;
                    }
                    curWayNode = arrPath[targetIdx];
                    stateMachine.TargetPos = curWayNode.worldPos;
                }
            }

            if (elapsedTime > resetPathDelay)
            {
                PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                yield return new WaitForSeconds(0.3f);
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
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.STOP));
        stateMachine.ChangeStateEnum(EState.STOP);
        StartCoroutine("CheckStopCoroutine");
    }

    private IEnumerator CheckStopCoroutine()
    {
        yield return new WaitForSeconds(stopDelay);

        //stateMachine.ResetState();
        stateMachine.ResetStateEnum();
    }
    #endregion

    #region StateAttackCondition
    private void StateAttack()
    {
        StopAllCoroutines();
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.ATTACK));
        stateMachine.ChangeStateEnum(EState.ATTACK);
        StartCoroutine("CheckAttackCoroutine");
    }

    private IEnumerator CheckAttackCoroutine()
    {
        while (true)
        {
            if (stateMachine.TargetTr == null)
            {
                // 추격, 정찰, 대기, 홀드 등 뭐든간에 이전으로 돌아감.
                FinishState();
            }


            if (Vector3.SqrMagnitude(stateMachine.TargetTr.position - transform.position) > Mathf.Pow(stateMachine.AttackRange, 2))
            {
                stateMachine.TargetTr = null;

                Collider[] arrCollider = null;
                arrCollider = Physics.OverlapSphere(transform.position, traceStartRange);

                if (arrCollider.Length > 0)
                {
                    foreach (Collider c in arrCollider)
                    {
                        if (c.CompareTag("EnemyUnit"))
                        {
                            stateMachine.SetTargetTr(c.transform);
                            break;
                        }
                    }
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
        //stateMachine.ChangeState(stateMachine.GetState((int)EState.HOLD));
        stateMachine.ChangeStateEnum(EState.HOLD);
        StartCoroutine("CheckHoldCoroutine");
    }

    private IEnumerator CheckHoldCoroutine()
    {
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(transform.position, stateMachine.AttackRange);

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
    #endregion


    private void FinishState()
    {
        StopAllCoroutines();
        //stateMachine.FinishState();
        stateMachine.FinishStateEnum();
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

    private void GetCurState(IState _curState)
    {
        

        switch (_curState)
        {

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


    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;

    private EMoveState curMoveCondition = EMoveState.NONE;

    private StateMachine stateMachine = null;

    private Vector3 targetPos = Vector3.zero;

    private Transform targetTr = null;

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;
    private int targetIdx = 0;

    private NodeUpdateDelegate updateNodeCallback = null;

    private float traceStartRange = 7f;
    private float resetPathDelay = 2f;
    private float stopDelay = 2f;

    private EState curState = EState.NONE;

}