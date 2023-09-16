using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public delegate void CurStateEnumDelegate(EState _curEState);

    public void Init(CurStateEnumDelegate _curStateEnumCallback)
    {
        curStateEnumCallback = _curStateEnumCallback;

        IState stateIdle = new StateIdle();
        IState stateMove = new StateMove();
        IState stateStop = new StateStop();
        IState stateHold = new StateHold();
        IState stateAttack = new StateAttack();

        arrState = new IState[(int)EState.LENGTH];

        arrState[(int)EState.IDLE] = stateIdle;
        arrState[(int)EState.MOVE] = stateMove;
        arrState[(int)EState.STOP] = stateStop;
        arrState[(int)EState.HOLD] = stateHold;
        arrState[(int)EState.ATTACK] = stateAttack;

        unitState.myTr = transform;

        curState = stateIdle;
        curStateEnum = EState.IDLE;
        stackStateEnum.Push(curStateEnum);
    }

    public void SetMyTr(Transform _myTr)
    {
        unitState.myTr = _myTr;
    }

    public Vector3 TargetPos
    {
        get => unitState.targetPos;
        set => unitState.targetPos = value;
    }

    public Transform TargetTr
    {
        get => unitState.targetTr;
        set => unitState.targetTr = value;
    }

    public void SetWaitForNewPath(bool _isWaiting)
    {
        unitState.isWaitForNewPath = _isWaiting;
    }

    public void FinishStateEnum()
    {
        curStateEnumCallback?.Invoke(stackStateEnum.Pop());
    }

    public void ChangeStateEnum(EState _newState)
    {
        curState.End(ref unitState);
        if (curStateEnum != _newState)
        {
            stackStateEnum.Push(curStateEnum);
            curStateEnum = _newState;
            curState = arrState[(int)curStateEnum];
        }
        curState.Start(ref unitState);
    }

    public void ResetStateEnum()
    {
        stackStateEnum.Clear();
        curStateEnumCallback?.Invoke(EState.IDLE);
    }

    private void Update()
    {
        if (curState != null)
            curState.Update(ref unitState);
    }

    [SerializeField]
    private SUnitState unitState;

    private IState[] arrState = null;
    private IState curState = null;

    private Queue<EState> queueStateEnum = new Queue<EState>();
    private Stack<EState> stackStateEnum = new Stack<EState>();
    private EState curStateEnum = EState.NONE;

    private CurStateEnumDelegate curStateEnumCallback = null;
}