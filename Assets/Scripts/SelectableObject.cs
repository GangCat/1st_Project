using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public ESelectableObjectType ObjectType => objectType;
    public Vector3 GetPos => transform.position;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (isControllable)
        {
            move = GetComponent<UnitMovement>();
            move.Init();
        }

        structFSM.myTr = transform;
        structFSM.targetPos = Vector3.zero;
        structFSM.moveSpeed = 5f;

        idleState = null;
        moveState = new UnitMoveFSM();
        holdState = null;
        stopState = null;
        patrollState = null;

        curState = idleState;
    }


    public void ChangeState(IFSM _newState)
    {
        StartCoroutine("ChangeFSMCoroutine", _newState);
    }

    private IEnumerator ChangeFSMCoroutine(IFSM _newState)
    {
        if (curState != null)
        {
            // 언박싱, 값 복사 중 뭐가 더 비용이 적을지 생각
            curState.FSM_End(ref structFSM);
        }
            yield return null;
            curState = _newState;
            curState.FSM_Start(ref structFSM);
    }

    private void Update()
    {
        if(curState != null)
            curState.FSM_Update(ref structFSM);
    }


    public void FollowTarget(Transform _targetTr)
    {
        if (isControllable)
            move.FollowTarget(_targetTr);

        //ChangeState(moveState);
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        structFSM.targetPos = _targetPos;

        if (isControllable)
            ChangeState(moveState);
    }

    public void Stop()
    {
        move.Stop();
    }

    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;

    private UnitMovement move = null;

    private IFSM curState = null;
    private IFSM idleState = null;
    private IFSM moveState = null;
    private IFSM holdState = null;
    private IFSM stopState = null;
    private IFSM patrollState = null;

    private SFSM structFSM;

    private float moveSpeed = 5f;
}
