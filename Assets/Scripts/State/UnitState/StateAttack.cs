using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : IState
{
    public void Start(ref SUnitState _structState)
    {
        targetTr = _structState.targetTr;
        attRate = _structState.attRate;
    }
    public void Update(ref SUnitState _structState)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < attRate)
        {
            elapsedTime = 0f;
            // 공격 애니메이션 출력
            targetTr.GetComponent<SelectableObject>().AttackDmg(_structState.attDmg);
        }
    }
    public void End(ref SUnitState _structState)
    {

    }

    private float elapsedTime = 0f;

    private float attRate = 0f;
    private Transform targetTr = null;
}
