using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Start(ref SUnitState _structState);
    public void Update(ref SUnitState _structFSM);
    public void End(ref SUnitState _structFSM);
}