using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyObject : SelectableObject
{
    public override void Init()
    {
        nodeIdx = SelectableObjectManager.InitNode(transform.position);
        stateMachine = GetComponent<StateMachine>();
        statusHp = GetComponent<StatusHp>();
        statusHp.Init();

        if (stateMachine != null)
        {
            stateMachine.Init(GetCurState);

            if (objectType.Equals(ESelectableObjectType.TURRET))
                StateHold();
            else
                StateIdle();
        }

        oriAttRange = attackRange;
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
    }

    public Transform TargetBunker => targetBunker;

    public void ResetTargetBunker()
    {
        targetBunker = null;
    }

    public void SetAttackDmg(float _ratio)
    {
        stateMachine.SetAttackDmg(_ratio);
    }

    public void ResetAttackDmg()
    {
        stateMachine.ResetAttackDmg();
    }

    public void SetAttackRange(float _ratio)
    {
        attackRange += oriAttRange * _ratio;
    }

    public void ResetAttackRange()
    {
        attackRange = oriAttRange;
    }

    public void SetLayer(int _layerIdx)
    {
        gameObject.layer = _layerIdx;
    }

    public void ResetLayer()
    {
        gameObject.layer = 1 << LayerMask.GetMask("SelectableObject");
    }

    public void SetMyTr(Transform _myTr)
    {
        stateMachine.SetMyTr(_myTr);
    }

    public void MoveByPos(Vector3 _Pos)
    {
        if (isMovable)
        {
            stateMachine.TargetTr = null;
            targetTr = null;

            isAttack = false;
            targetPos = _Pos;
            curMoveCondition = EMoveState.NORMAL;

            StateMove();
        }
    }

    public override void MoveAttack(Vector3 _targetPos)
    {
        isAttack = true;
        base.MoveAttack(_targetPos);
    }

    public void FollowTarget(Transform _targetTr, bool _isTargetBunker = false)
    {
        if (isMovable)
        {
            if (_targetTr.Equals(transform)) return;

            stateMachine.TargetTr = _targetTr;
            targetTr = _targetTr;

            if (_isTargetBunker)
            {
                targetBunker = _targetTr;
            }

            if (targetTr.CompareTag("EnemyUnit"))
            {
                isAttack = true;
                curMoveCondition = EMoveState.FOLLOW_ENEMY;
            }
            else
            {
                isAttack = false;
                curMoveCondition = EMoveState.FOLLOW;
            }

            StateMove();
        }
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        if (isMovable)
        {
            isAttack = true;
            targetPos = _wayPointTo;
            wayPointStart = transform.position;
            curMoveCondition = EMoveState.PATROL;

            StateMove();
        }
    }

    public override void Stop()
    {
        isAttack = false;
        base.Stop();
    }

    public void Hold()
    {
        if (isMovable)
        {
            stateMachine.TargetTr = null;
            targetTr = null;
            isAttack = true;
            StateHold();
        }
    }

    protected override IEnumerator CheckIsEnemyInChaseStartRangeCoroutine()
    {
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = overlapSphereWithNode(chaseStartRange);

            if (arrCollider.Length > 1)
            {
                foreach (Collider c in arrCollider)
                {
                    if (targetTr != null)
                    {
                        if (c.Equals(targetTr))
                        {
                            prevMoveCondition = curMoveCondition;
                            isAttack = true;
                            curMoveCondition = EMoveState.CHASE;
                            StateMove();
                            yield break;
                        }
                    }
                    else if (c.CompareTag("EnemyUnit"))
                    {
                        stateMachine.TargetTr = c.transform;
                        targetTr = c.transform;
                        isAttack = true;
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

    #region StateMoveConditions
    protected override void StateMove()
    {
        StopAllCoroutines();
        curWayNode = null;
        stateMachine.SetWaitForNewPath(false);
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);

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

    protected override IEnumerator CheckNormalMoveCoroutine()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        stateMachine.SetWaitForNewPath(true);

        while (curWayNode == null)
            yield return null;

        stateMachine.ChangeStateEnum(EState.MOVE);
        stateMachine.SetWaitForNewPath(false);
        while (true)
        {
            if (!curWayNode.walkable)
            {
                curWayNode = null;
                stateMachine.SetWaitForNewPath(true);
                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);

                while (curWayNode == null)
                    yield return null;

                stateMachine.SetWaitForNewPath(false);
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
            {
                stateMachine.SetWaitForNewPath(true);
                yield return new WaitForSeconds(0.5f);
                stateMachine.SetWaitForNewPath(false);
            }


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(stateMachine.TargetPos, 0.1f))
            {
                ++targetIdx;

                SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
                // 목적지에 도착시 
                if (isAttack)
                    CheckIsTargetInAttackRange();

                if (targetIdx >= arrPath.Length)
                {
                    curWayNode = null;
                    ResetState();
                    stateMachine.SetWaitForNewPath(false);
                    yield break;
                }
                UpdateTargetPos();
            }

            yield return null;
        }
    }

    private IEnumerator CheckPatrolMoveCoroutine()
    {
        Vector3 wayPointFrom = wayPointStart;
        Vector3 wayPointTo = targetPos;

        PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
        stateMachine.SetWaitForNewPath(true);
        while (curWayNode == null)
            yield return null;

        stateMachine.ChangeStateEnum(EState.MOVE);
        stateMachine.SetWaitForNewPath(false);

        while (true)
        {
            if (!curWayNode.walkable)
            {
                curWayNode = null;
                stateMachine.SetWaitForNewPath(true);
                PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);

                while (curWayNode == null)
                    yield return null;

                stateMachine.SetWaitForNewPath(false);
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
            {
                yield return new WaitForSeconds(0.3f);
            }


            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
            {
                ++targetIdx;
                SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
                CheckIsTargetInAttackRange();

                if (targetIdx >= arrPath.Length)
                {
                    Vector3 tempWayPoint = wayPointFrom;
                    wayPointFrom = wayPointTo;
                    wayPointTo = tempWayPoint;

                    PF_PathRequestManager.RequestPath(wayPointFrom, wayPointTo, OnPathFound);
                    stateMachine.SetWaitForNewPath(true);

                    while (targetIdx != 0)
                        yield return null;

                    stateMachine.SetWaitForNewPath(false);
                    continue;
                }
                UpdateTargetPos();
            }

            yield return null;
        }
    }

    protected override IEnumerator CheckFollowMoveCoroutine()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
        stateMachine.SetWaitForNewPath(true);

        while (curWayNode == null)
            yield return null;

        float elapsedTime = 0f;

        stateMachine.ChangeStateEnum(EState.MOVE);
        stateMachine.SetWaitForNewPath(false);

        while (true)
        {
            if (targetTr == null)
            {
                ResetState();
                yield break;
            }
            elapsedTime += Time.deltaTime;

            if (elapsedTime > stopDelay)
            {
                elapsedTime = 0f;
                if (Vector3.SqrMagnitude(transform.position - targetTr.position) > Mathf.Pow(followOffset, 2f))
                {
                    PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                    stateMachine.SetWaitForNewPath(true);
                    curWayNode = null;

                    while (curWayNode == null)
                        yield return null;

                    stateMachine.SetWaitForNewPath(false);
                }
            }
            else
            {
                if (curWayNode != null)
                {
                    if (!curWayNode.walkable)
                    {
                        curWayNode = null;
                        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                        stateMachine.SetWaitForNewPath(true);

                        while (curWayNode == null)
                            yield return null;

                        stateMachine.SetWaitForNewPath(false);
                    }

                    if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                        yield return new WaitForSeconds(0.1f);

                    if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
                    {
                        ++targetIdx;
                        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
                        if (isAttack)
                            CheckIsTargetInAttackRange();

                        if (targetIdx >= arrPath.Length)
                        {
                            curWayNode = null;
                            stateMachine.SetWaitForNewPath(true);
                        }
                        else
                            UpdateTargetPos();
                    }
                }
                else
                    stateMachine.SetWaitForNewPath(true);
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
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
        StartCoroutine("CheckHoldCoroutine");
    }

    private IEnumerator CheckHoldCoroutine()
    {
        yield return null;
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = overlapSphereWithNode(attackRange);

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

    protected override void GetCurState(EState _curStateEnum)
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
            case EState.ATTACK:
                StateAttack();
                break;
            default:
                break;
        }
    }


    [Header("-Friendly Unit Attribute")]
    [SerializeField]
    private bool isMovable = false;

    private Vector3 wayPointStart = Vector3.zero;
    private EMoveState prevMoveCondition = EMoveState.NONE;

    private float oriAttRange = 0f;
    private bool isAttack = false;

    private Transform targetBunker = null;
}