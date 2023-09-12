using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStop : IState
{
    public void Start(ref SUnitState _structState)
    {
        _structState.isHold = false;
        _structState.isAttackMove = false;
    }
    public void Update(ref SUnitState _structState)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > stopDelayTime)
        {
            _structState.callback(_structState.arrState[(int)EState.IDLE]);
        }

    }
    public void End(ref SUnitState _structState)
    {
    }

    private float elapsedTime = 0f;
    private float stopDelayTime = 1f;
}
