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

    public void Init(PF_Grid _grid)
    {

    }

    public void FollowTarget(Transform _targetTr)
    {
        if (isControllable)
            move.FollowTarget(_targetTr);
    }

    public void MoveByTargetPos(Vector3 _targetPos)
    {
        if (isControllable)
            move.MoveByTargetPos(_targetPos);
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
}
