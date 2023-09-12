using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : IState
{
    public void Start(ref SUnitState _structState)
    {
        myTr = _structState.myTr;
        targetTr = _structState.targetTr;
        attRange = _structState.attRange;
        attRate = _structState.attRate;
    }
    public void Update(ref SUnitState _structState)
    {
        elapsedTime += Time.deltaTime;

        if (Vector3.SqrMagnitude(myTr.position - targetTr.position) < Mathf.Pow(attRange, 2))
        {
            if (elapsedTime > attRate)
            {
                elapsedTime = 0f;

                // 공격 애니메이션 재생

                targetTr.GetComponent<SelectableObject>().AttackDmg(_structState.attDmg);
            }
            myTr.rotation = Quaternion.LookRotation(targetTr.position - myTr.position);
        }
        else
        {
            if (_structState.isHold)
            {
                _structState.callback(_structState.arrState[(int)EState.HOLD]);
            }
            else
            {
                _structState.callback(_structState.arrState[(int)EState.TRACE]);
            }
        }

    }
    public void End(ref SUnitState _structState)
    {
    }

    private float elapsedTime = 0f;

    private float attRange = 0f;
    private float attRate = 0f;
    private Transform targetTr = null;
    private Transform myTr = null;
}
