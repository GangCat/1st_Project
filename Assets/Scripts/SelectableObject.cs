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

            structState.listState = new List<IState>();

            structState.listState.Add(stateIdle);
            structState.listState.Add(stateMove);
            structState.listState.Add(stateStop);
            structState.listState.Add(stateHold);
            structState.listState.Add(statePatrol);
            structState.listState.Add(stateAttack);
            structState.listState.Add(stateTrace);
            structState.listState.Add(stateFollow);

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
        //if(curState != null)
            curState.Update(ref structState);
    }


    public void FollowTarget(Transform _targetTr)
    {
        if (isControllable)
            move.FollowTarget(_targetTr);

        //ChangeState(moveState);
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        structState.targetPos = _targetPos;

        if (isControllable)
            ChangeState(structState.listState[(int)EState.MOVE]);
    }

    public void MoveAttack(Vector3 _targetPos)
    {
        structState.targetPos = _targetPos;
        structState.isAttackMove = true;

        if (isControllable)
            ChangeState(structState.listState[(int)EState.MOVE]);
    }

    public void Stop()
    {
        move.Stop();
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
