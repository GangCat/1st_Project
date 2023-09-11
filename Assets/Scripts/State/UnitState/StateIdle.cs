using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StateIdle : IState
{
    public void Start(ref SUnitState _structState)
    {
        _structState.isHold = false;
        _structState.isAttackMove = false;
        myPos = _structState.myTr.position;
        traceStartRange = _structState.traceStartRange;
    }

    public void Update(ref SUnitState _structState)
    {
        elapsedTimeForCheckEnemy += Time.deltaTime;

        if (elapsedTimeForCheckEnemy > checkEnemyDelay)
        {
            elapsedTimeForCheckEnemy = 0f;
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(myPos, traceStartRange);

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

    public void End(ref SUnitState _structState)
    {
    }

    private Vector3 myPos = Vector3.zero;

    private float elapsedTimeForCheckEnemy = 0f;
    private float checkEnemyDelay = 0.5f;
    private float traceStartRange = 0f;
}
