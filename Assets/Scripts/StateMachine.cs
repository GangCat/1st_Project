using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    
    public void Init(int _nodeIdx, NodeUpdateDelegate _updateNodeCallback)
    {
        unitState.updateNodeCallback = _updateNodeCallback;
        unitState.nodeIdx = _nodeIdx;

        IState stateIdle = new StateIdle();
        IState stateMove = new StateMove();
        IState stateStop = new StateStop();
        IState stateHold = new StateHold();
        IState statePatrol = new StatePatrol();
        IState stateAttack = new StateAttack();
        IState stateTrace = new StateTrace();
        IState stateFollow = new StateFollow();

        arrState = new IState[(int)EState.LENGTH];

        arrState[(int)EState.IDLE] = stateIdle;
        arrState[(int)EState.MOVE] = stateMove;
        arrState[(int)EState.STOP] = stateStop;
        arrState[(int)EState.HOLD] = stateHold;
        arrState[(int)EState.PATROL] = statePatrol;
        arrState[(int)EState.ATTACK] = stateAttack;
        arrState[(int)EState.TRACE] = stateTrace;
        arrState[(int)EState.FOLLOW] = stateFollow;

        unitState.myTr = transform;
        unitState.callback = ChangeState;

        curState = stateIdle;
        stackState.Push(curState);
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
    public float AttackRate => unitState.attRate;

    public void SetTargetTr(Transform _targetTr)
    {
        unitState.targetTr = _targetTr;
    }

    public int GetNodeIdx()
    {
        return unitState.nodeIdx;
    }

    public IState GetState(int _idx)
    {
        return arrState[_idx];
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

    public void FinishState()
    {
        curState.End(ref unitState);
        curState = stackState.Pop();
        curState.Start(ref unitState);
    }

    public void ChangeState(IState _newState)
    {
        curState.End(ref unitState);
        if (curState != _newState)
        {
            stackState.Push(curState);
            curState = _newState;
        }
        curState.Start(ref unitState);
    }



    private void Update()
    {
        if(curState != null)
            curState.Update(ref unitState);
    }


    [SerializeField]
    private SUnitState unitState;

    private Queue<IState> queueState = new Queue<IState>();
    private Stack<IState> stackState = new Stack<IState>();
    private IState[] arrState = null;

    private IState curState = null;
}
