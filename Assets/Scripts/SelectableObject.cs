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
            move = GetComponent<UnitMovement>();
    }

    public void MoveToTargetPos(Vector3 _targetPos)
    {
        if (move)
            move.MoveToTargetPos(_targetPos);
    }

    [SerializeField]
    private ESelectableObjectType objectType = ESelectableObjectType.None;
    [SerializeField]
    private bool isControllable = false;

    private UnitMovement move = null;
}
