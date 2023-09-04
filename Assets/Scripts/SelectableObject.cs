using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public ESelectableObjectType ObjectType => objectType;
    public Vector3 GetPos => transform.position;

    public void Awake()
    {
        if (isControllable)
        {
            move = GetComponent<UnitMovement>();
            move.Init();
        }
    }

    //public void MoveToTargetPos(Vector3 _targetPos)
    //{
    //    if (move)
    //        move.MoveByTargetPos(_targetPos);
    //}

    public void FollowTarget(Transform _targetTr)
    {
        if (isControllable)
            move.FollowTarget(_targetTr);
    }

    public void MoveByMoveVec(Vector3 _moveVec)
    {
        if (isControllable)
            move.MoveByMoveVec(_moveVec);
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        if (isControllable)
            move.MoveByTargetPos(_targetPos);
    }

    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;

    private UnitMovement move = null;
}
