using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    public void Start(ref SUnitState _structFSM)
    {
        throw new System.NotImplementedException();
    }
    public void Update(ref SUnitState _structFSM)
    {
        throw new System.NotImplementedException();
    }
    public void End(ref SUnitState _structFSM)
    {
        throw new System.NotImplementedException();
    }
}
