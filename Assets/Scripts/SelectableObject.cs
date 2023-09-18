using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private enum EMoveState { NONE = -1, NORMAL, ATTACK, PATROL, CHASE, FOLLOW, FOLLOW_ENEMY }
    public void Init()
    {
        nodeIdx = SelectableObjectManager.InitNode(transform.position);
        stateMachine = GetComponent<StateMachine>();
        statusHp = GetComponent<StatusHp>();
        statusHp.Init();

        if (stateMachine != null)
        {
            stateMachine.Init(GetCurState);

            if (objectType.Equals(ESelectableObjectType.UNIT) || 
                objectType.Equals(ESelectableObjectType.UNIT_HERO) ||
                objectType.Equals(ESelectableObjectType.ENEMY_UNIT))
                StateIdle();
            else if (objectType.Equals(ESelectableObjectType.TURRET))
                StateHold();
        }

        oriAttRange = attackRange;
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
    }

    public ESelectableObjectType ObjectType => objectType;

    public Vector3 Position 
    {
        get => transform.position;
        set => transform.position = value;
    }

    public bool IsBunker
    {
        get => isBunker;
        set => isBunker = value;
    }

    public int NodeIdx => nodeIdx;

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

    public void UpdateCurNode()
    {
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
    }

    public void GetDmg(float _dmg)
    {
        //Debug.LogFormat("Hit Dmg {0}", _dmg);
    }

    public void SetMyTr(Transform _myTr)
    {
        stateMachine.SetMyTr(_myTr);
    }


    public void FollowTarget(Transform _targetTr, bool _isBunker = false)
    {
        if (isMovable)
        {
            if (_targetTr.Equals(transform)) return;

            stateMachine.TargetTr = _targetTr;
            targetTr = _targetTr;
            isBunker = _isBunker;

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

    public void MoveAttack(Vector3 _targetPos)
    {
        if (isMovable)
        {
            stateMachine.TargetTr = null;
            targetTr = null;
             
            isAttack = true;
            targetPos = _targetPos;
            curMoveCondition = EMoveState.ATTACK;

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

    public void Stop()
    {
        if (isMovable)
        {
            stateMachine.TargetTr = null;
            targetTr = null;
            isAttack = false;
            StateStop();  
        }
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


    #region StateIdleCondition
    private void StateIdle()
    {
        stateMachine.TargetTr = null;
        targetTr = null;
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
        stateMachine.ChangeStateEnum(EState.IDLE);
        StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
    }
    #endregion

    private IEnumerator CheckIsEnemyInChaseStartRangeCoroutine()
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

    private void CheckIsTargetInAttackRange()
    {
        if (targetTr != null)
        {
            if (isTargetInRangeFromMyPos(targetTr.position, attackRange))
                StateAttack();
        }
    }

    #region StateMoveConditions
    private void StateMove()
    {
        StopAllCoroutines();
        curWayNode = null;
        stateMachine.SetWaitForNewPath(false);

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
                    //StartCoroutine("CheckIsTargetInAttackRangeCoroutine");
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
        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        stateMachine.SetWaitForNewPath(true);

        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeStateEnum(EState.MOVE);
        stateMachine.SetWaitForNewPath(false);
        while (true)
        {
            if (!curWayNode.walkable)
            {
                stateMachine.TargetPos = transform.position;
                curWayNode = null;

                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                stateMachine.SetWaitForNewPath(true);
                while (curWayNode == null)
                    yield return null;

                stateMachine.TargetPos = curWayNode.worldPos;
                stateMachine.SetWaitForNewPath(false);
            }

            if (Physics.Linecast(transform.position, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
                yield return new WaitForSeconds(0.3f);


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
                    //curWayNode = null;
                    ResetState();
                    stateMachine.SetWaitForNewPath(false);
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
        stateMachine.SetWaitForNewPath(true);
        while (curWayNode == null)
            yield return null;

        stateMachine.TargetPos = curWayNode.worldPos;
        stateMachine.ChangeStateEnum(EState.MOVE);
        stateMachine.SetWaitForNewPath(false);

        while (true)
        {
            if (!curWayNode.walkable)
            {
                curWayNode = null;
                PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
                stateMachine.SetWaitForNewPath(true);
                while (curWayNode == null)
                    yield return null;

                stateMachine.TargetPos = curWayNode.worldPos;
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

                    stateMachine.TargetPos = curWayNode.worldPos;
                    stateMachine.SetWaitForNewPath(false);
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
        stateMachine.SetWaitForNewPath(true);

        while (curWayNode == null)
            yield return null;

        float elapsedTime = 0f;

        stateMachine.TargetPos = curWayNode.worldPos;
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
                    curWayNode = null;
                    PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                    stateMachine.SetWaitForNewPath(true);
                    while (curWayNode == null)
                        yield return null;

                    stateMachine.TargetPos = curWayNode.worldPos;
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

                        stateMachine.TargetPos = curWayNode.worldPos;
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
                            if(isBunker)
                            {
                                // 벙커에 들어가는 함수호출
                                Debug.Log("Bunker");
                            }

                            curWayNode = null;
                            stateMachine.SetWaitForNewPath(true);
                        }
                        else
                        {
                            curWayNode = arrPath[targetIdx];
                            stateMachine.TargetPos = curWayNode.worldPos;
                        }
                    }
                }
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

    private Collider[] overlapSphereWithNode(float _range)
    {
        return Physics.OverlapSphere(SelectableObjectManager.listNodeUnderUnit[nodeIdx].worldPos, _range, 1 << LayerMask.NameToLayer("SelectableObject"));
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
    private float stopDelay = 2f;
    [SerializeField]
    private float followOffset = 3f;

    private EMoveState curMoveCondition = EMoveState.NONE;
    private EMoveState prevMoveCondition = EMoveState.NONE;
    private StateMachine stateMachine = null;

    private Vector3 targetPos = Vector3.zero;
    private Vector3 wayPointStart = Vector3.zero;

    private Transform targetTr = null;

    private int targetIdx = 0;
    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;

    private StatusHp statusHp = null;

    private int nodeIdx = 0;
    private float oriAttRange = 0f;
    private bool isAttack = false;
    private bool isBunker = false;
}