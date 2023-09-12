using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public ESelectableObjectType ObjectType => objectType;
    public Vector3 GetPos => transform.position;


    public void Init(int _nodeIdx, NodeUpdateDelegate _updateNodeCallback = null)
    {
        structState.updateNodeCallback = _updateNodeCallback;
        structState.nodeIdx = _nodeIdx;

        if (isControllable)
        {
            move = GetComponent<UnitMovement>();
            move.Init();
        }

        if (isControllable)
        {
            IState stateIdle = new StateIdle();
            IState stateMove = new StateMove();
            IState stateStop = new StateStop();
            IState stateHold = new StateHold();
            IState statePatrol = new StatePatrol();
            IState stateAttack = new StateAttack();
            IState stateTrace = new StateTrace();
            IState stateFollow = new StateFollow();

            structState.arrState = new IState[(int)EState.LENGTH];

            structState.arrState[(int)EState.IDLE] = stateIdle;
            structState.arrState[(int)EState.MOVE] = stateMove;
            structState.arrState[(int)EState.STOP] = stateStop;
            structState.arrState[(int)EState.HOLD] = stateHold;
            structState.arrState[(int)EState.PATROL] = statePatrol;
            structState.arrState[(int)EState.ATTACK] = stateAttack;
            structState.arrState[(int)EState.TRACE] = stateTrace;
            structState.arrState[(int)EState.FOLLOW] = stateFollow;

            structState.myTr = transform;
            structState.callback = ChangeState;
        
            curState = stateIdle;
        }
    }

    public void AttackDmg(int _dmg)
    {
        Debug.LogFormat("Hit Dmg {0}", _dmg);
    }

    public void ChangeState(IState _newState)
    {
        if(isControllable)
            StartCoroutine("ChangeFSMCoroutine", _newState);
    }

    private IEnumerator ChangeFSMCoroutine(IState _newState)
    {
        // 언박싱, 값 복사 중 뭐가 더 비용이 적을지 생각
        curState.End(ref structState);
        yield return null;
        curState = _newState;
        curState.Start(ref structState);
    }

    private void Update()
    {
        if(isControllable)
            curState.Update(ref structState);
    }


    public void FollowTarget(Transform _targetTr)
    {
        structState.targetTr = _targetTr;
        if (isControllable)
            ChangeState(structState.arrState[(int)EState.FOLLOW]);
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        structState.targetPos = _targetPos;

        if (isControllable)
            ChangeState(structState.arrState[(int)EState.MOVE]);
    }

    public void MoveAttack(Vector3 _targetPos)
    {
        structState.targetPos = _targetPos;
        structState.isAttackMove = true;

        if (isControllable)
            ChangeState(structState.arrState[(int)EState.MOVE]);
    }

    public void Patrol(Vector3 _wayPointTo)
    {
        structState.targetPos = _wayPointTo;

        if (isControllable)
            ChangeState(structState.arrState[(int)EState.PATROL]);
    }

    public void Stop()
    {
        if (isControllable)
            ChangeState(structState.arrState[(int)EState.STOP]);
    }

    public void Hold()
    {
        if (isControllable)
            ChangeState(structState.arrState[(int)EState.HOLD]);
    }

    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;
    [SerializeField]
    private SUnitState structState;

    private UnitMovement move = null;

    private IState curState = null;
}
