using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : IState
{
    public void Start(ref SUnitState _structState)
    {
        _structState.isHold = false;
        myTr = _structState.myTr;
        moveSpeed = _structState.moveSpeed;

        PF_PathRequestManager.RequestPath(myTr.position, _structState.targetPos, OnPathFound);
    }

    public void Update(ref SUnitState _structState)
    {
        if (arrPath == null) return;

        myPos = myTr.position;

        if (_structState.isAttackMove)
        {
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
                            _structState.callback(_structState.listState[(int)EState.TRACE]);
                        }
                    }
                }
            }
        }

        elapsedTimeForCheckPath += Time.deltaTime;

        if (elapsedTimeForCheckPath < checkPathDelay) return;

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
                _structState.callback(_structState.listState[(int)EState.STOP]);
                return;
            }

            curWayNode = arrPath[targetIdx];
        }

        if (!curWayNode.walkable)
        {
            PF_PathRequestManager.RequestPath(myPos, arrPath[arrPath.Length - 1].worldPos, OnPathFound);
            elapsedTimeForCheckPath = 0f;
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
    private float elapsedTimeForCheckEnemy = 0f;
    private float checkEnemyDelay = 0.1f;
    private float elapsedTimeForCheckPath = 1f;
    private float checkPathDelay = 0.2f;

    private Transform myTr = null;
    private Transform targetTr = null;
    private Vector3 myPos = Vector3.zero;

    private PF_Node[] arrPath = null;
    private PF_Node curWayNode = null;
}