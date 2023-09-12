using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePatrol : IState
{
    public void Start(ref SUnitState _structState)
    {
        wayPointFrom = _structState.myTr.position;
        wayPointTo = _structState.targetPos;
        myTr = _structState.myTr;
        moveSpeed = _structState.moveSpeed;

        _structState.isHold = false;

        PF_PathRequestManager.RequestPath(wayPointFrom, wayPointTo, OnPathFound);
    }

    public void Update(ref SUnitState _structState)
    {
        if (arrPath == null) return;

        myPos = myTr.position;

        elapsedTimeForCheckEnemy += Time.deltaTime;

        if (elapsedTimeForCheckEnemy > checkEnemyDelay)
        {
            elapsedTimeForCheckEnemy = 0f;
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(myPos, _structState.traceStartRange);

            if (arrCollider.Length > 0)
            {
                foreach (Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        _structState.targetTr = c.transform;
                        _structState.callback(_structState.arrState[(int)EState.TRACE]);
                        return;
                    }
                }
            }
        }

        elapsedTimeForCheckPath += Time.deltaTime;
        elapsedTimeForRequestPath += Time.deltaTime;

        if (elapsedTimeForCheckPath < checkPathDelay) return;
        if (elapsedTimeForRequestPath < RequestPathDelay) return;

        if (!curWayNode.walkable)
        {
            PF_PathRequestManager.RequestPath(myPos, arrPath[arrPath.Length - 1].worldPos, OnPathFound);
            elapsedTimeForCheckPath = 0f;
            return;
        }

        if (Physics.Linecast(myPos, curWayNode.worldPos, 1 << LayerMask.NameToLayer("SelectableObject")))
        {
            elapsedTimeForCheckPath = 0f;
            return;
        }

        if (Vector3.SqrMagnitude(myPos - curWayNode.worldPos) < 0.01f)
        {
            ++targetIdx;
            if (targetIdx >= arrPath.Length)
            {
                //_structState.updateNodeCallback(myTr.position, _structState.nodeIdx);
                wayPointTo = wayPointFrom;
                wayPointFrom = myTr.position;
                PF_PathRequestManager.RequestPath(wayPointFrom, wayPointTo, OnPathFound);
                return;
            }

            curWayNode = arrPath[targetIdx];
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

    public void OnDrawGizmos()
    {
        if (arrPath != null)
        {
            for (int i = targetIdx; i < arrPath.Length; ++i)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(arrPath[i].worldPos, Vector3.one * 0.4f);

                if (i == targetIdx)
                    Gizmos.DrawLine(myTr.position, arrPath[i].worldPos);
                else
                    Gizmos.DrawLine(arrPath[i - 1].worldPos, arrPath[i].worldPos);
            }
        }
    }

    private Vector3 wayPointFrom = Vector3.zero;
    private Vector3 wayPointTo = Vector3.zero;

    private int targetIdx = 0;
    private float moveSpeed = 0f;

    private float elapsedTimeForCheckEnemy = 0f;
    private float checkEnemyDelay = 0.1f;
    private float elapsedTimeForCheckPath = 1f;
    private float checkPathDelay = 0.2f;
    private float elapsedTimeForRequestPath = 1f;
    private float RequestPathDelay = 0.2f;

    private Transform myTr = null;
    private Vector3 myPos = Vector3.zero;

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;
}
