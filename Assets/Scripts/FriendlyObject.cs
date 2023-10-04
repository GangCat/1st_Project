using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyObject : SelectableObject, ISubscriber
{
    public override void Init()
    {
        nodeIdx = SelectableObjectManager.InitNodeFriendly(transform.position);
        stateMachine = GetComponent<StateMachine>();
        statusHp = GetComponent<StatusHp>();
        statusHp.Init();

        if (stateMachine != null)
        {
            oriAttRange = attackRange;
            stateMachine.Init(GetCurState);
            ResetStateStack();
            if (objectType.Equals(EObjectType.TURRET))
            {
                StructureCollider[] arrCollider = GetComponentsInChildren<StructureCollider>();
                for (int i = 0; i < arrCollider.Length; ++i)
                    arrCollider[i].Init(GetDmg, objectType);
            }
            else if(unitType.Equals(EUnitType.RANGED))
            {
                stateMachine.UpgradeAttDmg((SelectableObjectManager.LevelRangedUnitDmgUpgrade - 1) * 2);
                statusHp.UpgradeHp((SelectableObjectManager.LevelRangedUnitHpUpgrade - 1) * 10);
            }
            else if (unitType.Equals(EUnitType.MELEE))
            {
                stateMachine.UpgradeAttDmg((SelectableObjectManager.LevelMeleeUnitDmgUpgrade - 1) * 2);
                statusHp.UpgradeHp((SelectableObjectManager.LevelMeleeUnitHpUpgrade - 1) * 10);
            }
            StateIdle();
            UpdateCurNode();
        }
        else
        {
            StructureCollider[] arrCollider = GetComponentsInChildren<StructureCollider>();
            for (int i = 0; i < arrCollider.Length; ++i)
                arrCollider[i].Init(GetDmg, objectType);
        }
    }

    public EUnitType GetUnitType => unitType;
    public float GetCurHpPercent => statusHp.GetCurHpPercent;
    public void Select(int _listIdx)
    {
        isSelect = true;
        listIdx = _listIdx;
    }

    public void unSelect()
    {
        isSelect = false;
        listIdx = -1;
    }

    public void Init(int _barrackIdx)
    {
        barrackIdx = _barrackIdx;
        Subscribe();
    }

    public override void UpdateCurNode()
    {
        SelectableObjectManager.UpdateFriendlyNodeWalkable(transform.position, nodeIdx);
    }

    public Transform TargetBunker => targetBunker;

    public override void GetDmg(float _dmg) 
    {
        if (statusHp.DecreaseHpAndCheckIsDead(_dmg))
        {
            StopAllCoroutines();
            SelectableObjectManager.ResetFriendlyNodeWalkable(transform.position, nodeIdx);

            if (objectType.Equals(EObjectType.UNIT))
            {
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.DEAD, gameObject, unitType, barrackIdx);
                Broker.UnSubscribe(this, EPublisherType.SELECTABLE_MANAGER);
            }
            else if (objectType.Equals(EObjectType.HBEAM))
            {
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.DESTROY_HBEAM, gameObject, unitType, barrackIdx);
            }
            else if (objectType.Equals(EObjectType.BUNKER))
            {
                GetComponent<StructureBunker>().OutAllUnit();
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.DESTROY, gameObject);
            }
            else
                ArrayFriendlyObjectCommand.Use(EFriendlyObjectCommand.DESTROY, gameObject);
        }
        else if (isSelect)
        {
            SelectableObjectManager.UpdateHp(listIdx);
        }
    }

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
        gameObject.layer = LayerMask.NameToLayer("SelectableObject");
    }

    public void SetMyTr(Transform _myTr)
    {
        stateMachine.SetMyTr(_myTr);
    }

    public void UpgradeAttackDmg(float _increaseDmg)
    {
        stateMachine.UpgradeAttDmg(_increaseDmg);
    }

    public void UpgradeAttackRange(float _increaseRange)
    {
        oriAttRange += _increaseRange;
        attackRange += _increaseRange;
    }

    public void MoveByPos(Vector3 _Pos)
    {
        isAttack = false;
        targetPos = _Pos;
        curMoveCondition = EMoveState.NORMAL;
        ResetStateStack();
        PushState();
        StateMove();
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

            ResetStateStack();

            stateMachine.TargetTr = _targetTr;
            targetTr = _targetTr;

            if (_isTargetBunker)
                targetBunker = _targetTr;

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
            PushState();
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
            ResetStateStack();
            PushState();
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
            isAttack = true;
            ResetStateStack();
            PushState();
            StateHold();
        }
    }

    protected override IEnumerator CheckIsEnemyInChaseStartRangeCoroutine()
    {
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = overlapSphere(chaseStartRange);

            if (arrCollider.Length > 1)
            {
                foreach (Collider c in arrCollider)
                {
                    EObjectType targetType = c.GetComponent<IGetObjectType>().GetObjectType();

                    if (targetTr != null)
                    {
                        if (c.transform.Equals(targetTr))
                        {
                            prevMoveCondition = curMoveCondition;
                            curMoveCondition = EMoveState.CHASE;
                            PushState();
                            StateMove();
                            yield break;
                        }
                        continue;
                    }

                    if (targetType.Equals(EObjectType.ENEMY_UNIT))
                    {
                        stateMachine.TargetTr = c.transform;
                        targetTr = c.transform;
                        isAttack = true;
                        prevMoveCondition = curMoveCondition;
                        curMoveCondition = EMoveState.CHASE;
                        PushState();
                        StateMove();
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    #region StateMoveConditions
    protected override void StateMove()
    {
        StopAllCoroutines();
        curWayNode = null;
        stateMachine.SetWaitForNewPath(true);
        UpdateCurNode();
        ChangeState(EState.MOVE);

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
                if (targetTr == null || targetTr.gameObject.activeSelf == false)
                {
                    if (prevMoveCondition != EMoveState.NONE)
                    {
                        curMoveCondition = prevMoveCondition;
                        prevMoveCondition = EMoveState.NONE;
                    }
                        FinishState();
                }
                else
                {
                    StartCoroutine("CheckFollowMoveCoroutine");
                    StartCoroutine("CheckIsTargetInChaseFinishRangeCoroutine");
                    //StartCoroutine("CheckIsTargetInAttackRangeCoroutine");
                }
                break;
            case EMoveState.FOLLOW:
                if (targetTr == null || targetTr.gameObject.activeSelf == false)
                    FinishState();
                else
                    StartCoroutine("CheckFollowMoveCoroutine");
                break;
            case EMoveState.FOLLOW_ENEMY:
                if (targetTr == null || targetTr.gameObject.activeSelf == false)
                    FinishState();
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

        while (curWayNode == null)
            yield return null;

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

            if (IsObjectBlocked())
            {
                stateMachine.SetWaitForNewPath(true);
                yield return new WaitForSeconds(0.5f);
                stateMachine.SetWaitForNewPath(false);
            }

            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(stateMachine.TargetPos, 0.1f))
            {
                ++targetIdx;
                UpdateCurNode();
                // 목적지에 도착시 
                if (isAttack)
                    CheckIsTargetInAttackRange();

                if (targetIdx >= arrPath.Length)
                {
                    curWayNode = null;

                    if (Vector3.SqrMagnitude(targetPos - transform.position) > Mathf.Pow(3f, 2f))
                    {
                        PF_PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
                        stateMachine.SetWaitForNewPath(true);
                        while (curWayNode == null)
                            yield return null;

                        stateMachine.SetWaitForNewPath(false);
                        continue;
                    }

                    FinishState();
                    stateMachine.SetWaitForNewPath(false);
                    yield break;
                }
                UpdateTargetPos();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckPatrolMoveCoroutine()
    {
        Vector3 wayPointFrom = wayPointStart;
        Vector3 wayPointTo = targetPos;

        PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
        while (curWayNode == null)
            yield return null;

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

            if (IsObjectBlocked())
            {
                stateMachine.SetWaitForNewPath(true);
                yield return new WaitForSeconds(0.5f);
                stateMachine.SetWaitForNewPath(false);
            }

            // 노드에 도착할 때마다 새로운 노드로 이동 갱신
            if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
            {
                ++targetIdx;
                UpdateCurNode();
                CheckIsTargetInAttackRange();

                if (targetIdx >= arrPath.Length)
                {
                    if (Vector3.SqrMagnitude(transform.position - wayPointTo) > Mathf.Pow(2f, 2f))
                    {
                        PF_PathRequestManager.RequestPath(transform.position, wayPointTo, OnPathFound);
                        stateMachine.SetWaitForNewPath(true);
                        while (curWayNode == null)
                            yield return null;

                        stateMachine.SetWaitForNewPath(false);
                        continue;
                    }

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

        while (curWayNode == null)
            yield return null;

        float elapsedTime = 0f;
        stateMachine.SetWaitForNewPath(false);

        while (true)
        {
            if (targetTr == null)
            {
                stateMachine.TargetTr = null;
                //PushState();
                FinishState();
                yield break;
            }
            else if (targetTr.gameObject.activeSelf.Equals(false))
            {
                targetTr = null;
                stateMachine.TargetTr = null;
                //PushState();
                FinishState();
                yield break;
            }
            elapsedTime += Time.deltaTime;

            if (elapsedTime > stopDelay)
            {
                elapsedTime = 0f;
                if (!isTargetInRangeFromMyPos(targetTr.position, followOffset))
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

                    if (IsObjectBlocked())
                    {
                        stateMachine.SetWaitForNewPath(true);
                        yield return new WaitForSeconds(0.5f);
                        stateMachine.SetWaitForNewPath(false);
                    }

                    if (isTargetInRangeFromMyPos(curWayNode.worldPos, 0.1f))
                    {
                        ++targetIdx;
                        UpdateCurNode();
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
        ChangeState(EState.HOLD);
        UpdateCurNode();
        StartCoroutine("CheckHoldCoroutine");
    }

    private IEnumerator CheckHoldCoroutine()
    {
        yield return null;
        while (true)
        {
            Collider[] arrCollider = null;
            arrCollider = overlapSphere(attackRange);

            if (arrCollider.Length > 0)
            {
                foreach (Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        targetTr = c.transform;
                        stateMachine.TargetTr = targetTr;
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
            case EState.HOLD:
                StateHold();
                break;
            default:
                break;
        }
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.SELECTABLE_MANAGER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        switch (_message)
        {
            case EMessageType.UPGRADE_RANGED_HP:
                if (unitType.Equals(EUnitType.RANGED))
                {
                    UpgradeHp(SelectableObjectManager.LevelRangedUnitHpUpgrade);
                    Debug.Log("RangedUpgradeHp");
                }
                break;
            case EMessageType.UPGRADE_RANGED_DMG:
                if (unitType.Equals(EUnitType.RANGED))
                {
                    UpgradeDmg(SelectableObjectManager.LevelRangedUnitDmgUpgrade);
                    Debug.Log("RangedUpgradeDmg");
                }
                break;
            case EMessageType.UPGRADE_MELEE_HP:
                if (unitType.Equals(EUnitType.MELEE))
                {
                    UpgradeHp(SelectableObjectManager.LevelMeleeUnitHpUpgrade);
                    statusHp.UpgradeHp((SelectableObjectManager.LevelMeleeUnitHpUpgrade - 1) * 10);
                    Debug.Log("MeleeUpgradeHp");
                }
                break;
            case EMessageType.UPGRADE_MELEE_DMG:
                if (unitType.Equals(EUnitType.MELEE))
                {
                    UpgradeDmg(SelectableObjectManager.LevelMeleeUnitDmgUpgrade);
                    Debug.Log("MeleeUpgradeDmg");
                }
                break;
            default:
                break;
        }
    }

    private void UpgradeHp(int _level)
    {
        statusHp.UpgradeHp((_level - 1) * 10);
    }

    private void UpgradeDmg(int _level)
    {
        stateMachine.UpgradeAttDmg((_level - 1) * 2);
    }

    [Header("-Friendly Unit Attribute")]
    [SerializeField]
    private bool isMovable = false;
    [SerializeField]
    private EUnitType unitType = EUnitType.NONE;

    private Vector3 wayPointStart = Vector3.zero;
    private Transform targetBunker = null;

    private int barrackIdx = -1;
    private int listIdx = -1;
    private float oriAttRange = 0f;
    private bool isAttack = false;
    private bool isSelect = false;
}