using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFollow : IState
{
    public void Start(ref SUnitState _structState)
    {
        _structState.isHold = false;
        myTr = _structState.myTr;
        targetTr = _structState.targetTr;
        moveSpeed = _structState.moveSpeed;

        PF_PathRequestManager.RequestPath(myTr.position, targetTr.position, OnPathFound);
    }

    public void Update(ref SUnitState _structState)
    {
        if (arrPath == null) return;
        
        myPos = myTr.position;

        elapsedTimeForCheckPath += Time.deltaTime;
        elapsedTimeForRequestPath += Time.deltaTime;

        if (elapsedTimeForCheckPath < checkPathDelay) return;
        if (elapsedTimeForRequestPath < checkPathDelay) return;

        if (Vector3.SqrMagnitude(myPos - curWayNode.worldPos) < 0.01f)
        {
            _structState.updateNodeCallback(myPos, _structState.nodeIdx);
            if (Vector3.SqrMagnitude(myPos - targetTr.position) < 3f) return;

            ++targetIdx;

            if (targetIdx >= arrPath.Length)
            {
                PF_PathRequestManager.RequestPath(myPos, targetTr.position, OnPathFound);
                elapsedTimeForRequestPath = 0f;
                return;
            }

            curWayNode = arrPath[targetIdx];
        }

        //if(elapsedTimeForRequestPath > RequestPathDelay)
        //{
        //    elapsedTimeForRequestPath = 0f;
        //    PF_PathRequestManager.RequestPath(myPos, targetTr.position, OnPathFound);
        //    return;
        //}

        if (Physics.Linecast(myPos, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
        {
            elapsedTimeForCheckPath = 0f;
            return;
        }

        if (!curWayNode.walkable)
        {
            PF_PathRequestManager.RequestPath(myPos, targetTr.position, OnPathFound);
            elapsedTimeForRequestPath = 0f;
            return;
        }

        myTr.rotation = Quaternion.LookRotation(curWayNode.worldPos - myPos);
        myTr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    public void End(ref SUnitState _structState)
    {
        _structState.updateNodeCallback(myTr.position, _structState.nodeIdx);

        arrPath = null;
        targetIdx = 0;
        curWayNode = null;
    }

    private void OnPathFound(PF_Node[] _newPath, bool _pathSuccessful)
    {
        if (_pathSuccessful)
        {
            arrPath = _newPath;
            targetIdx = 0;
            curWayNode = arrPath[0];
        }
    }

    private int targetIdx = 0;
    private float moveSpeed = 0f;

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;

    private float elapsedTimeForCheckPath = 1f;
    private float checkPathDelay = 0.4f;
    private float elapsedTimeForRequestPath = 10f;
    private float RequestPathDelay = 1f;

    private bool isCloseToTarget = false;

    private Vector3 myPos = Vector3.zero;
    private Transform targetTr = null;
    private Transform myTr = null;
}
