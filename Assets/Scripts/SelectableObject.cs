using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    protected enum EMoveState { NONE = -1, NORMAL, ATTACK, PATROL, CHASE, FOLLOW, FOLLOW_ENEMY }
    public virtual void Init()
    {
        nodeIdx = SelectableObjectManager.InitNode(transform.position);
        stateMachine = GetComponent<StateMachine>();
        statusHp = GetComponent<StatusHp>();
        statusHp.Init();

        if (stateMachine != null)
        {
            stateMachine.Init(GetCurState);
            StateIdle();
        }

        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
    }

    public ESelectableObjectType ObjectType => objectType;

    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public void UpdateCurNode()
    {
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
    }

    public void GetDmg(float _dmg)
    {
        Debug.LogFormat("{0}Get Dmg {1}", transform.name, _dmg);
    }

    public virtual void MoveAttack(Vector3 _targetPos)
    {
        stateMachine.TargetTr = null;
        targetTr = null;

        targetPos = _targetPos;
        curMoveCondition = EMoveState.ATTACK;

        StateMove();
    }

    public virtual void Stop()
    {
        stateMachine.TargetTr = null;
        targetTr = null;
        StateStop();
    }

    #region StateIdleCondition
    protected void StateIdle()
    {
        stateMachine.TargetTr = null;
        targetTr = null;
        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
        stateMachine.ChangeStateEnum(EState.IDLE);
        StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
    }
    #endregion

    protected virtual IEnumerator CheckIsEnemyInChaseStartRangeCoroutine()
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
                            curMoveCondition = EMoveState.CHASE;
                            StateMove();
                            yield break;
                        }
                    }
                    else
                    {
                        ESelectableObjectType targetType = c.GetComponent<SelectableObject>().ObjectType;

                        if (!targetType.Equals(ESelectableObjectType.ENEMY_UNIT))
                        {
                            stateMachine.TargetTr = c.transform;
                            targetTr = c.transform;
                            curMoveCondition = EMoveState.CHASE;
                            StateMove();
                            yield break;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected IEnumerator CheckIsTargetInChaseFinishRangeCoroutine()
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

    protected void CheckIsTargetInAttackRange()
    {
        if (targetTr != null)
        {
            if (isTargetInRangeFromMyPos(targetTr.position, attackRange))
                StateAttack();
        }
    }

    #region StateMoveConditions
    protected virtual void StateMove()
    {
        StopAllCoroutines();
        curWayNode = null;
        stateMachine.SetWaitForNewPath(false);

        switch (curMoveCondition)
        {
            case EMoveState.ATTACK:
                StartCoroutine("CheckNormalMoveCoroutine");
                StartCoroutine("CheckIsEnemyInChaseStartRangeCoroutine");
                break;
            case EMoveState.CHASE:
                if (targetTr == null)
                    StateStop();
                else
                {
                    StartCoroutine("CheckFollowMoveCoroutine");
                    StartCoroutine("CheckIsTargetInChaseFinishRangeCoroutine");
                }
                break;
            default:
                break;
        }
    }

    protected virtual IEnumerator CheckNormalMoveCoroutine()
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
                stateMachine.TargetPos = transform.position;
                curWayNode = null;

                PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                stateMachine.SetWaitForNewPath(true);
                while (curWayNode == null)
                    yield return null;

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
                CheckIsTargetInAttackRange();

                if (targetIdx >= arrPath.Length)
                {
                    ResetState();
                    stateMachine.SetWaitForNewPath(false);
                    yield break;
                }

                UpdateTargetPos();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual IEnumerator CheckFollowMoveCoroutine()
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
                FinishState();
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
                        yield return new WaitForSeconds(0.3f);


                    if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
                    {
                        ++targetIdx;
                        SelectableObjectManager.UpdateNodeWalkable(transform.position, nodeIdx);
                        CheckIsTargetInAttackRange();

                        if (targetIdx >= arrPath.Length)
                        {
                            curWayNode = null;
                            stateMachine.SetWaitForNewPath(true);
                        }
                        else
                        {
                            UpdateTargetPos();
                        }
                    }
                }
                else
                {
                    PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
                    stateMachine.SetWaitForNewPath(true);

                    while (curWayNode == null)
                        yield return null;

                    stateMachine.SetWaitForNewPath(false);
                }
            }
            yield return null;
        }
    }

    protected void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            arrPath = _newPath;
            targetIdx = 0;
            UpdateTargetPos();
        }
        else
        {
            curWayNode = null;
            stateMachine.SetWaitForNewPath(true);
            Invoke("ResearchPath", 3f);
        }
    }

    protected void UpdateTargetPos()
    {
        curWayNode = arrPath[targetIdx];
        stateMachine.TargetPos = curWayNode.worldPos;
    }

    protected void ResearchPath()
    {
        PF_PathRequestManager.RequestPath(transform.position, targetTr.position, OnPathFound);
    }
    #endregion

    #region StateStopCondition
    protected void StateStop()
    {
        StopAllCoroutines();
        stateMachine.ChangeStateEnum(EState.STOP);
        StartCoroutine("CheckStopCoroutine");
    }

    protected IEnumerator CheckStopCoroutine()
    {
        yield return new WaitForSeconds(stopDelay);

        ResetState();
    }
    #endregion

    #region StateAttackCondition
    protected void StateAttack()
    {
        StopAllCoroutines();
        stateMachine.ChangeStateEnum(EState.ATTACK);
        StartCoroutine("AttackCoroutine");
        //StartCoroutine("CheckIsEnemyInAttackRangeCoroutine");
    }

    protected IEnumerator AttackCoroutine()
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

    protected void FinishState()
    {
        StopAllCoroutines();
        stateMachine.FinishStateEnum();
    }

    protected void ResetState()
    {
        StopAllCoroutines();
        stateMachine.ResetStateEnum();
    }

    protected bool isTargetInRangeFromMyPos(Vector3 _targetPos, float _range)
    {
        return Vector3.SqrMagnitude(transform.position - _targetPos) < Mathf.Pow(_range, 2);
    }

    protected Collider[] overlapSphereWithNode(float _range)
    {
        return Physics.OverlapSphere(SelectableObjectManager.listNodeUnderUnit[nodeIdx].worldPos, _range, 1 << LayerMask.NameToLayer("SelectableObject"));
    }

    protected virtual void GetCurState(EState _curStateEnum)
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

    [Header("-Unit Attribute")]
    [SerializeField]
    protected ESelectableObjectType objectType = ESelectableObjectType.NONE;


    [Header("-Unit Control Values")]
    [SerializeField]
    protected float chaseStartRange = 0f;
    [SerializeField]
    protected float chaseFinishRange = 0f;
    [SerializeField]
    protected float attackRange = 0f;
    [SerializeField]
    protected float stopDelay = 2f;
    [SerializeField]
    protected float followOffset = 3f;

    protected EMoveState curMoveCondition = EMoveState.NONE;
    protected StateMachine stateMachine = null;

    protected Vector3 targetPos = Vector3.zero;

    protected Transform targetTr = null;

    protected int targetIdx = 0;
    protected PF_Node[] arrPath = null;
    protected PF_Node curWayNode = null;

    protected StatusHp statusHp = null;

    protected int nodeIdx = 0;
}