using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : IState
{
    public void Start(ref SUnitState _structState)
    {
        myTr = _structState.myTr;
        targetTr = _structState.targetTr;
        attRate = _structState.attRate;
        attDmg = _structState.attDmg;
    }

    public void Update(ref SUnitState _structState)
    {
        if (targetTr != null)
        {
            myTr.rotation = Quaternion.LookRotation(targetTr.position - myTr.position);
            elapsedTime += Time.deltaTime;
            if (elapsedTime < attRate)
            {
                elapsedTime = 0f;
                // ���� �ִϸ��̼� ���
                targetTr.GetComponent<SelectableObject>().AttackDmg(attDmg);
            }
        }
    }

    public void End(ref SUnitState _structState)
    {

    }

    private int attDmg = 0;
    private float elapsedTime = 0f;
    private float attRate = 0f;

    private Transform targetTr = null;
    private Transform myTr = null;
}
