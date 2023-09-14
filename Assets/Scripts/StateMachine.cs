using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public delegate void CurStateEnumDelegate(EState _curEState);

    public void Init(
        int _nodeIdx, 
        NodeUpdateDelegate _updateNodeCallback,
        CurStateEnumDelegate _curStateEnumCallback)
    {
        unitState.nodeIdx = _nodeIdx;
        unitState.updateNodeCallback = _updateNodeCallback;
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

    public float AttackRange => unitState.attRange;

    public void SetTargetTr(Transform _targetTr)
    {
        unitState.targetTr = _targetTr;
    }

    public int GetNodeIdx()
    {
        return unitState.nodeIdx;
    }

    public void EnqueueState(IState _state)
    {
        queueState.Enqueue(_state);
    }

    public void ClearQueue()
    {
        queueState.Clear();
    }

    public void PushState(IState _state)
    {
        stackState.Push(_state);
    }

    public void ClearStack()
    {
        stackState.Clear();
    }

    #region stateTest
    //public void FinishState()
    //{
    //    curState.End(ref unitState);
    //    curState = stackState.Pop();
    //    curStateCallback?.Invoke(curState);
    //    curState.Start(ref unitState);
    //}

    //public void ChangeState(IState _newState)
    //{
    //    curState.End(ref unitState);
    //    if (curState != _newState)
    //    {
    //        stackState.Push(curState);
    //        curState = _newState;
    //    }
    //    curStateCallback?.Invoke(curState);
    //    curState.Start(ref unitState);
    //}

    //public void ResetState()
    //{
    //    stackState.Clear();
    //    curState.End(ref unitState);
    //    curState = arrState[(int)EState.IDLE];
    //    curStateCallback?.Invoke(curState);
    //    curState.Start(ref unitState);
    //}
    #endregion

    #region estate test
    public void FinishStateEnum()
    {
        curState.End(ref unitState);
        curStateEnum = stackStateEnum.Pop();
        curStateEnumCallback?.Invoke(curStateEnum);
        curState = arrState[(int)curStateEnum];
        curState.Start(ref unitState);
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
        //curStateEnumCallback?.Invoke(curStateEnum);
        curState.Start(ref unitState);
    }

    public void ResetStateEnum()
    {
        stackStateEnum.Clear();

        curState.End(ref unitState);

        curStateEnum = EState.IDLE;
        curState = arrState[(int)curStateEnum];

        curStateEnumCallback?.Invoke(curStateEnum);
        curState.Start(ref unitState);
    }
    #endregion

    private void Update()
    {
        if (curState != null)
            curState.Update(ref unitState);
    }

    [SerializeField]
    private SUnitState unitState;

    private Queue<IState> queueState = new Queue<IState>();
    private Stack<IState> stackState = new Stack<IState>();
    private IState[] arrState = null;

    private Stack<EState> stackStateEnum = new Stack<EState>();
    private EState curStateEnum = EState.NONE;

    private IState curState = null;
    private CurStateEnumDelegate curStateEnumCallback = null;
}