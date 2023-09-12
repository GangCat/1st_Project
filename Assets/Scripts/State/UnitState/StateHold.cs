using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHold : IState
{
    public void Start(ref SUnitState _structState)
    {
        _structState.isHold = true;
        myPos = _structState.myTr.position;
        elapsedTime = 0f;
    }

    public void Update(ref SUnitState _structState)
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > checkDelay)
        {
            elapsedTime = 0f;
            Collider[] arrCollider = null;
            arrCollider = Physics.OverlapSphere(myPos, _structState.attRange);

            if(arrCollider.Length > 0)
            {
                foreach(Collider c in arrCollider)
                {
                    if (c.CompareTag("EnemyUnit"))
                    {
                        _structState.targetTr = c.transform;
                        _structState.callback(_structState.arrState[(int)EState.ATTACK]);
                    }
                }
            }
        }

    }

    public void End(ref SUnitState _structState)
    {
    }

    private Vector3 myPos = Vector3.zero;

    private float elapsedTime = 0f;
    private float checkDelay = 0.1f;
}
