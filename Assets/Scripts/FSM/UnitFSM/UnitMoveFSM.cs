using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UnitMoveFSM : IFSM
{
    public void FSM_Start(ref SFSM _structFSM)
    {
        PF_PathRequestManager.RequestPath(_structFSM.myTr.position, _structFSM.targetPos, OnPathFound);
    }

    public void FSM_Update(ref SFSM _structFSM)
    {
        Vector3 myPos = _structFSM.myTr.position;
        if (arrPath == null)
            return;

        if (Physics.Linecast(myPos, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
        {
            // 0.5초 대기 타이머
            return;
        }

        if (Vector3.SqrMagnitude(myPos - curWayNode.worldPos) < 0.01f)
        {
            ++targetIdx;
            if (targetIdx >= arrPath.Length)
            {
                return;
            }

            curWayNode = arrPath[targetIdx];

            if (!curWayNode.walkable)
            {
                PF_PathRequestManager.RequestPath(myPos, arrPath[arrPath.Length - 1].worldPos, OnPathFound);
            }
        }

        _structFSM.myTr.rotation = Quaternion.LookRotation(curWayNode.worldPos - myPos);
        _structFSM.myTr.Translate(Vector3.forward * _structFSM.moveSpeed * Time.deltaTime);
    }

    public void FSM_End(ref SFSM _structFSM)
    {

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

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;
}